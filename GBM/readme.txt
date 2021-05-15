Game Backup Monitor v1.2.8 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

May 15, 2021

Disclaimer:

v1.2.8 is still in development, this file will be updated as changes are made.
This development version includes database changes and may not be compatible with the final release, it's not recommended to use this version outside of a testing environment.

New in 1.2.8

All Platforms:

- Differential backups are now supported and can be enabled in the Game Manager.
- Validation on Path fields (Save Path, Game Path, etc) will now trim off a file name if it's been included.
- Fixed unhandled exception on the Launcher Manager when using the "..." (Browse) button with an empty "Executable" field.
- Fixed importing multiple backup files for a single game.
	- GBM will now create manifest entries for each file when appropriate, instead of just the newest file.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html