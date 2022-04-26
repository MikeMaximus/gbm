Game Backup Monitor v1.3.1 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 26, 2022

New in 1.3.1

Disclaimer:

v1.3.1 is still in development, this file will be updated as changes are made.
Database changes made in development builds may not be compatible with the final release.  Use at your own risk!

All Platforms:

- Experimental support for importing configurations from a Ludusavi YAML manifest file.
	- Choose from thousands of backup configurations in the Ludusavi Manifest(https://github.com/mtkennerly/ludusavi-manifest) sourced from PCGamingWiki.
		- A "Ludusavi Manifest" option is available from all import menus, importing any file with the Ludusavi YAML format is also supported.
		- Ludusavi configurations are suitable for manual backups only, they cannot automatically detect running games until a process to monitor is selected for each configuration.
		- Windows and Linux are supported.
			- Linux displays configurations for native and windows/dos games.
		- Ludusavi configurations are unique and will not overwrite GBM's official or manually created configurations.		
		- This is a work in progress!
			- Certain configurations may not always convert to GBM correctly.
			- Configurations currently not supported (Ex. Steam Cloud save locations) will not be displayed for import.
- Improved saved game detection on the Import window.
- Fixed performance issues when filtering on the Import window with large data sets.
- Fixed an issue with the import feature not syncing immediately after being used from the main window or system tray.
	- This would cause imported changes to be lost if GBM was closed before a sync was triggered by another action.
- Improved the pending backup notification feature.	
	- Clicking notification now opens the Game Manager to the "Backup Management" tab with the first game selected.
	- The notification will no longer keep reappearing during a session when all pending backups are not restored.
- Improved method of saving bandwidth when importing files from the web.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html