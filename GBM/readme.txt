Game Backup Monitor v1.2.8 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

August 3, 2021

New in 1.2.8

All Platforms:

- Differential backups are now supported and can be enabled in the Game Manager.
	- Read the online manual for details on how and when to use this backup method.
- Validation on "Game Path" will now trim off a file name if it's been included.
- Fixed importing multiple backup files for a single game.
	- GBM will now create manifest entries for each file when appropriate, instead of just the newest file.
- The "Save multiple backups" option used with a specific backup limit will now only clean up expired backups when a new backup is successfully completed.
- Fixed unhandled exception on the Launcher Manager when using the "..." (Browse) button with an empty "Executable" field.
	
Windows:

- GBM no longer tries to determine a relative save path when the "Game Path" and "Save Path" are on different drives.
- Updated SQLite to 3.36.0

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html