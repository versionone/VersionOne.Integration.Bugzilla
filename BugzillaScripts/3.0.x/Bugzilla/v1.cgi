#!/usr/bin/perl -wT

use strict;
use lib qw(.);

use Bugzilla;
use Bugzilla::Constants;

# Use an eval here so that runtests.pl accepts this script even if SOAP-Lite
# is not installed.
eval 'use XMLRPC::Transport::HTTP;
      use Bugzilla::WebService;';
$@ && ThrowCodeError('soap_not_installed');

Bugzilla->usage_mode(Bugzilla::Constants::USAGE_MODE_WEBSERVICE);

my $response = Bugzilla::WebService::XMLRPC::Transport::HTTP::CGI
    ->dispatch_with( {'V1' => 'Bugzilla::WebService::V1',
					  'User'     => 'Bugzilla::WebService::User',
					 })
    ->on_action(\&Bugzilla::WebService::handle_login)
    ->handle;
