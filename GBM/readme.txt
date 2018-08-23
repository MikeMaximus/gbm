Game Backup Monitor v1.1.4 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

August 25th, 2018

New in 1.1.4

All Platforms:

- Changed method of handling required OS environment paths and added a new path check on startup.
- Incorrect conflict warnings will no longer be shown in some situations when running a backup configuration with "Save Multiple Backups" enabled.
- Fixed the precision of "Total Hours" on the Session Viewer, it now always rounds to 2 decimal places.

Windows:

- GBM will no longer keep an open file handle on detected processes and custom icons.

Linux:

- Sorting by "Hours" in the Session Viewer will no longer crash GBM on certain systems.
- GBM no longer attempts to get icons from a Linux binary.
- Removed compression from GBM icon to prevent issues with recent versions of imagemagick.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html