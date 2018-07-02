Game Backup Monitor v1.1.3 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

July 1st, 2018

New in 1.1.3

All Platforms:

- Game tags are now sorted alphanumerically on the Game Manager and in XML exports.
- GBM now displays a unique backup overwrite warning for games that use a relative saved game path.
- The "Save Multiple Backups" setting now allows infinite backups when "Backup Limit" is set to 0.  This is now the default for new configurations.
- "Save Multiple Backups" and "Backup Limit" are now core fields, they are synced by default and included in the Import/Export.
- The Game Manager will now clean up it's own manifest when backup files are deleted outside of GBM.  As to not affect performance, this only occurs when the "Backup Data" field is accessed.
- When a single game is selected, the Game Manager now restores the currently selected backup, instead of always restoring the latest backup.
- GBM now displays the full path of the detected process when multiple configurations are triggered.
- Fixed an issue that could cause the certain controls to become enabled incorrectly on the Game Manager.
- Fixed an issue that caused GBM not to remove empty sub-folders unless the "Use Game ID for files and folders" setting was enabled when the folder was created.
- Fixed an issue that caused GBM not to rename backup files or sub-folders unless the "Use Game ID for files and folders" setting was enabled.
- GBM no longer displays a sync warning when the user deletes all game configurations from the Game Manager.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html