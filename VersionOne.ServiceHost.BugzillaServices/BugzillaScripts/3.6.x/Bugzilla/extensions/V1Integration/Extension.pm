package Bugzilla::Extension::V1Integration;
use strict;
use base qw(Bugzilla::Extension);

our $VERSION = '1.3.6.1';

sub webservice {
    my ($self, $args) = @_;
    my $dispatch = $args->{dispatch};
    $dispatch->{V1} = "Bugzilla::Extension::V1Integration::V1";

}


__PACKAGE__->NAME;
