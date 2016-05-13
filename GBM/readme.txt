Game Backup Monitor v0.96.5977 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

May 13, 2016

Disclaimer:

This is beta release software.  You may still encounter some bugs.

New in 0.96.5977

- (Windows) Updated GBM's version of 7-Zip to 16.00 (2016-05-10)

New in 0.96

For more information regarding Linux support read the FAQ at http://mikemaximus.github.io/gbm-web/linux.html

- (All) Modified backup and restore logic for better usability when doing batch operations and to fix Mono related issues.
- (All) Overhauled the Search screen for better usability and to fix Mono related issues.
- (All) Overhauled the Import screen for better usability.  Added game tags, simple filtering, sorting and general fixes.
- (Linux) Fixed some bad code causing the "Official Import" connection to time out when making multiple connections in the same session.
- (Linux) Fixed cross-platform issues with opening backup files and restore locations from the Game Manager.
- (All) Backup files opened from the Game Manager will now open directly in the app they are associated with, like 7-Zip GUI or File Roller.
- (Linux) Fixed cross-platform issues with automatic file/folder searching.  It's now enabled on Linux.
- (All) GBM now restores saved games to the currently configured saved game path by default.  The stored manfiest location is only used when there's no configuration for the game.
- (Linux) Added Linux support via Mono!
- (All) Replaced System.Data.SQLite with the Mono.Data.Sqlite for cross-platform support.
- (Windows) Updated GBM's version of 7-Zip to 15.14 (2015-12-31)
- (All) Added the ability to set the 7-Zip compression level on the Settings screen.
- (Windows) GBM now deletes all user files to the Windows recycle bin by default.
- (All) Added the ability to clear and save the session log from the Tools menu.
- (All) GBM now auto-saves and clears the session log to %localappdata%\gbm if it reaches it's limit (2 MB).
- (All) The file size will be displayed in the session log after each backup.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html

Important Upgrade Information:

If you've used a pre-release of v0.96, you may experience the following error message after installing this version.

Column 'CompressionLevel' does not belong to table Table.

If you get this error follow these steps:

1.  Start GBM and click "Yes" to continue past the error. 
2.  Press CTRL and ~ (Tilde) to open the Developer Console.
3.  Enter the command exactly as follows (or copy & paste it) then click OK.

SQL Local PRAGMA user_version=95

4.  You'll receive confirmation that the command was executed.  Click OK and immediately close GBM via the File menu.
5.  Restart GBM and the error will no longer appear!