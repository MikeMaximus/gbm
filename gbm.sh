#!/bin/sh -e
# prepare 
if [  -z "${XDG_DATA_HOME}" ]; then
    XDG_DATA_HOME="${HOME}/.local/share"
fi
mkdir -p "${XDG_DATA_HOME}/gbm"
pidfile="${XDG_DATA_HOME}/gbm/pid"

# check whether pid file exists and process is running
if ( ! [ -e "${pidfile}" ] || ! pgrep -F "${pidfile}" ); then

    # check for all dependencies
    for prog in mono readlink df 7za;do
        [ -n "`whereis -b ${prog} | cut -sd' ' -f2`" ] || (echo "Please install ${prog}" && exit 1);
    done
    for lib in libsqlite3;do
        [ -n "`ldconfig -p | grep ${lib}`" ] || (echo "Please install ${lib}" && exit 1);
    done

    # directory this script is located in
    dir="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
    echo "Located in ${dir}";
    gbmpath='./';

    # locate GBM.exe
    if [ "${dir}" = '/usr/bin' ] && [ -s '/usr/share/gbm/GBM.exe' ]; then
        gbmpath='/usr/share/gbm/';
    elif [ "${dir}" = '/usr/local/bin' ] && [ -s '/usr/local/share/gbm/GBM.exe' ]; then
        gbmpath='/usr/local/share/gbm/';
    elif [ ! -s './GBM.exe' ]; then
        echo 'GBM.exe not found';
        exit 2;
    fi

    # pass our arguments to GBM and run it in background
    mono --desktop "${gbmpath}GBM.exe" "$@" &
    # store pid and wait for process to end
    echo $! > "${pidfile}"
    wait $!
    rm "${pidfile}"
    exit $?;
else
    echo "GBM is already running"
    exit 1;
fi
