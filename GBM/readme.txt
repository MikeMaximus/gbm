Game Backup Monitor v1.3.1 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 21, 2022

New in 1.3.1

Disclaimer:

v1.3.1 is still in development, this file will be updated as changes are made.
Database changes made in development builds may not be compatible with the final release.  Use at your own risk!

All Platforms:

- Experimental support for importing configurations from a Ludusavi YAML manifest file.
	- This means you can now import thousands of backup configurations from the main Ludusavi manifest file sourced from PCGamingWiki.
		- This is currently not automatic, you must download the manifest yourself and use the "Import Games - > File" option.
		- Ludusavi configurations are suitable for manual backups only, they cannot automatically detect running games until the Process field is manually updated with the proper data.	
- Fixed performance issues when filtering on the Import window with large data sets.
- Fixed an issue with the import feature not syncing immediately after being used from the main window or system tray.
	- This would cause imported changes to be lost if GBM was closed before a sync was triggered by another action.
- Improved the pending backup notification feature.	
	- Clicking notification now opens the Game Manager to the "Backup Management" tab with the first game selected.
	- The notification will no longer keep reappearing during a session when all pending backups are not restored.
- Improved method of saving bandwidth when checking for game list updates.


The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html