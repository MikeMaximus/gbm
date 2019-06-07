Game Backup Monitor v1.1.9 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

June 7, 2019

New in 1.1.9

All Platforms:

- Fixed a bug on the Game Manager that caused the "Save Entire Folder" checkbox to be unchecked anytime the "Save Path" field was changed.
- The "Add Game Wizard" now allows you to set an unlimited number of backups.
- The "Backup Limit" label has been updated to indicate that 0 means unlimited.
- The "Open Backup File" button on the Game Manager is now called "Open Backup".  It now gives a choice between opening the backup file or the folder containing the file.
- Set rules are now used for backup folder and file names, regardless of the operating system GBM is running on.
	- GBM now always filters out NTFS reserved characters and allows a maximum file name length of 255.
	- This will prevent various problems when using a backup drive with a non-standard file system in Linux or Windows.
	- These rules will be applied to new backup files or folders, existing backups not be modified.
- Made improvements to the Start-Up Wizard.
	- The default backup location now includes a "Game Backup Monitor" sub-folder.
	- The wizard will now automatically create the backup location if it doesn't exist.
	- You can no longer attempt to Import Backup Files during the Start-Up Wizard.

Linux:

- Fixed a bug that caused the "Available Disk Space" check to fail if the backup folder contained one or more spaces.
	
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html