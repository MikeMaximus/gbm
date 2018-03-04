Game Backup Monitor v1.1.0 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 3rd, 2018

New in 1.1.0

Disclaimer:

1.  This is pre-release software intended for testing. 
2.  Database files from this version (gbm.s3db) may not be compatible with the full release.  GBM makes automatic backups of your database files if you need to revert to a prior version.
3.  Do not make external links to this release, it will be available for a limited time.

All Platforms:

- Core Design Changes (Game Configuration)	
	- Game ID is now exposed to the user and can be changed.  This feature is mainly for developer and contributer usage.
	- Game ID is generated automatically by GBM or acquired from an import, the user doesn't need to set it unless they want to.
	- Game ID is now used to name game backup files and folders.
	- Game Name can now contain any character.
	- When a game is deleted via Game Manager (or sync), all backup manifest entries for that particular game are now deleted. The backup files themselves are not.
- Core Design Changes (Features)
	- The "Enable Sync" feature is now mandatory and the option been removed from Settings.
	- The "Clean Local Manifest" feature has been removed.  It is no longer required because manfiest entries can no longer be orphaned.  Existing orphaned entries will be removed during the v1.1.0 database upgrade.
- Import / Export Changes
	- When importing a game list, GBM now uses the Game ID to determine if a game is new or has an updated configuration.
	- Games with an updated configuration are identified by red text.
- Added Regular Expression support for game detection
	- This feature allows GBM to detect games based on a pattern instead of a single process name.
	- This allows GBM to better support games that run from multiple executables and games that use interpreters or emulators.
	- Use the new "Regular Expression" checkbox on the Game Manager and enter the pattern in the "Process" field.
	- GBM will validate patterns and offer to help troubleshoot (using regexr.com) when validation fails.
- Updated session CSV export to adhere to RFC 4180.  It now handles commas, quotes and line breaks correctly.

Windows Only:

- Updated 7-Zip to v18.01

Linux Only:

- GBM now uses notify-send (libnotify) if it's available to display notifications on Linux.
	- Mono style notifications will be displayed if notify-send is not available.
	- The GBM icon will be displayed on notifications if it's been installed to the correct location (via makefile or deb).

Known Issues:

- If one or more Game IDs are changed on one computer and these changes are synced to another PC sharing the same backup folder:
	- The local session data on that PC for the changed game(s) will be lost.
	- The local backup manifest data for the changed game(s) on that PC will be reset.  GBM will see any backups for the changed game(s) as new and will handle them accordingly.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html