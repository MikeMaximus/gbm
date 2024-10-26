Game Backup Monitor v1.4.0 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

October 26, 2024

New in 1.4.0

Windows:

- Game Backup Monitor now has support for "Dark Mode" in Windows 10 & 11.
    - Dark Mode is automatically applied based on your operating system theme, it cannot be toggled manually.
- Installers now detect and close GBM automatically if it's running.
- Installers will no longer look blurry with High DPI scaling.
- Installers now automatically set the optimal High DPI compatability mode for GBM on Windows 10 or later.
    - This prevents GBM from looking blurry with High DPI scaling.
    - This can be modified in the compatability settings for GBM.exe if another scaling mode is preferred.
- Updated YamlDotNet to 16.1.3.
- Updated SQLite to 3.47.0.
- Updated 7-Zip to 24.08.

General:

- Changed list icons on the "Import Game Configurations" form to be more appropriate for both light and dark modes.

Known Issues:

- Dark mode is not applied to the date controls on the "Session Viewer" form.
    
Notes:

- Dark Mode was made possible by the Dark-Mode-Forms (https://github.com/BlueMystical/Dark-Mode-Forms) project.
- GBM uses a custom version of this project available at https://github.com/MikeMaximus/DarkModeForms.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html