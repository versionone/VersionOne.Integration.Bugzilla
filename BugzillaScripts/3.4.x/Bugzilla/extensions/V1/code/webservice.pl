# -*- Mode: perl; indent-tabs-mode: nil -*-
use strict;
use warnings;
use Bugzilla;
my $dispatch = Bugzilla->hook_args->{dispatch};
$dispatch->{V1} = "extensions::V1::lib::V1";

