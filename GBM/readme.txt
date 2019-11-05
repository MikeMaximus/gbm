Game Backup Monitor v1.2.0 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 5, 2019

New in 1.2.0

All Platforms:

- Added the ability to easily backup or restore the entire game list.
	- Use the "Run Full Backup" or "Run Full Restore" options in the File menu or Tray menu to easily backup or restore your entire game list.
	- Any confirmations or checks requiring user input are automatically handled when using these tools.  See the "Full Backup and Restore" section of the manual for details.
- Added the ability to "link" game configurations.
	- This allows any linked configuration(s) to automatically run a backup or restore each time their parent is triggered.  This can continue in a chain.
	- This feature allows for the proper configuration of rare games that stored their saved games in multiple unique locations.  It may also have other creative uses.
	- Please read "Link -> Configuration" in the "Game Manager" section of the manual for details on exactly how this feature works.
- Metadata is now added to all backup files.
	- This is a small XML file that contains important information about the archive and the configuration used to make the backup.
	- This allows GBM and possibly any other software to easily identify and use GBM backup files.
	- This file is not extracted when restoring a backup, it will not be cluttering up your saved game folders.
- All backup operations are now performed in a temporary folder before the resulting file is moved to the backup folder.
	- This change was required to fix periodic issues with cloud software (Dropbox) locking backup files while they were being created.
	- The temporary folder can be manually set in "Settings -> Backup and Restore", it defaults to %LOCALAPPDATA%\gbm (~/.local/share/gbm).
- Improved the ability to import backup files using metadata.
	- There is a now a global tool to import backup files or entire folders available in the "Tools" menu.
	- Only backup files with GBM metadata can be imported using this new tool.
	- You may still force the import of backup files without metadata (or incorrect metadata) using the "Import Backup Files" tool in the Game Manager.
- GBM now properly checks for available disk space when doing batch operations.
- GBM no longer attempts to search for a game when the process name is a regular expression, it isn't supported.
- Fixed various long-standing problems with the "Cancel" button.
	- Using "Cancel" during a backup or restore now properly cancels out of batch operations.
	- Using the "Cancel" button no longer prevents future operations from executing properly.
- The Help menu is no longer disabled unnecessarily when the application is busy.
- Various other small improvements have been made to the GUI and messages to improve usability.

New in 1.1.9HF2 (Hotfix #2 - July 31, 2019)

All Platforms:

- Multiple variables in the same path are now supported. 
	- This feature is meant to allow the mix of relative path variables with a single absolute path variable when the situation requires it.
	- Multiple Custom Path Variables in the same path are now supported.
	- Mixing Environment Variables with relative Custom Path Variables is now supported.
- Allow saving a game configuration with an empty Process field. 
	- This feature is meant to make life easier for users who want to use GBM for manual backups.
	- New users will receive a one-time warning when saving a configuration with no Process, current users may or may not receive this new warning.
	- Configurations with an empty Process field will be automatically excluded from monitoring, regardless of the "Monitor this Game" setting.

New in 1.1.9HF1 (Hotfix #1 - July 18, 2019)

All Platforms:

- Custom Path Variables and certain Environment Variables are now supported in the "Game Path" field.
- Fixed multiple crash issues caused by a custom icon being an invalid image format.
- GBM now uses UTC date/time for the build identifier.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html