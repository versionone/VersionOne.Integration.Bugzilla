# -*- Mode: perl; indent-tabs-mode: nil -*-
use strict;

package Bugzilla::WebService::V1;

use base qw(Bugzilla::WebService);

import SOAP::Data qw(type);

use Bugzilla;
use Bugzilla::Bug;
use Bugzilla::Constants;
use Bugzilla::Error;
use Bugzilla::User;
use Bugzilla::Field;
use Bugzilla::Util qw(trim);
use Bugzilla::Token;

use Time::Zone;

sub Version {
        return "1.0.0.0";
}

sub Login {
        my $username = @_[1];
        my $password = @_[2];
        my $remember = @_[3];

        if (defined($remember)) {
                $remember = $remember ? 'on' : '';
        }

        my $cgi = Bugzilla->cgi;

        $cgi->param('Bugzilla_login', $username);
        $cgi->param('Bugzilla_password', $password);
        $cgi->param('Bugzilla_remember', $remember);

        Bugzilla->login;

        return type('int')->value(Bugzilla->user->id);
}

sub Logout {
    my $self = shift;
    Bugzilla->logout;
    return undef;
}

sub GetBugs {
        # the first argument passed to us is the name of the saved search
        my $searchname = @_[1];

        # this action requires the user to be logged in
        Bugzilla->login(LOGIN_REQUIRED);

        # get all the saved searches this user has
        my $list = Bugzilla->user->queries();

        my $size = @$list;
        my $i = 0;
        # loop over the saved searches finding the one whose name matches searchname
        for ($i=0;$i<$size;$i++)
        {
                my $query = @$list[$i];
                # if this saved search's name matches the one we want
                if ($query->name eq $searchname)
                {
                        my $url = $query->url;
                        $ENV{'REQUEST_METHOD'} = 'GET';
                        my $params = new Bugzilla::CGI($url);

                        # define the mapping between sql columns and final dictionary field names
                        my @selectcolumns = ("bug_id");
                        my @selectnames = ("bugs.bug_id");

                        # ask the Search code to generate some sql for us based on the named search parameters
                        my $search = new Bugzilla::Search(
                                'fields' => \@selectnames,
                                'params' => $params
                        );
                        my $sql = $search->getSQL();

                        my $dbh = Bugzilla->switch_to_shadow_db();

                        # set the signal handlers to prevent denial-of-service
                        $::SIG{TERM} = 'DEFAULT';
                        $::SIG{PIPE} = 'DEFAULT';

                        # prepare the sql statement and execute it
                        my $buglist_sth = $dbh->prepare($sql);
                        $buglist_sth->execute();

                        my @bugs = ();

                        while (my @row = $buglist_sth->fetchrow_array()) {
                                my $bug = {};
                                foreach my $column (@selectcolumns) {
                                        $bug->{$column} = shift @row;
                                }
                                push(@bugs,$bug->{'bug_id'});
                        }

                        return \@bugs;
                }
        }
}


sub _GetBug {
        my $bugid = @_[0];

        ValidateBugID($bugid);

        my $bug = new Bugzilla::Bug($bugid);

	$bug->longdescs();
	my $description = $bug->{'longdescs'}->[0]->{'body'};

        my %item;
        $item{'id'} = type('int')->value($bug->bug_id);
        $item{'name'} = type('string')->value($bug->short_desc);
        $item{'description'} = type('string')->value($description);
        $item{'productid'} = type('int')->value($bug->product_id);
        $item{'componentid'} = type('int')->value($bug->component_id);
        $item{'assignedtoid'} = type('int')->value($bug->assigned_to->id);
        return \%item;
}

sub GetBug {
        return _GetBug(@_[1]);
}

sub GetProduct {
        my $productid = @_[1];
        my $project = new Bugzilla::Product($productid);
        return $project;
}

sub GetUser {
        my $userid = @_[1];
        my $user = new Bugzilla::User($userid);

        my %user;
        $user{'id'} = type('int')->value($user->id);
        $user{'name'} = type('string')->value($user->name);
        $user{'login'} = type('string')->value($user->login);
        return \%user;
}

sub _UpdateField {
        my $bugid = @_[0];
        my $fieldname = @_[1];
        my $fieldvalue = @_[2];

        my $sql = "Update bugs set " . $fieldname . " = " . $fieldvalue . " where bug_id = " . $bugid;

        Bugzilla->dbh->do($sql);
}

sub _IsCommentRequired {
	my ($function) = (@_);
    
	# Param is 1 if comment should be added !
    	my $ret = Bugzilla->params->{ "commenton" . $function };

    	# Allow without comment in case of undefined Params.
    	$ret = 0 unless ( defined( $ret ));

	return $ret;	
}

