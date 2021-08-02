Game Backup Monitor v1.2.8 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

August 2, 2021

Disclaimer:

v1.2.8 is still in development, this file will be updated as changes are made.
This development version includes database changes and may not be compatible with the final release, it's not recommended to use this version outside of a testing environment.

New in 1.2.8

All Platforms:

- Differential backups are now supported and can be enabled in the Game Manager.
	- If you're unsure, read the online manual for details on how and when to use this new feature.
- Validation on "Game Path" will now trim off a file name if it's been included.
- Fixed importing multiple backup files for a single game.
	- GBM will now create manifest entries for each file when appropriate, instead of just the newest file.
- The "Save multiple backups" option with a specific backup limit will now clean up expired backups when a new backup is successfully completed, not before.
- Fixed unhandled exception on the Launcher Manager when using the "..." (Browse) button with an empty "Executable" field.
	
Windows:

- GBM no longer tries to determine a relative save path when the "Game Path" and "Save Path" are on different drives.
- Updated SQLite to 3.36.0

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html