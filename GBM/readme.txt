Game Backup Monitor v1.1.0 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 12th, 2018

New in 1.1.0

Disclaimer:

Version 1.1.0 makes fundamental changes to how GBM works with game configurations and backup data, in addition to many other updates.  Read the changes below carefully before upgrading.

I've done my best to make sure the upgrade process allows everyone to continue using their existing data and configurations.

However, users who are sharing a backup folder between multiple PCs will experience data loss at some point due to the changes in this version.  Please read "Known Issue #1" in this file for details.

All Platforms:

- Core Design Changes (Game Configuration)	
	- Game ID is now exposed to the user and can be changed.  This feature is mainly for developer and contributer usage.
	- Game ID is generated automatically by GBM or acquired from an import, the user doesn't need to set it unless they want to.
	- Game ID (instead of Game Name) can now be used to name backup files and folders.  
		- GBM will continue to use the name by default for ease of use.
		- This behavior can be toggled in the "Backup and Restore" section of Settings.
		- Using the game Name for backup files has a minor risk associated with it. See "Known Issue #2 and #5" for more details.
	- Game Name can now contain any character.
	- When a game is deleted via Game Manager (or sync), all backup manifest entries for that particular game are now deleted. The backup files themselves are not.
	- The Game Manager now syncs changes to the remote database immediately, instead of only when closed.

- Feature Addtions & Changes
	- Added Regular Expression support for game detection
		- This feature allows GBM to detect games based on a pattern instead of a single process name.
		- This allows GBM to better support games that run from multiple executables and games that use interpreters or emulators.
		- Use the new "Regular Expression" checkbox on the Game Manager and enter the pattern in the "Process" field.
		- GBM will validate patterns and offer to help troubleshoot (using regexr.com) when validation fails.
	- Added the ability to start another process (or multiple processes) whenever a game is detected. (Thanks for the suggestion Naliel Supremo)
		- This is useful to automatically start utilities, such as custom control schemes or overlays when a specific game is detected.
		- The "Process Manager" allows you to manage any programs you'd like to launch.
		- The "Processes..." button on the Game Manager allows you to assign processes to any selected game.
		- A process can be set to end when the game is closed.
		- Processes and related settings are specific to the local machine only.  They are not synced to the backup folder and are not part of the import/export.
	- Added "Backup GBM data files on launch" to the settings.  A long overdue feature, this will backup both the remote and local databases (as gbm.s3db.launch.bak) each time GBM starts.  
		- This new setting is enabled by default.
		- Only one backup is kept, the prior one will be overwritten.
	- Added the ability to display messages that can be supressed.  These messages can be reset via the Settings screen.
	- The "Enable Sync" feature is now mandatory and the option been removed from Settings.
	- The "Clean Local Manifest" feature has been removed.  It is not required because manfiest entries are no longer orphaned by design.  Existing orphaned entries will be removed during the v1.1.0 database upgrade.
	- Added "Sync Game IDs" feature.  This allows the user to update their game configuration identifiers to match the official list or an export file.
		- The sync is based on similarly named game configurations, therefore it's not 100% effective.  Some data may be missed and require manual changes.
		- This is an optional upgrade tool for users with existing data from older versions.  It allows the import feature to properly recognize and update game configurations.
		- If you share a backup folder with multiple PCs, this feature will cause some data loss when the new IDs are synced to the other PCs.  See "Known Issue #1"

- Import / Export Changes
	- GBM now uses the Game ID to determine if a game is new or has an updated configuration.
	- GBM will offer to "Sync Game IDs" when importing from the official list after upgrading to v1.1.0.
		- This allows the import to recognize and update your configurations from older versions.
		- This offer only appears once and only appears for users that have upgraded from an older version.	
	- Added icons to the import list to indicate a "New" or "Updated" game.

- Updated session CSV export to adhere to RFC 4180.  It now handles commas, quotes and line breaks correctly.

Windows Only:

- Updated 7-Zip to v18.01

Linux Only:

- GBM now uses notify-send (libnotify) if it's available to display notifications on Linux.
	- Mono style notifications will be displayed if notify-send is not available.
	- The GBM icon will be displayed on notifications if it's been installed to the correct location (via makefile or deb).

Known Issues:

1.  If one or more Game IDs are changed (manually or via Game ID Sync) on one computer and these changes are synced to another PC sharing the same backup folder, the following data will be lost.
	- The local session data on that PC for the changed game(s) will be lost.
	- The local backup manifest data for the changed game(s) on that PC will be lost.  GBM will see any backups for the changed game(s) as new and will handle them accordingly.
	- Any processes assigned to the changed games(s) on that PC will be lost.
	Once your PCs are back in sync, this will no longer be an issue unless you are constantly changing your Game IDs, which is not recommended.
2.  Backup files are not being renamed or removed when a new backup is created.
	- This happens on the first backup after toggling between using the Name or ID for your file names.  It's best to choose one setting and stick with it.
3.  Configurations on the official game list are no longer fully compatible with older GBM versions.  
	- Technically they will work.  But any game imported with a special character in it's name, such as a colon, will not create backup files correctly.
	- These characters can be manually removed from the game name after importing, then the configurations will function properly.
4.  The error "The requested operation requires elevation" occurs when GBM tries to launch a process associated with a game.
	- This means the process you're trying to launch with GBM requires administrator privilege.
	- Click the blue "user" icon on the bottom left of the GBM window to quickly switch to administrator mode.
5.  Game configurations using the same name, and configurations that end up with the same name when special characters are stripped will overwrite each other's backup files.
	- For most users this should be a non-issue.  Toggle "Use Game ID for folder and file names" to on in the Settings screen if this is a problem for you.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html