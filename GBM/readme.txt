Game Backup Monitor v1.3.1 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 27, 2022

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
- Experimental official support for games that use Steam Cloud save locations.
	- GBM will attempt to auto-configure the custom path variables required for Steam Cloud locations on each start-up and show a one-time warning if it fails.
		- Future official list updates can now contain games that use Steam Cloud save location.
		- Ludusavi Manifest configurations that use a Steam Cloud location can be imported. (not implemented yet)
	- Users that created Steam Cloud path variables for their own configuations will not be affected, these configurations will still function.
		- The current variables may be automatically renamed to the official ones or duplicated depending on how they were setup.
		- See the "Steam Cloud" section of the online manual for details on how to migrate your current Steam Cloud variable(s) to the official ones if necessary. (not written yet)
- Improved saved game detection on the Import window.
- Fixed performance issues when filtering on the Import window with large data sets.
- Fixed an issue with the import feature not syncing immediately after being used from the main window or system tray.
	- This would cause imported changes to be lost if GBM was closed before a sync was triggered by another action.
- Fixed an issue with configuration paths not be updated correctly when changing a custom path variable name and path at the same time.
- Improved the pending backup notification feature.	
	- Clicking notification now opens the Game Manager to the "Backup Management" tab with the first game selected.
	- The notification will no longer keep reappearing during a session when all pending backups are not restored.
- Improved method of saving bandwidth when importing files from the web.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html