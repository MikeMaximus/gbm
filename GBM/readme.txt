Game Backup Monitor v1.3.1 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

May 15, 2022

New in 1.3.1

Disclaimer:

v1.3.1 is still in development, this file will be updated as changes are made.
Database changes made in development builds may not be compatible with the final release.  Use at your own risk!

All Platforms:

- Support for importing configurations from the primary Ludusavi Manifest or any file using the manifest strucuture.
	- The primary Ludusavi Manifest(https://github.com/mtkennerly/ludusavi-manifest) contains thousands of backup configurations sourced from PCGamingWiki(https://www.pcgamingwiki.com).
	- See the Ludusavi Manifest(https://mikemaximus.github.io/gbm-web/manual.html#ludusavi) section of the manual for more information about this feature.
- Official support for games that use Steam Cloud save locations.
	- The official game lists will now contain games that use Steam Cloud save locations.
	- GBM will auto-configure the custom path variables required for Steam Cloud locations on each start-up if they don't exist.	
		- See the Preconfigured Store Variables(https://mikemaximus.github.io/gbm-web/manual.html#storevariables) section of the manual for more information about this feature.
- UI improvements on the Import window.
	- Only configurations with detected saved games are shown and selected by default.
		- This should be much less confusing and intimidating for new users.
		- If no saved games are detected, the full list is shown.
	- The "Select All" checkbox should now function in a more expected manner.
	- Column sizes will not longer reset when filters are applied.
	- Added columns to display more information about each configuration.
	- Fixed performance issues when filtering or sorting large data sets.
- Improved saved game detection on the Import window.
	- Windows registry configurations are now detected.
	- Better detection of configurations that use file includes.
- The GameID will now automatically be used for the backup sub-folder and file name of any configuration using duplicate names, regardless of the global setting for names.
	- This is done as a safety measure to prevent unknowingly overwriting the backup file of another configuration using the same name.
	- Edit any duplicate configuration names and make them unique to prevent this from happening.
- Fixed an issue with the import feature not syncing immediately after being used from the main window or system tray.
	- This would cause imported changes to be lost if GBM was closed before a sync was triggered by another action.
- Fixed an issue with configuration paths not be updated correctly when changing a custom path variable name and path at the same time.
- Fixed an issue with the Include/Exclude builder in the Game Manager not opening to the correct folder when using a custom path variable in the "Game Path" and a relative "Save Path".
- Improved the pending backup notification feature.	
	- Clicking notification now opens the Game Manager to the "Backup Management" tab with the first game selected.
	- The notification will no longer keep reappearing during a session when all pending backups are not restored.
- Improved method of saving bandwidth when importing files from the web.

Linux:

- Fixed games running in Wine not being detected.
	- "wine-preloader, wine, wine64-preloader, wine64" will now be detected instead of just the preloader variants.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html