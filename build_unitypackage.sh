#!/bin/bash

release_dir="$1"
unity_path=/Applications/Unity/Unity.app

if [ ! -e $unity_path ]; then
    echo "You need to install Unity.app on your Mac"
    exit 1
fi

cd "$release_dir"
rm -rf _unitypackage
mkdir _unitypackage
mkdir _unitypackage/Assets
mkdir _unitypackage/Assets/Kii
mkdir _unitypackage/Assets/Kii/Scripts
mkdir _unitypackage/Assets/Kii/Libs
mkdir _unitypackage/Assets/Plugins
#mkdir _unitypackage/Assets/Plugins/Android
#mkdir _unitypackage/Assets/Plugins/iOS

# copy the SDK
cp Kii*.dll _unitypackage/Assets/Kii/Libs
cp JsonOrg.dll _unitypackage/Assets/Kii/Libs
# copy the push plugin
cp ../../../UnityPlugins/UnityPushPlugin/Assets/Plugins/*.cs _unitypackage/Assets/Kii/Scripts
cp -rf ../../../UnityPlugins/UnityPushPlugin/Assets/Plugins/Android _unitypackage/Assets/Plugins
cp -rf ../../../UnityPlugins/UnityPushPlugin/Assets/Plugins/iOS _unitypackage/Assets/Plugins
sed -i '' -e 's/com.kii.unity.sample.push/com.example.your.application.package.name/g' _unitypackage/Assets/Plugins/Android/AndroidManifest.xml

# copy the server side auth plugin
cp ../../../KiiServerSideAuth/Assets/Plugins/*.cs _unitypackage/Assets/Kii/Scripts
cp -rf ../../../KiiServerSideAuth/Assets/Plugins/Android _unitypackage/Assets/Plugins
cp -rf ../../../KiiServerSideAuth/Assets/Plugins/iOS _unitypackage/Assets/Plugins

$unity_path/Contents/MacOS/Unity \
    -batchmode -quit \
    -buildTarget android \
    -projectPath "$release_dir"/_unitypackage \
    -exportPackage Assets/Kii \
                   Assets/Kii/Scripts \
                   Assets/Kii/Libs \
                   Assets/Plugins \
                   Assets/Plugins/Android \
                   Assets/Plugins/iOS \
                   ../KiiCloudUnitySDK.unitypackage

