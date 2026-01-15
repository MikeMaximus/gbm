Game Backup Monitor v1.4.4 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

January 15, 2026

New in 1.4.4

General:

- Added Russian language support.
	- Thank you oblako17 for providing the Russian translation and testing.
- The UI has been overhauled (again) for better multi-language support.
	- Forms and buttons are now much larger in general to support more text.	
	- Labels are now always above input fields to prevent issues with varying text length.
	- Some controls have changed locations to prevent issues with varying text length.
- Fixed an issue with the "Defaults" button on the Settings form.
	- Some settings were not being validated correctly after being set to default.
- The details section on the main form will now refresh properly after changing languages.
- Optimized manifest verfication.
- GBM now compares it's manifest to the current files in the backup folder before doing a backup.
	- This prevents possible issues from occuring if the user has manually deleted any backup files.
	- This only applies to games that save multiple backup files.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html