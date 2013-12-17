This folder contains Bugzilla script files that are required for VersionOne Integration.

The following files need to be copied to your Bugzilla installation directory and have the appropriate permissions.

* the 'V1' folder from the 'extensions' directory should be copied to /<Bugzilla_root>/extensions/


The following file can be used to verify VersionOne Bugzilla extension:

* TestV1Service.pl

perl TestV1Service.pl <Path_to_Bugzilla> <login> <password> <show_extensions>

<Path_to_Bugzilla> - path to the xmlrpc.cgi file. It is in the Bugzilla's base directory.(ex: http://bugzilla/xmlrpc.cgi )

<login> - valid Bugzilla username
<password> -  matching password

<show_extensions> - show list of set extensions (0 or 1) (optional)