sub ResolveBug {
	my $bugid = @_[1];
	my $resolution = @_[2];
	my $comment = @_[3];

	# system could be setup to required comments
	if (($comment eq '') && _IsCommentRequired('resolve'))
	{
		ThrowUserError("comment_required");
	}	

	# ensure the resolution is a valid one
	check_field('resolution', $resolution, Bugzilla::Bug->settable_resolutions);	

	# don't resolve as fixed while still unresolved blocking bugs
	if (Bugzilla->params->{"noresolveonopenblockers"} && $resolution eq 'FIXED')
	{
		my @idlist = ($bugid);
		my @dependencies = Bugzilla::Bug::CountOpenDependencies(@idlist);
		if (scalar @dependencies > 0) {
			ThrowUserError("still_unresolved_bugs",
				{
					dependencies     => \@dependencies,
					dependency_count => scalar @dependencies
				}
			);
            	}
        }


	# RESOLVED bugs should have no time remaining	
	_UpdateField($bugid,'remaining_time',0);
	
	# Change resolution
	_ChangeResolution($bugid,$resolution);

	# update the status
	_ChangeStatus($bugid, 'RESOLVED');

        return type('boolean')->value(1);
}

sub _ChangeResolution {
	my ($bug_id, $str) = (@_);
	my $dbh = Bugzilla->dbh;
	my $cgi = Bugzilla->cgi;
	my $PrivilegesRequired = 0;
	my $vars = {};

	# Make sure the user is allowed to change the resolution.
	# If the user is changing several bugs at once using the UI,
	# then he has enough privs to do so. In the case he is hacking
	# the URL, we don't care if he reads --UNKNOWN-- as a resolution
	# in the error message.
	my $old_resolution = '-- UNKNOWN --';
	$old_resolution =
		$dbh->selectrow_array('SELECT resolution FROM bugs WHERE bug_id = ?',
			undef, $bug_id);
	my $bug = new Bugzilla::Bug($bug_id);
	unless ($bug->check_can_change_field('resolution', $old_resolution, $str,
		\$PrivilegesRequired))
	{
		$vars->{'oldvalue'} = $old_resolution;
		$vars->{'newvalue'} = $str;
		$vars->{'field'} = 'resolution';
		$vars->{'privs'} = $PrivilegesRequired;
		ThrowUserError("illegal_change", $vars);
	}
	_UpdateField($bug_id,'resolution',$dbh->quote($str));
}

sub AcceptBug {
	my $bugid = @_[1];
	if (Bugzilla->params->{"usetargetmilestone"}  && Bugzilla->params->{"musthavemilestoneonaccept"})
        {
		ThrowUserError("milestone_required", { bug_id => $bugid });
        }
	_ChangeStatus($bugid,"ASSIGNED");
	return type('boolean')->value(1);
}


my %usercache = ();
sub ReassignBug {
        my $bugid = @_[1];
        my $assignto = @_[2];

        #validate the assign to user
        my $assignee = login_to_id(trim($assignto), THROW_ERROR);
        if (Bugzilla->params->{"strict_isolation"}) {
                $usercache{$assignee} ||= Bugzilla::User->new($assignee);
                my $assign_user = $usercache{$assignee};
                my $bug = _GetBug($bugid);
                my $productid = $bug->{'productid'};
                if (!$assign_user->can_edit_product($productid)) {
                        my $product_name = Bugzilla::Product->new($productid)->name;
                        ThrowUserError('invalid_user_group',
                        {
                                'users'   => $assign_user->login,
                                'product' => $product_name,
                                'bug_id' => $bugid
                        });
                }
        }

        # change the assign to field
        _UpdateField($bugid,'assigned_to',$assignee);

        # update the status
        _ChangeStatus($bugid,'NEW');

        return type('boolean')->value(1);
}

sub UpdateBug {
	my $bugid = @_[1];
	my $field = @_[2];
	my $value = @_[3];

	my $f = new Bugzilla::Field({name => $field });
	if ($f && !$f->obsolete)
	{
		if ($f->type == FIELD_TYPE_SINGLE_SELECT) {
			check_field($field, $value, $f->legal_values);
		}
		_UpdateField($bugid, $field, Bugzilla->dbh->quote($value));
		return type('boolean')->value(1);
	}

        return type('boolean')->value(0);
}

sub _ChangeStatus {
        my $bugid = @_[0];
        my $status = @_[1];

	if (is_open_state($status)) 
	{
            # The logic for this block is:
            # If the new state is open:
            #   If the old state was open
            #     If the bug was confirmed
            #       - move it to the new state
            #     Else
            #       - Set the state to unconfirmed
            #   Else
            #     - leave it as it was


		my @open_state = map(Bugzilla->dbh->quote($_), BUG_STATE_OPEN);	
		my $open_state = join(", ", @open_state);

		my $sqlvalue = "CASE 
				WHEN bug_status IN($open_state) THEN
					(CASE WHEN everconfirmed = 1 THEN 
						" . Bugzilla->dbh->quote($status) .
                        		" ELSE 
						'UNCONFIRMED' 
					END) 
				ELSE 
					bug_status 
				END";
		
		_UpdateField($bugid, 'bug_status', $sqlvalue);

        } 
	else 
	{
		_UpdateField($bugid, 'bug_status', Bugzilla->dbh->quote($status));
        }
	
}

1;
