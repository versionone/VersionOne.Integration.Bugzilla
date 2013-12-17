package Bugzilla::Extension::V1Integration::V1;
# -*- Mode: perl; indent-tabs-mode: nil -*-
use strict;

import SOAP::Data qw(type);

use Bugzilla;
use Bugzilla::Bug;
use Bugzilla::Constants;
use Bugzilla::Error;
use Bugzilla::User;
use Bugzilla::Field;
use Bugzilla::Util qw(trim);
use Bugzilla::Token;
use Bugzilla::Status;
use base qw(Bugzilla::WebService);
use Data::Dumper;

use Time::Zone;

sub Version {
	return "1.4.0.5";
}

sub GetBugs {
	# the first argument passed to us is the name of the saved search
	my $searchname = $_[1]->{searchphrase};

	# this action requires the user to be logged in
	Bugzilla->login(LOGIN_REQUIRED);

	# define the mapping between sql columns and final dictionary field names
	my @selectcolumns = ("bug_id");
	my $sql = _GetUrlForSearch($searchname);

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

sub _GetUrlForSearch {
    my $searchname = shift;

	my $url = _FindUserQueryByName($searchname);
	if (!defined $url) {
		ThrowCodeError('Search name "' . $searchname . '" doesn\'t exist.', { param => $searchname });
	}

	my @selectnames = ("bug_id");
	$ENV{'REQUEST_METHOD'} = 'GET';
	my $params = new Bugzilla::CGI($url);

	# ask the Search code to generate some sql for us based on the named search parameters
	my $search = new Bugzilla::Search(
		'fields' => \@selectnames,
		'params' => scalar $params->Vars
	);

	return $search->sql();
}

sub _FindUserQueryByName {
	my $queryName = shift;

	# get all the saved searches this user has
	my $list = Bugzilla->user->queries();
	my $size = @$list;
	my $i = 0;

	# loop over the saved searches finding the one whose name matches searchname
	for ($i=0;$i<$size;$i++) {
		my $query = @$list[$i];

		# if this saved search's name matches the one we want
		if ($query->name eq $queryName) {
			return $query->url;
		}
	}

	return undef;
}


sub _GetBug {
	my $bugid = $_[0];

	my $bug = Bugzilla::Bug->check($bugid);
	my $bug_desc = $bug->comments({ order => 'oldest_to_newest'});
	my $description = $bug_desc->[0]->body_full();

	my %item;
	$item{'id'} = SOAP::Data::type('int')->value($bug->bug_id);
	$item{'name'} = SOAP::Data::type('string')->value($bug->short_desc);

	if ($description ne '') {
		$item{'description'} = SOAP::Data::type('string')->value($description);
	} else {
		$item{'description'} = '';
	}

	$item{'description'} = SOAP::Data::type('string')->value($description);
	$item{'productid'} = SOAP::Data::type('int')->value($bug->product_id);
	$item{'componentid'} = SOAP::Data::type('int')->value($bug->component_id);
	$item{'assignedtoid'} = SOAP::Data::type('int')->value($bug->assigned_to->id);
	$item{'priority'} = SOAP::Data::type('string')->value($bug->priority);

	return \%item;
}

sub GetBug {
	return _GetBug($_[1]->{bugid});
}

sub GetProduct {
	my $productid = $_[1]->{productid};
	my $project = new Bugzilla::Product($productid);
	return $project;
}

sub GetUser {
	my $userid = $_[1]->{userid};
	my $user = new Bugzilla::User($userid);

	my %user;
	$user{'id'} = SOAP::Data::type('int')->value($user->id);
	$user{'name'} = SOAP::Data::type('string')->value($user->name);
	$user{'login'} = SOAP::Data::type('string')->value($user->login);
	return \%user;
}

sub _UpdateField {
	my $bugid = $_[0];
	my $fieldname = $_[1];
	my $fieldvalue = $_[2];
	my $dbh = Bugzilla->dbh;

	my $bug = new Bugzilla::Bug($bugid);

	my $oldValue;
	if ($fieldname eq "assigned_to") {
		$oldValue = $bug->$fieldname->id;
	} else {
		$oldValue = $bug->$fieldname;
	}
	if ($fieldname =~ /^(\w+)$/i) {
		$fieldname = $1;
	} else {
		ThrowCodeError('invalid_field_name', { field => $fieldname });
	}
	if ($fieldvalue =~ /^([\w\d:\.\?\&\/\:\=]+)$/i) {
		$fieldvalue = $1;
	} else {
		ThrowCodeError('param_invalid', { param => $fieldname });
	}

	$bugid = _ValidateBugId($bugid);

	$dbh->bz_start_transaction();

	$dbh->do("UPDATE bugs SET " . $fieldname . " = " . Bugzilla->dbh->quote($fieldvalue) . " WHERE bug_id = ?",
			undef,  $bugid);

	# add activity to log
	if ($oldValue ne $fieldvalue) {
		_SaveHistory($bugid, $fieldname,  $oldValue, $fieldvalue, $dbh);
	}

	$dbh->bz_commit_transaction();
}

sub _ValidateBugId {
	my $bugid = shift;

	if ($bugid =~ /^(\d+)$/) {
		$bugid = $1;
	} else {
		ThrowCodeError('bug_id_does_not_exist', { bug_id => $bugid });
	}

	return $bugid;
}

sub _SaveHistory {
	my ($bugid, $fieldname,  $oldValue, $fieldvalue, $dbh) = @_;

	$oldValue = defined $oldValue ? $oldValue : '';

	if ($fieldname eq "assigned_to") {
		$oldValue = new Bugzilla::User($oldValue)->name;
		$fieldvalue = new Bugzilla::User($fieldvalue)->name;
	}

	Bugzilla::Bug::LogActivityEntry($bugid, $fieldname,  $oldValue, $fieldvalue, Bugzilla->user->id,
		$dbh->selectrow_array("SELECT NOW()"));
}

sub GetFieldValue {
	my $bugid = $_[1]->{bugid};
	my $fieldname = $_[1]->{fieldname};
	my $dbh = Bugzilla->dbh;

	if ($fieldname!~/^[a-z0-9_]+$/i) {
		return "";
	}

	Bugzilla::Bug->check($bugid);

	my $fieldValue;
	if ($fieldname =~ /^(\w+)$/i) {
		$fieldname = $1;
	} else {
		ThrowCodeError('invalid_field_name', { field => $fieldname });
	}
	$bugid = _ValidateBugId($bugid);

	($fieldValue) = $dbh->selectrow_array("SELECT " . $fieldname . " FROM bugs WHERE bug_id=?", undef, $bugid);

	return SOAP::Data::type('string')->value($fieldValue);
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
	my $bugid = $_[1]->{bugid};
	my $resolution = $_[1]->{resolution};
	my $comment = $_[1]->{comment};

	$bugid = _ValidateBugId($bugid);

	my $bug = new Bugzilla::Bug($bugid);

	# ensure the resolution is a valid one
	Bugzilla::Bug::check_field('resolution', $resolution);

	# don't resolve as fixed while still unresolved blocking bugs
    if (Bugzilla->params->{"noresolveonopenblockers"} && $resolution eq 'FIXED' && scalar @{$bug->dependson}) {
        my $dep_bugs = Bugzilla::Bug->new_from_list($bug->dependson);
        my $count_open = grep { $_->isopened } @$dep_bugs;
        if ($count_open) {
            ThrowUserError("still_unresolved_bugs",
                           { bug_id => $bug->id, dep_count => $count_open });
        }
    }

	# update the status
	_ChangeStatus($bug, 'RESOLVED', $resolution);

	$bug->update();

	return SOAP::Data::type('boolean')->value(1);
}

sub AcceptBug {
	my $bugid = $_[1]->{bugid};

	$bugid = _ValidateBugId($bugid);

	my $bug = new Bugzilla::Bug($bugid);

	if (Bugzilla->params->{"usetargetmilestone"}  && Bugzilla->params->{"musthavemilestoneonaccept"})
	{
		ThrowUserError("milestone_required", { bug_id => $bugid });
	}
	# NOTE was: ASSIGNED, this status does not exist in default 4.0.1 setup
	_ChangeStatus($bug, "IN_PROGRESS");
	$bug->update();

	return SOAP::Data::type('boolean')->value(1);
}


my %usercache = ();

sub ReassignBug {
	my $bugid = $_[1]->{bugid};
	my $assignto = $_[1]->{assignto};
	$bugid = _ValidateBugId($bugid);
	my $bug = new Bugzilla::Bug($bugid);

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
	$bug->set_assigned_to($assignto);

	# NOTE was: NEW, this status does not exist in default 4.0.1 setup
	_ChangeStatus($bug, 'CONFIRMED');
	$bug->update();

	return SOAP::Data::type('boolean')->value(1);
}

sub UpdateBug {
	my $bugid = $_[1]->{bugid};
	my $field = $_[1]->{fieldname};
	my $value = $_[1]->{fieldvalue};
	$bugid = _ValidateBugId($bugid);

	my $f = new Bugzilla::Field({name => $field });

	if ($f && !$f->obsolete)
	{
		if ($f->type == FIELD_TYPE_SINGLE_SELECT) {
			Bugzilla::Bug::check_field($field, $value);
		}
		_UpdateField($bugid, $field, $value);
		return SOAP::Data::type('boolean')->value(1);
	}

	return SOAP::Data::type('boolean')->value(0);
}

sub _ChangeStatus {
	my $bug = $_[0];
	my $status = $_[1];
	my $resolution = $_[2];

	my @valid_statuses = @{$bug->status()->can_change_to()};

	if ( grep { $_->name eq $status} @valid_statuses ) {
		my $param;
		if ($resolution && $resolution ne "") {
			$bug->set_remaining_time(0);
			$param = {'resolution' => $resolution};
			$bug->add_comment("Resolution has changed to " . $resolution . " by VersionOne");
		}
		$bug->set_bug_status($status, $param);
	}
}

sub _SaveToLog {
	my $data = $_[0];
	my $source = $_[1];
	open(ERRORLOGFID, ">>./log.log");
	print ERRORLOGFID "$source:\n";
	print ERRORLOGFID "$data \n";
	close ERRORLOGFID;
}

1;
