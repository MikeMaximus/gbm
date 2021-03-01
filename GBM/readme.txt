Game Backup Monitor v1.2.6 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 1, 2021

Disclaimer:

v1.2.6 is still in development.  This file will be updated as changes are made.

New in 1.2.6

All Platforms:

- Fixed a crash that occurs on game detection when GBM cannot access the process details.
- Any linked process that requires administrator privileges will now ask for elevation when being launched from GBM.
	- This means GBM no longer needs to be running as administrator to launch these linked processes.
- If GBM quits unexpectedly during a backup operation, such as a power outage or OS shutdown, any incompleted backup operations will be resumed automatically the next time you run the app.
- Fixed the "Limit" field always displaying zero on the Game Manager.
- The "Play" button on the main form is now disabled for a few seconds after clicking.
	- This prevents attempting to launch a game multiple times by accident.
- The "Last Action" field on the main form now uses the regional setting for "Short Time" instead of being set to a specific format.
- Prevented the container splitter on the main window from sometimes gaining focus after a game is detected.
- Prevented a confusing error from being shown in the log when an icon can't be cached for expected reasons.
- Fixed the possibility of caching the incorrect icon in rare situations.


The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html