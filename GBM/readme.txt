Game Backup Monitor v1.0.6 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

January 15, 2018

New in 1.0.6

All Platforms:

- Session Viewer Updates
	- The viewer now defaults to show seven days from the last recorded session, instead of all sessions.
	- Fixed issues with date sorting, it will now sort correctly regardless of format.
	- The recording of short sessions can now be ignored using a setting.
- Changed setting "Backup only when session time exceeds XX minutes" to "Ignore sessions shorter than XX minutes".
	- This setting is now used to ignore recording sessions times (when enabled) in addition to ignoring a backup.
	- This setting has been moved to the "General" section.
	- This change requires no update from the user, it will function exactly as it did before.
	
Windows Only:

- A warning is now displayed if the "Start with Windows" setting is enabled while GBM is running as administrator.
	- GBM currently will not "Start with Windows" as administrator when this setting is enabled.
	- Please see http://mikemaximus.github.io/gbm-web/gbm_task_scheduler.html for a work-around.

Linux Only:

- Enhanced makefile and added start script for Linux installations (basxto)
- GBM is now available on the archlinux user repository. https://aur.archlinux.org/packages/gbm/ (basxto)
- GBM is now available as a deb package for installation. (basxto)

	
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html