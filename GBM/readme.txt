Game Backup Monitor v1.1.3 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

June 25th, 2018

New in 1.1.3

All Platforms:

- Game tags are now sorted alphanumerically on the Game Manager and in XML exports.
- GBM now displays a unique backup overwrite warning for games that use a relative saved game path.
- The "Save Multiple Backups" setting now allows infinite backups when "Backup Limit" is set to 0.
- The Game Manager will now clean up it's own manifest when backup files are deleted outside of GBM.  As to not affect performance, this only occurs when the "Backup Data" field is accessed.
- When a single game is selected, the Game Manager now restores the currently selected backup, instead of always restoring the latest backup.
- Fixed an issue with the "Save Multiple Backups" setting always syncing even when disabled in "Optional Sync Fields"

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html