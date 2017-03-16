Game Backup Monitor v1.01 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 1, 2017

New in 1.01

- (All) Added features to automatically handle new backup files.
	- Automatically restore new backup files when they are detected.
	- Automatically mark new backup files as restored if the game isn't installed on the current PC.
	- These new features are available in the "Backup and Restore" section of the Settings.  They are optional and disabled by default.
- (All) Added new configuration option to delete saved game folder on restore
	- This option is used for games that change the file names of their saves, which results in a mix of old and new saved games if the old saves aren't deleted first.
	- This option has limitations and is not part of official configurations or import/export features.
- (All) The "Verify backup files with a checksum" option has been removed.  This feature is now baked into GBM and cannot be disabled.
- (Windows) Fixed rare issue related to DPI display scaling.

Read the "Settings" and "The Game Manager" sections of the manual (http://mikemaximus.github.io/gbm-web/manual.html) for more details on how the new features work and their limitations.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html