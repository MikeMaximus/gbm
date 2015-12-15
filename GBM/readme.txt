Game Backup Monitor v0.95 Pre-Release Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

December 15, 2015

Disclaimer:

This is beta release software.  You may still encounter some bugs.

Important Upgrade Notice:

v0.95 changes how DOSBox games are detected, and WILL break all current DOSBox configurations.  This change was made to futureproof GBM, the old method of DOSBox detection was a hack and very prone to being broken by future changes to DOSBox or Windows.

There isn't a foolproof way to fix the configurations automatically on upgrade.  They need to be manually repaired.

To repair a configuration, you'll need to delete the DOS executable part of the process field and make sure the case matches the DOSBox executable being used by the game.  

For example, Capitalism Plus using a process of "dosbox:CAPPLUS" and DOSBox.exe would become just "DOSBox".  You can use the new "Custom" filter to easily find and update all your DOSBox games in the Game Manager.

All available official configurations for DOSBox games will be updated and ready for the v0.95 stable release.

And finally, due to this change GBM can only detect DOS games that are using their own copy of DOSBox in a unique location.  Most users will be unaffected, classic games sold online are already packaged in this format.

New in 0.95

- Fixed a regression that caused non-critical fields (Game Path, Company, Version, Icon, Enabled, Monitor Only) to be wiped or reset on sync.
- The "Check for new backups" feature has been renamed and redesigned. Instead of an annoying pop-up, it now shows a simple notification in the main menu or tray menu.
- Added the ability to trigger backups only after a certain session time has elapsed (Global Setting), this setting will be disabled by default.
- You can now cancel out of the "Choose Game" window when GBM detects multiple games may be running.
- Added the ability to filter by game information such as name, process and company.
- Many UI improvements and fixes with filtering.  The "Tag" filter is now called "Custom" filter, due to the new options available.
- Added a new, more intuitive way of including and excluding items in a backup.  See the Game Manager section of the GBM manual for details.
- Updated the "Add Game Wizard" to use the new include / exclude and tagging features.
- Removed the special handling of DOSBox games for future proofing, DOSBox games will now be handled like all other games.
- Moved the import and export game list features from the Tools menu into the Game Manager.
- Made some visual improvements to the main app window.
- Many minor UI improvements and bug fixes.

New in 0.94

- Added the ability to organize your games with Tags.  Users can create and customize their own list of Tags.
- Added the ability to filter by Tag in the Game Manager.
- Added the ability to filter by Tag when doing an XML export.
- Added the ability to disable Time Tracking.  Enabling or disabling this setting has no effect on data already recorded.
- Added the ability to restart GBM as Administrator from the status bar.
- Redesigned notifications, GBM is now silent unless a problem occurs.
- Enhanced the sync logic and performance.
- The backup "Timestamp" setting is no longer included in XML imports or exports.
- Redesigned the XML import and export core.  Sorry, this invalidates all prior XML exports files, but it will ensure future backwards compatability.
- Various bug fixes.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html