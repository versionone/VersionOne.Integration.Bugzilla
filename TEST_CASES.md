##VersionOne Integration for Bugzilla 4.4.2 Test Cases

The test cases outlined in this document are for verifying that the installation, configuration, and primary workflows of the VersionOne Integration for Bugzilla function as expected.  These test cases are by no means comprehensive, but essentially serve as a "smoke test" of the integration.

### Test Case 1: Verify Documentation

Step 1. Open documentation at http://versionone.github.io.VersionOne.Integration.Bugzilla/  
Step 2. Verify that system requirements are accurate.  
> Expected result: Documentation contains all pertinent system requirements.  

Step 3. Verify installation steps.  
Step 4. Verify screenshots.  
> Expected result: Screenshots should match what user will see during installation and completing workflow tasks.  

Step 5. Verify usage workflows.  
> Expected result: VersionOne and Bugzilla workflows produce desired work item tracking and visibility within VersionOne.  

### Test Case 2: Verify V1 Bugzilla Integration Installation  
*Assumption: Bugzilla is already installed, configured and running properly.*

Step 1. Backup existing V1 integration if prior version currently installed.  
Step 2. Download and install latest version of V1 Bugzilla Integration.    
Step 3. Determine your Bugzilla version number; based on that version number caopy and place or upgrade the appropriate VersionOne integration scripts that run on your existing Bugzilla server.  
> Expected result:  The following scripts are placed in the Bugzilla/extensions/V1 directory:  
>Config.pm  
>Extension.pm  
>TestV1Service.pl  
>V1.pm should be place in Bugzilla/extensions/V1/lib   
  
Step 4. Verify the V1 scripts have been properly installed.
>Expected result: After executing the perl command in the documentation, if your scripts have been installed properly you should see something very similar to:  
>*Connect to Bugzilla instance located at http://bugserver/Bugzilla/xmlrpc.cgi  
>Using Credentials administrator@example.com/password  
>Login Result {'id' => '1'}  
>V1.Version Result '1.4.0.5'  
>Logout Result ''  
>Done*  

### Test Case 3: Configure Bugzilla
Step 1. Select or create a new Bugzilla user that has rights to accept work, create, and modify bugs.  
Step 2. Decide on an integration methodology: Assigned Bugs or Tagged Bugs.

### Test Case 3A: Assigned Bugs
Step 1. All new bugs created in Bugzilla must be assigned to the user selected above.
Step 2. Create a Saved Search in Bugzilla per the documentation.
Step 3. Run that Saved Search to verify that desired bugs are retrieved.
>Expected result: Search returns any and all bugs that are assigned to the selected Bugzilla user with a Bugzilla status of CONFIRMED. 

### Test Case 3B: Tagged Bugs  
Step 1. Create a new Custom Field in Bugzilla per the documentation.  
>Expected result: Bugzilla now contains a custom field named "cf_versiononestate", with a description value of "VersionOne Defect State", and with enabled, legal values of "Created" and "Closed".  

Step 2. All new bugs created in Bugzilla must be saved with this new VersionOne Defect State custom created field set to the "Created" value.  
Step 3. Create a Saved Search in Bugzilla per the documentation.
>Expected result: Search returns any and all bugs where VersionOne Defect State equals "Created".  

### Test Case 4: VersionOne Defect URL  
Step 1. Create a Custom Field in Bugzilla per the documentation.  
>Expected result: Bugzilla contains a Custom Field named "cf_versiononeurl" and with a description value of "VersionOne Defect Url".  This description value becomes the title of the Custom Field.  

### Test Case 5: Configure VersionOne  
*Assumption: You are not using the V1 Team Edition*  

Step 1. Add "Bugzilla" to the V1 Global Source list.  
>Expected result: Navigating to Administration -> List Types -> Global tab and viewing the Source table, Bugzilla should be an entry in that table.  

Step 2. Bugzilla identifier will be stored in the V1 Reference field.  

### Test Case 6: Configure the V1 Integration  

Step 1. Run the ServiceHostConfigTool.exe; this should open in a window starting with a General Tab.  
Step 2. On the General Tab, specify your V1 connection details then click the Validate button.  
>Expected result: A message in green type will appear to the far left of the Validate button confirming that "VersionOne settings are valid".  

Step 3. In the tab tree on the left section of the window, select Workitems. 
>Workitems tab appears.  

Step 4. Specify the V1 field that will be storing the Bugzilla identifier; the default is the V1 Reference field.  
>Expected result: Reference Field Name should be set to Reference and the Disabled checkbox should remain unchecked.  

Step 5. In the tab tree on the left, select Bugzilla.  
>Expected result: Bugzilla tab should appear with two of it's own tabs labeled "Bugzilla Service Settings" and "Project and Priority Mappings" respectively.  

Step 6. On the "Bugzilla Service Settings" tab, complete all fields per the documentation deciding if you would like to Assign or Tag bugs in Bugzilla.  

Step 7. Map Bugzilla and V1 Project values.
>Expected result: Available V1 projects should appear in the left drop down list, Bugzilla products need to be entered manually.  Make sure to match the Bugzilla product name and spacing exactly as it appears in Bugzilla.  

Step 8. Map Bugzilla and V1 Priority values.  
>Expected result: Available V1 priority values should appear in the left drop down list, Bugzilla priority values will need to be entered manually.  Make sure to enter all Bugzilla priority values that will be utilized noting that a V1 priority may map to more than one Bugzilla priority. 

Step 9. Click the save icon in the upper left and exit the ServiceHostConfigTool.
>Expected result: You may get prompted to save changes upon exiting the tool, you can save again by clicking "Yes" and you will get prompted to rename or overwrite the existing config file.  DO NOT rename the config file, simply click the save button again and replace the existing file. This will just resave the same settings.  If you click "No" upon exiting the application, the tool will simply close and your saved changes will remain.  

### Test Case 7: Start the Integration  

Step 1. Now that all components have been configured, start the integration by navigating to your installation folder and double-click the VersionOne.ServiceHost.exe.  
>Expected result: A command window will open and display several [Info] messages followed by a [Startup] message. The integration will then begin polling Bugzilla new bugs and V1 for closed bugs and display information pertaining to the polling results in this window.  

### Test Case 8: Test the Integration  

Step 1. Make sure the Integration is running, Bugzilla is open, and your instance of VersionOne is open.  
Step 2. Create a new bug in Bugzilla:  
Step 2 A - Assigned. If using the Assigned method, make sure the new bug is assigned to the person or persons specified in the Saved Search created in Bugzilla.  
>Expected result: After creating the new bug, execute the Assigned Bugs Saved Search and ensure that the newly created bug appears in the search results.  

Step 2 B - Tagged. If using the Tagged method, make sure the VersionOne Defect State is set to the "Create Field Value" specified in the ServiceHostConfigTool and submit the bug.  
>Expected result: After creating the new bug, execute the Tagged Bugs Saved Search and ensure that the newly created bug appears in the search results.  

Step 3. Check V1 for your newly created Bugzilla Bug by viewing the specified V1 Project Backlog.  
>Expected result: If all is set up correctly, you should see your Bugzilla bug appear in the V1 Project Backlog set to the appropriate mapped priority after the specified polling time interval that was set in the ServiceHostConfigTool. 

Step 4. Check Bugzilla for an updated Status of your new bug.  
>Expected result: The Bugzilla bug should now have a status of IN_PROGRESS as it was updated by the Integration. 

Step 5. Close the defect in V1.  
>Expected result: The V1 defect should now appear closed in V1 and, after the specified polling time, that same bug should appear as closed in Bugzilla.


