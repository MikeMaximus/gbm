Game Backup Monitor v0.94 Readme
http://mikemaximus.github.io/gbm-web/

November 17, 2015

Disclaimer:

This is beta release software.  You may still encounter some bugs.

Introduction:

This application is designed to run in the system tray and monitor for games you play.  When you exit a game your save files will be automatically compressed (7z compression), saved to a folder you specify.  This may be a cloud folder or external backup drive that other computers will be accessing.  GBM can also take care of restoring the save data to other computers that have access to the main backup folder.

GBM has been designed with mostly classic gaming in mind, but it can be used for any application!

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

New in 0.93

- GBM now handles date/time fields in an efficient manner, this will solve all issues with regional date formats.
- GBM will now gracefully exit with an error message if it detects a database version is newer than the program version itself.
- "Access Denied" errors should no longer occur when dragging shortcuts into the Add Game Wizard.
- Redesigned how the backup includes and excludes files.  The changes allow for more complex backup configurations.
- Added an "Open Restore Path" button to the Game Manager, this will automatically open the current backup's restore location in Windows Explorer.
- Game names are now cropped in many places to prevent errors when messages and/or notifications exceed control limits.

New in 0.92 

- GBM now properly handles detecting games in DOSBox that share the same DOS executable name.
- "Start with Windows" toggle is now available in settings. Defaults to off and applies to Current User only.
- Added "Backups Only" filter option to the Game Manager, this lets you view only games that have backups.
- "Backups Only" and "Pending Restore" filters will now list existing backup data for games have been deleted from the game list.
- The game list sync is now more verbose in the log.
- GBM now triggers a master to local sync when the master data is changed by another application, such as your cloud client downloading an updated version of the master data after GBM is already running.
- When importing game configurations, you can now choose exactly which game configurations to import.  This applies to the official list or importing an xml file.
- Added a Sync toggle to Settings. Disabling sync will speed up the application for users that use GBM on a single computer.
- GBM should now properly display a wait cursor in instances that the UI is unavailable.
- Added a "Compact Databases" tool, this is used to rebuild GBM's databases to use an optimal amount of disk space. It shouldn't be needed often.
- Game detection has been changed. It will take slightly longer for GBM detect a game but this should fix a few long outstanding issues.
- Backup & Restore has been optimized to handle everything more efficiently.
- GBM will now use the 2015 build of 7-Zip for compression, as well use the 64-bit version of 7z when using the 64-bit version of GBM. 64-bit users will see a huge speed improvement when backing up games with large complex saves, such as Divinity: Original Sin.
- GBM now allows you to cancel monitoring via the File menu, system tray menu and monitor status button. You can also now exit the application while a game is being monitored.
- The GBM system tray icon tooltip will now display it's current task.
- Replaced the icons in GBM with some better looking ones, the system tray icon will now visually represent GBM's current action.
- You can now cancel a backup or restore operation in progress. Intended for emergency situations only, cancelling will not undo any actions completed before the operation terminated.
- GBM now uses SHA-256 to verify your backup files before restoring them. This is optional, but will be enabled by default.
- Overhauled the backend, GBM will use more space to store records, but the improved database design will prevent future bugs. 
- GBM will now back up your configuration data automatically on a version upgrade.
- Another ton of minor fixes and tweaks too various to list.

New in 0.84

- Tons of bug fixes and tweaks.  GBM is stable enough to move to Beta!
- Removed Backup Manager and Restore Manager.  All backup & restore functionality is now integrated into the Game Manager.
- You can now backup and restore mutliple games at the same time.
- You can now edit multiple games at the same time, only on toggle fields for now.
- Added a "Monitor Only" option.  GBM can only monitor the time you've played a game and not trigger a backup when it's closed.
- Deleting a backup will now delete the sub-folder for the game, if it's not empty a confirmation will be displayed first.
- Since it can't reliably find them, GBM will no longer attempt to automatically search for dosbox games or games that share a process name.
- You can now multi-select on the Game Manager screen to delete multiple games.
- When changing your backup folder, if the newly selected backup folder already contains GBM data you now have a choice on how you want to handle the initial game list sync.

New in 0.81

- Fixed a critical bug with the Game Manager which allowed you to add duplicate entries.  This resulted in a crash and the inability start the app again.  0.81 fixes this bug and will repair any database broken by this issue so the app will function again. (Thanks Nirth from GOG.com)
- Fixed an annoying bug where extra game data was being lost when syncing hours spent between computers.
- Fixed a potential issue with changing the backup folder.
- The Add Game Wizard will now properly handle entries with spaces while working with the Exclude and File Type helper.

New in 0.8

- Since it's all about the games anyway, re-branded ABM to GBM - Game Backup Manager.
- Removed confusing utilities and screens and replaced them with a user friendly Game Manager.
- Rebuilt the Custom Path Variable screen with the new Game Manager look.
- 64-bit build available, this will allow GBM to fully detect 64 bit and 32 bit applications. (32-bit will be default download)
- Switched back-end from XML to SQLite 3.  Old configurations will be imported and upgraded.
- Automatic syncing of game list and applicable extra data (such as Hours played) between computers sharing the same backup folder. See "Notes on Automatic Syncing" in this readme for details.
- All import features redesigned to work more consistently.  Added export feature.
- In the rare instance the user is asked to manually find a path, GBM will now search for the location automatically.

New in 0.7.1.2:

- Better Monitoring: You no longer need to run ABM as Administrator to monitor certain applications in 99% of situations. See "Current Limitations" for details.
- Time Tracking:  ABM now tracks the amount of time spent in each application, similar to Steam.
- Search: You can now search for a specific application using the new search feature on all applicable forms.
- Application Data:  You can now add or override application details by using the new "Edit Application Cache" feature.

New in 0.7.0.7:

- Fixed some monitor list saving and validation issues.
- Fixed the manual backup feature, required application data wasn't being cached. 
- Fixed possible issue with official import.
- Minor Tweaks.

New in 0.7.0.5:

- Start-up Wizard: A simple guide to setup the basics when first starting the program.
- Add Application Wizard: A more user friendly way to add applications to monitor and backup.  The classic monitor list is still available for more advanced configurations and management.
- Custom Path Variables: An advanced feature to allow the configuration for more applications to be shared between computers.
- Manual Backups:  Execute a backup for any monitored application on demand (some applications will be need to be detected by ABM at least once to use this feature)
- Various tweaks to make things look and run a little better.
- Many bug fixes