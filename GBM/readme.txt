Game Backup Monitor v1.2.5 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

December 13, 2020

New in 1.2.5

Disclaimer:

v1.2.5 is still in development.  This file will be updated as changes are made.

All Platforms:

- Overhaul of the main window interface (more details to come later)
- AbsolutePath is now a calculated field and no longer stored.
	- This is a bigger change than it seems, but I feel it was worth it.  It never should have been stored and this will clean up some issues and prepare for future updates.
	- This will break future official lists for prior versions of GBM (v1.1.5 - v1.2.4).  The last official list(s) compatible with these prior versions will be archived so they can still be accessed.
- Application settings are now handled more efficiently.
- Added missing code to properly update or delete existing launch data when certain configuration changes are made.
- Fixed issues with changing a configuration identifier when that configuration had existing backup files.

Windows:

- The icons from game executables are now cached after a session.
	- The main window and game manager will display the cached icon when viewing the game details, unless a custom icon has been set.
	- Cached icons are stored using the PNG format in the GBM temporary folder (can be customized in Settings).
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html