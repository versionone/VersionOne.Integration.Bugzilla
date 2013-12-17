This folder contains the Bugzilla script files that are required for the VersionOne Integration.

The following files need to be copied to your Bugzilla installation and have the appropriate permissions.

* the 'V1' folder from the 'extensions' directory need to copy to /<the_root_of_your_Bugzilla_installation>/extensions/


The following file can be used to verify the VersionOne Bugzilla extensions:

* TestV1Service.pl

perl TestV1Service.pl <Path_to_Bugzilla> <login> <password> <show_extensions>

<Path_to_Bugzilla> - path to the xmlrpc.cgi file. It is in the Bugzilla's base directory.(ex: http://bugzilla_domen/xmlrpc.cgi )

<login> - user name for login to bugzilla

<password> -  password for login to bugzilla

<show_extensions> - show list of set extensions (0 or 1) (optional)