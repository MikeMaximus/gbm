Game Backup Monitor v1.4.4 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

January 29, 2026

New in 1.4.4

General:

- Added Russian language support.
	- Thank you oblako17 for providing the Russian translation and testing.
- The UI has been overhauled (again) for better multi-language support.
	- Windows and buttons are now much larger in general to support more text.	
	- Labels are now always above input fields to prevent issues with varying text length.
	- Some controls have changed locations to prevent issues with varying text length.
- Added the ability to override image and icon resources.
	- This allows the user to easily change any images or icons in GBM to their own custom versions.
	- Custom images and icon files can be placed in a special folder to replace the defaults.
	- Please read the "Advanced Customization" section of the online manual to get started.  
- Fixed an issue with the backup and temporary folders not being found in portable mode if the start-up location of the app changes.
	- If you still experience this issue after upgrading, open the "Settings" window and click "Save" without changing anything, then try starting the app from another location.
- Fixed an issue with the "Defaults" button on the Settings form.
	- Some settings were not being validated correctly after being set to default.
- The details section on the main window will now refresh properly after changing languages.
- GBM now compares it's manifest to the current files in the backup folder before doing a backup.
	- This prevents possible issues from occuring if the user has manually deleted any backup files.
	- This only applies to games that save multiple backup files.	

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html