#!/bin/bash

current_dir=$(cd $(dirname $0);pwd)

if [ ! -e /Applications/Unity/Unity.app ]; then
	echo "You need to install Unity.app on your Mac"
	exit 1
fi

# Build jar file
cd Android-Source
sh gradlew build
cd ../

mkdir -p Assets/Plugins/Android
cp Android-Source/app/build/outputs/aar/app-release.aar Assets/Plugins/Android/WebViewDialog.aar

rm -rf bin
mkdir bin

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode -quit \
    -buildTarget android \
    -projectPath "$current_dir" \
    -exportPackage Assets/Plugins \
                   Assets/Plugins/Android \
                   Assets/Plugins/iOS \
                   ./bin/KiiServerSideAuth.unitypackage
