Game Backup Monitor v1.2.6 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 6, 2021

New in 1.2.6

All Platforms:

- Any linked process that requires administrator privileges will now ask for elevation when being launched from GBM.
	- This means GBM no longer needs to be running as administrator to launch these linked processes.
- If GBM quits unexpectedly during a backup operation, such as a power outage or OS shutdown, any incompleted backup operations will be resumed automatically the next time you run the app.
- The "Play" button on the main form is now disabled for a few seconds after a game launch is triggered.
	- This prevents attempting to launch a game multiple times by accident.
- Fixed a crash that occurs on game detection when GBM cannot access the process details.
- Fixed the possibility of caching the incorrect icon in some situations.
- Prevented a confusing error from being shown in the log when an icon can't be cached for expected reasons.
- Fixed an issue that caused GBM to unnecessarily require the "Game Path" when detecting a game with a "Monitor Only" configuration.
- Fixed an issue with the automatic restore feature that could cause restore operations to trigger multiple times when using linked configurations.
- Fixed an issue that caused GBM to repeatedly detect the same process if an unexpected error occured while waiting for that process to end.
	- The error message will now be displayed in the log and monitoring will be automatically stopped.
- The "Last Action" field on the main form now uses the regional setting for "Short Time" instead of being set to a specific format.
- Fixed the "Limit" field always displaying zero on the Game Manager.
- Fixed various small issues with the user interface.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html