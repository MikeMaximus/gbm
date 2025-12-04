#! /bin/bash

printf "Game Backup Monitor - Portable Mode Switcher\n"
printf "Current Folder: $PWD\n\n"

if [ -f "portable.ini" ]; then
    rm -f portable.ini >/dev/null 2>&1

    if [ $? -eq 0 ]; then
        printf "Game Backup Monitor is now in normal mode.\n"
        printf "You may run this file again to toggle the mode.\n\n"
    else
        printf "An error occured, you likely don't have the required permissions.\n"
        printf "Try manually deleting the portable.ini file from $PWD to disable portable mode.\n\n"
    fi
else 
    touch portable.ini >/dev/null 2>&1

    if [ $? -eq 0 ]; then
        printf "Game Backup Monitor is now in portable mode.\n"
        printf "You may run this file again to toggle the mode.\n\n"
    else
        printf "An error occured, you likely don't have the required permissions.\n"
        printf "$PWD is not a suitable location for portable mode.\n\n"
    fi
fi

read -s -n 1 -p "Press any key to continue..."$'\n'