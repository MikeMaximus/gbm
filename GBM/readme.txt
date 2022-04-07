Game Backup Monitor v1.3.0 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 6, 2022

New in 1.3.0

All Platforms:

- New features for importing & exporting game configurations.
	- Added the ability to import any valid XML file from a HTTP or HTTPS URL.
		- This makes it a little easier to host and share your own configuration lists.
		- The last URL will be automatically saved to make it easier to repeatly check the same list.
	- Added the ability to use import and export features from the "File" menu on the main window and system tray.
	- XML files are now cached in GBM's temporary folder when importing from the internet.
	- Added an optional method of greatly reducing the bandwidth used when hosting XML files.
- System tray menu has been changed to prevent it from getting too large.
	- A "File" sub-menu has been added and some features have been moved to this menu.
- Removed the "Sync Game IDs" feature.
	- This feature was designed to ease upgrades from old versions of GBM and should no longer be required.
- Fixed an issue with some browse windows on the Game Manager not opening to the correct location when using path variables.

Windows:

- Updated 7-Zip to 21.07
- Due to TLS 1.2 support, GBM now requires .NET Framework v4.5 or later.	

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html