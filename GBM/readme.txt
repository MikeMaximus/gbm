Game Backup Monitor v0.95 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

January 1, 2016

Disclaimer:

This is beta release software.  You may still encounter some bugs.

Important Upgrade Notice:

- v0.95 changes how DOSBox games are detected and will break DOSBox configurations from prior versions.  Sorry for the inconvenience.
- To repair a configuration, you'll need to delete the DOS executable part of the process field and make sure the case matches the DOSBox executable being used by the game.  
- For example, Capitalism Plus using a process of "dosbox:CAPPLUS" and DOSBox.exe would become just "DOSBox".  
- You can use the new "Custom" filter to easily find and update all your DOSBox games in the Game Manager.
- Due to this change GBM can only detect DOS games that are using their own copy of DOSBox in a unique location.  Most users will be unaffected, classic games sold online are already packaged in this format.
- Official configurations for DOSBox games are updated for this change.

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
- Made changes that will allow GBM to be easily translated to other languages.
- Many minor UI improvements and bug fixes.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html