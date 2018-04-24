#!/bin/bash

current_dir=$(cd $(dirname $0);pwd)

if [ ! -e /Applications/Unity/Unity.app ]; then
	echo "You need to install Unity.app on your Mac"
	exit 1
fi

# Build AndroidPushPlugin
cd AndroidPushPlugin
android update project --path . --target android-19
./gradlew clean copyJar
cd ../

# Place AndroidPushPlugin to UnityPushPlugin
cp AndroidPushPlugin/app/build/outputs/jar/release/classes.jar \
UnityPushPlugin/Assets/Plugins/Android/androidpushplugin.jar

sed -i '' -e 's/com.kii.unity.sample.push/com.example.your.application.package.name/g' ./UnityPushPlugin/Assets/Plugins/Android/AndroidManifest.xml

rm -rf bin
mkdir bin

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode -quit \
    -buildTarget android \
    -projectPath "$current_dir"/UnityPushPlugin \
    -exportPackage Assets/Plugins \
                   Assets/Plugins/Android \
                   Assets/Plugins/iOS \
                   ../bin/KiiPushPlugin.unitypackage

sed -i '' -e 's/com.example.your.application.package.name/com.kii.unity.sample.push/g' ./UnityPushPlugin/Assets/Plugins/Android/AndroidManifest.xml

