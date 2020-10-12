package Bugzilla::Extension::V1Integration;
use 5.10.1;
use strict;
use base qw(Bugzilla::Extension);

our $VERSION = '1.4.0.5';

sub webservice {
    my ($self, $args) = @_;
    my $dispatch = $args->{dispatch};
    $dispatch->{V1} = "Bugzilla::Extension::V1Integration::V1";

}


__PACKAGE__->NAME;
