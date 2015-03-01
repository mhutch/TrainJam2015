#!/bin/bash

DIR=`pwd`

WINDIR="TrainJam2015.Windows/bin/Release"
MACDIR="TrainJam2015.Mac/bin/Release"

rm -r "${WINDIR}" "${MACDIR}"

#HACK xbuild doesn't build mac project
#mdtool doesn't run custom xbuild logic for windows projcet
xbuild /p:Configuration=Release /p:Platform=x86
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool build '-c:Release|x86' || exit 1

VERSION=`git describe --always --tag`

PACKAGE_WINDOWS="CloudTracks-Windows-${VERSION}"
PACKAGE_MAC="CloudTracks-Mac-${VERSION}"

rm -rf release
mkdir -p release/$PACKAGE_WINDOWS release/$PACKAGE_MAC
cp -r README.md Screenshot.png $WINDIR/* release/$PACKAGE_WINDOWS || exit 1
cp -r README.md Screenshot.png $MACDIR/*.app release/$PACKAGE_MAC || exit 1

cd release
7z u -tzip -r "${PACKAGE_WINDOWS}.zip" $PACKAGE_WINDOWS || exit 1
7z u -tzip -r "${PACKAGE_MAC}.zip" $PACKAGE_MAC || exit 1

rm -rf $PACKAGE_WINDOWS $PACKAGE_MAC
