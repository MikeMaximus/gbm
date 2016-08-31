Game Backup Monitor v0.98 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

September 1, 2016

Disclaimer:

This is beta release software.  You may still encounter some bugs.

New in 0.98

- (All) Redesigned GBM's ability to manage multiple backups per game.
	- The "Timestamp each backup" option has been renamed "Save multiple backups".
	- You can limit the amount of backups you keep for each game by setting a limit (2 to 100).
	- GBM will keep your backup folder clean by automatically deleting old backups as limits are reached or modified.
	- The Game Manager now allows you to browse and manage ALL backups for each game, not just the latest backup.
- (All) Using "Monitor Only" no longer puts anys limitations on the game configuration or available features.
- (Windows Installer) The installer will now properly go into upgrade mode if a prior version of GBM is installed.  Note: 32-bit and 64-bit qualify as different versions.
- (Windows Installer) The installer will now create an uninstall entry in Add/Remove Programs (Apps & Features).
- (Linux) 64-bit games running in Wine will now be properly detected.
- (Linux) Add Game Wizard will no longer remove all extensions when selecting an executable.
- (Linux) GBM now prefers to use an absolute path when creating Linux game configurations.

Important Upgrade Information:

- The v0.98 Game Manager will not detect old backup files made using the "Timestamp each backup" setting in prior versions.
- Configurations using the "Timestamp each backup" option will have "Save multiple backups" automatically enabled with a backup limit of 5.
- If "Timestamp each backup" is currently set to sync, then "Save multiple backups" will also be set to sync after upgrading.

For more information regarding Linux support read the FAQ at http://mikemaximus.github.io/gbm-web/linux.html

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html