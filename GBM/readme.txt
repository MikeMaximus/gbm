Game Backup Monitor v1.3.0 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 30, 2022

New in 1.3.0

Disclaimer:

v1.3.0 is still in development, this file will be updated as changes are made.
This development version may include database changes and may not be compatible with the final release, it's not recommended to use this version outside of a testing environment.

All Platforms:

- New features for importing & exporting game configurations.
	- Added the ability to import any valid XML file from a HTTP or HTTPS URL.
	- Added the ability to use import and export features from the "File" menu on the main window and system tray.
	- Imported XML files are now cached in GBM's designated temporary folder.
	- Added support for signature files(SHA256) when importing XML from the web.
		- When applicable, GBM will now check for a signature file and compare it against the cached version of the file to see if it's changed before downloading it again.
		- Using a signature file is not required but will save bandwidth for the client and the web server.
		- More information on this feature will be available in the manual at a later date.
- System tray menu has been changed to prevent it from getting too large.
	- A "File" sub-menu has been added and some features have been moved to this menu.
- Removed the "Sync Game IDs" feature.
	- This feature was designed to ease upgrades from old versions of GBM and should no longer be required.
- Fixed an issue with some browse windows on the Game Manager not opening to the correct location when using path variables.

Windows:

- Updated 7-Zip to 21.07

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html