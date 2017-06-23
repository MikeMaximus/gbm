Game Backup Monitor v1.02 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

July 1, 2017

New in 1.02

- (Windows) Upgraded SQLite to 3.19.3.
- (All) The SQLite version is displayed on the "About" screen.
- (All) Added the ability to detect command parameters.
	- Use parameter detection for better detection of games running in emulators or interpreters like DOSBox.
	- This is an advanced optional feature and is not available in the "Add Game Wizard", please read the manual (http://mikemaximus.github.io/gbm-web/manual.html) for more details.	
	- (Linux) Please note that Wine detection is still handled automatically by GBM and only requires a Windows process name.  But this feature does work with Wine if you need to detect parameters!
- (All) Added the ability to resize and maximize the main program window.
	- The log is now displayed by default and resizes with the window.
	- The "Show/Hide Log" button has been removed due to technical issues with this change.
	- The minimum window size will let you easily hide the log as in past versions.
- (All) The last browse location in various dialogs is now saved, such as when using the Import/Export feature.
- (All) Available disk space is checked before attempting a backup.  The log now displays available disk space and save folder size.
- (Linux) Using the keyboard to navigate the game list in the Game Manager now works correctly.
- (All) Tags can now be added to a new game configuration before saving on the Game Manager.
- (All) Fixed various issues when adding new game configurations while using filters on the Game Manager.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html