OSNAME=${shell uname -s}
ifeq (${OSNAME}, Darwin)
UNITY_COMMAND="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
else
UNITY_COMMAND="C:/Program Files (x86)/Unity/Editor/Unity.exe"
endif

PROJECT_PATH=`pwd`
TARGET_DIR="target"
TARGET_FILE="${TARGET_DIR}/KiiServerSideAuth.unitypackage"

all: clean build

prebuild: build-sdk build-jar

build-sdk:
	(cd ../; \
	make;)
	cp ../KiiCloudStorageSDK/bin/Release/KiiCloudStorageSDK.dll Assets

build-jar:
	(cd Android-Source; \
	sh gradlew build;);
	mkdir -p Assets/Plugins/Android
	cp Android-Source/app/build/outputs/aar/app-release.aar Assets/Plugins/Android/WebViewDialog.aar

build:
	mkdir -p ${TARGET_DIR}
	${UNITY_COMMAND} -projectPath ${PROJECT_PATH} -exportPackage Assets ${TARGET_FILE} -batchmode -quit

release-build: clean prebuild build

clean:
	rm -rf ${TARGET_DIR}
