Game Backup Monitor v1.1.5 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

October 4th, 2018

New in 1.1.5

All Platforms:

- Fixed a crash that could occur when renaming a game configuration.
- Rewrote portions of game detection:
	- The handling of duplicate game configurations has been flawed since regular expression support was added in v1.1.  This has been fixed.
	- GBM is now more efficient when detecting games by parameter and/or process path.
- Changed how save path variables are handled:
	- GBM no longer uses it's own format and naming scheme for path variables.    
	- GBM now uses Windows environment variables in game configurations.  
		- For example, *appdatalocal* is now %LOCALAPPDATA% in a Windows configuration.
		- GBM also uses some custom environment variables, such as %USERDOCUMENTS% and %COMMONDOCUMENTS%.  These are needed for compatability and to handle some limitations with how GBM interacts with 7-Zip.
	- GBM now uses the XDG specification for game configurations in Linux. (Thanks basxto!)
		- For example, *appdatalocal* is now ${XDG_DATA_HOME:-~/.local/share} in a Linux configuration.
	- Custom Path Variables have changed.  For example, *Steam User Data* will now be %Steam User Data%.
		- They will appear this way in Windows and Linux.
		- GBM will no longer allow the creation of variables using reserved names, such as APPDATA.
	- Your configurations will be automatically updated to these new formats when upgrading to v1.1.5.
	- These changes will break game list compatability with other versions of GBM.  Archived lists are available at http://mikemaximus.github.io/gbm-web/archive.html for those that wish to stay on an older version.
- Added a new setting, "Show resolved save paths in Game Manager".
	- This new setting is enabled by default.
	- When enabled, GBM will display resolved save paths in the Game Manager.  This is how GBM displayed paths prior to v1.1.5.
	- When disabled, GBM will display save paths with their variables when applicable.
- Added a tooltip to applicable "Path" fields on the Game Manager.
	- This tooltip either displays either a resolved or unresolved path.  
	- The behaviour is toggled by the "Show resolved save paths" setting.
- Added "Import Backup Files" feature to the Game Manager.
	- This feature allows you to import one or more backup files for a specific game configuration.
	- This is useful if you lost your GBM database(s), but not the backup files.  It also can be used to easily move compatible saved game backups between Windows and Linux.
	- GBM cannot verify that the backups being imported are compatible with the current configuration.  This is up to the user!
	- This feature will be expanded and refined in future releases.
	
Linux:

- Fixed an issue that prevented Wine / Proton games from being detected in some cases.
- GBM can now use any Windows configuration to detect and backup games running in Wine / Proton.
	- An absolute Windows save path, such "%APPDATA%\Game\Saved Games" will be automatically converted to the proper path within the detected Wine prefix.
	- The converted path will be saved to the configuration once game has been detected at least once.
	- You cannot restore a backup using an absolute Windows path.  The game needs to be detected at least once so the correct save path can be determined.
	- This feature should be considered "Beta" and may not work in all scenarios.  Please report any issues you may encounter!

Website:

- Search features have been added to the Official Game Lists (Thanks basxto!)
	- Includes advanced features such as searching by tag and excludes.
	- Tags are now clickable for automatic searching of similar games.
	- Searches are linkable.
	- You can now link directly to a single configuration using the new link icon.
	- Configurations intelligently collapse or uncollapse based on search results.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html