# VersionOne Bugzilla Test
# Use this script to verify that the VersionOne Bugzilla Webserice components are installed correctly
# Writen by a Perl Neophite with help from
#     XMLRPCsh.pl
#    The Bugzilla  bz_webservice_demo.pl script
#     All over the web.
use strict;

use File::Basename qw(dirname);
use File::Spec;
use HTTP::Cookies;
use XMLRPC::Lite;
use Data::Dumper; $Data::Dumper::Terse = 1; $Data::Dumper::Indent = 0;

my $serverUrl = shift;
my $Bugzilla_login = shift;
my $Bugzilla_password = shift;
my $showExtensions = shift;

# I tested this with a 0 and 1 and the results were the same.
# Therefore I've not included this as commandline option
my $Bugzilla_remember="0";

# Open our cookie jar. We save it into a file so that we may re-use cookies
# to avoid the need of logging in every time. You're encouraged, but not
# required, to do this in your applications, too.
# Cookies are only saved if Bugzilla's rememberlogin parameter is set to one of
#    - on
#    - defaulton (and you didn't pass 0 as third parameter to User.login)
#    - defaultoff (and you passed 1 as third parameter to User.login)
my $cookie_jar = new HTTP::Cookies('file' => File::Spec->catdir(dirname($0), 'cookies.txt'), 'autosave' => 0);
my $xmlrpc = XMLRPC::Lite->proxy($serverUrl, 'cookie_jar' => $cookie_jar)->on_fault(sub{});

# this is working since 3.2 version
# list of existing bugzilla extensions
if ($showExtensions && $showExtensions == 1) {
	my $soapresult = $xmlrpc->call('Bugzilla.extensions');
	_die_on_fault($soapresult);
	my $extensions = $soapresult->result()->{extensions};
	foreach my $extensionname (keys(%$extensions)) {
	    print "Extension '$extensionname' information\n";
	    my $extension = $extensions->{$extensionname};
	    foreach my $data (keys(%$extension)) {
	        print '  ' . $data . ' => ' . $extension->{$data} . "\n";
	    }
	}
}

print "\nConnect to Bugzilla instance located at ", $serverUrl, "\n";

if($Bugzilla_login =~ /\S/)
{
	print "Using Credentials ", $Bugzilla_login, "/", $Bugzilla_password, "\n";
	my $loginResult = $xmlrpc->call('User.login',
	                            { login => $Bugzilla_login,
	                              password => $Bugzilla_password,
	                              remember => $Bugzilla_remember } );
	showResult("Login", $loginResult);
}

showResult("V1.Version", $xmlrpc->call('V1.Version', ''));
showResult("Logout", $xmlrpc->call('User.logout'));


print "Done\n";

#
# Show the results of each call
# First parameter is function being called
# Second paremeter is result
#
sub showResult {
	my $function = shift;
    my $result = shift;
	defined($result) && $result->fault ? print(STDERR join "\n", "\n*** $function Failed ***", @{$result->fault}{'faultCode', 'faultString'}, '') :
	  !$xmlrpc->transport->is_success  ? print(STDERR join "\n", "\n***  TRANSPORT Failed when executing $function ***", $xmlrpc->transport->status, '')                 :
                                         print(STDERR "$function Result ", Dumper($result->paramsall), "\n");
	exit 1 if $result->fault
}

#
#
#
sub _die_on_fault {
    my $soapresult = shift;

    if ($soapresult->fault) {
        my ($package, $filename, $line) = caller;
        die $soapresult->faultcode . ' ' . $soapresult->faultstring .
            " in SOAP call near $filename line $line.\n";
    }
}