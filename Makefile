CONFIG = Release
GIT := $(shell which git)
GIT_VER := $(shell ${GIT} rev-list -n1 --abbrev-commit HEAD)
GIT_BRANCH_CMD := ${GIT} branch | grep \* | sed -e 's/\*//g' | sed -e 's/ //g' -e 's/\//_/g'
GIT_BRANCH := $(shell ${GIT_BRANCH_CMD})
CURRENT := $(shell pwd)

all:clean archive doc

clean: clean-unity doc-clean

clean-unity:
	rm -rf KiiCloudStorageSDK/bin/
	rm -rf KiiCloudStorageSDK/AssemblyInfo.dll
	rm -rf KiiCloudStorageSDK/doc.xml
	rm -rf KiiCloudStorageSDK/docs-cloud/
	rm -rf KiiCloudStorageSDK/mdoc/
	rm -rf KiiCloudStorageSDK/KiiCloudStorageTest/bin/
	rm -rf KiiCloudStorageSDK/KiiCloudStorageTest/TestResult.xml
	rm -rf KiiCloudStorageSDK/KiiCloudStorageTest/test-results/
	rm -rf KiiCloudStorageSDK/KiiCloudStorageLargeTest/bin/
	rm -rf KiiCloudStorageSDK/KiiCloudStorageLargeTest/TestResult.xml
	rm -rf KiiCloudStorageSDK/KiiCloudStorageLargeTest/test-results/
	rm -rf test-results/
	rm -rf KiiCloudUnitySDK/bin/
	rm -rf KiiCloudUnitySDK/doc.xml
	rm -rf KiiCloudUnitySDK/mdoc/
	rm -rf KiiCloudUnitySDK/obj/
	rm -rf KiiCloudUnitySDK/AssemblyInfo.dll
	rm -rf KiiCloudUnitySDK/docs-cloud/

build: build-unity

build-unity:
	nuget restore .
	(cd UnityPlugins; \
	sh ./build.sh; \
	cd ../; \
	cd KiiServerSideAuth; \
	sh ./build.sh; \
	cd ../; \
	xbuild /property:Configuration=${CONFIG} KiiCloudUnitySDK.sln)

archive: archive-unity

archive-unity: build-unity
	(cd ${CURRENT}/KiiCloudUnitySDK/bin/${CONFIG}; \
	cp -rpv ../../../UnityPlugins/bin/KiiPushPlugin.unitypackage ./; \
	cp -rpv ../../../KiiServerSideAuth/bin/KiiServerSideAuth.unitypackage ./; \
	cd ${CURRENT}; \
	sh ./build_unitypackage.sh "${CURRENT}/KiiCloudUnitySDK/bin/${CONFIG}"; \
	cd ${CURRENT}/KiiCloudUnitySDK/bin/${CONFIG}; \
	mv ./KiiCloudUnitySDK.unitypackage ./KiiCloudUnitySDK-${CONFIG}-${GIT_VER}.unitypackage)

test: test-unity

test-unity: build-unity
	python runtest.py --file smalltests.js

largetest: largetest-unity

largetest-unity: build-unity
	python runtest.py --file largetests.js

doc: doc-unity

doc-unity: doc-clean build
	(cd ${CURRENT}/KiiCloudUnitySDK; \
	sh genxml.sh ${CONFIG}; \
	mcs /target:library /out:./bin/${CONFIG}/plugin.dll /reference:../UnityPlugins/libs/UnityEngine.dll,./bin/${CONFIG}/KiiCloudStorageSDK.dll,./libs/JsonOrg.dll /define:UNITY_ANDROID ../UnityPlugins/UnityPushPlugin/Assets/Plugins/KiiPushPlugin.cs ../UnityPlugins/UnityPushPlugin/AssemblyInfo.cs ../KiiServerSideAuth/Assets/Plugins/KiiSocialNetworkConnector.cs; \
	mdoc update -r ../UnityPlugins/libs/UnityEngine.dll,../KiiCloudStorageSDK/bin/${CONFIG}/KiiCloudStorageSDK.dll -i doc.xml -o ./mdoc/ ./bin/${CONFIG}/KiiCloudUnitySDK.dll ../KiiCloudStorageSDK/bin/${CONFIG}/KiiCloudStorageSDK.dll ./bin/${CONFIG}/plugin.dll; \
	sed -ie 's/<Title>.*<\/Title>/<Title>Kii Cloud SDK<\/Title>/g' ./mdoc/index.xml; \
	sed -ie 's/<Copyright>.*<\/Copyright>/<Copyright>\(c\) 2014 Kii. All rights reserved. \(Last updated: $(shell date +"%Y-%m-%d")\)<\/Copyright>/g' ./mdoc/index.xml; \
	mdoc export-html -o ./docs-cloud/ ./mdoc/; \
	sed -ie 's/Documentation for this section has not yet been entered\.//g' ./docs-cloud/index.html; \
	sed -ie 's/Documentation for this section has not yet been entered\.//g' ./docs-cloud/KiiCorp.Cloud.Storage/index.html; \
	sed -ie 's/Documentation for this section has not yet been entered\.//g' ./docs-cloud/KiiCorp.Cloud.Storage.Connector/index.html; \
	sed -ie 's/Documentation for this section has not yet been entered\.//g' ./docs-cloud/KiiCorp.Cloud.Analytics/index.html; \
	sed -ie 's/Documentation for this section has not yet been entered\.//g' ./docs-cloud/KiiCorp.Cloud.ABTesting/index.html; \
	sed -ie 's/Documentation for this section has not yet been entered\.//g' ./docs-cloud/KiiCorp.Cloud.Unity/index.html; \
	sed -ie 's/<a href="KiiCorp.Cloud.Unity\/index.html">KiiCorp.Cloud.Unity Namespace<\/a>/<a href="KiiCorp.Cloud.Unity\/index.html">KiiCorp.Cloud.Unity Namespace<\/a><\/h2><h2 class="Section"><a href="KiiCorp.Cloud.Unity.AndroidPlugin\/index.html">Android Native Push Plugin<\/a>/g' ./docs-cloud/index.html; \
	cd ${CURRENT}/UnityPlugins/AndroidPushPlugin; \
	ant javadoc; \
	cp -r ${CURRENT}/UnityPlugins/AndroidPushPlugin/docdir ${CURRENT}/KiiCloudUnitySDK/docs-cloud/KiiCorp.Cloud.Unity.AndroidPlugin; \
	cd ${CURRENT}/KiiCloudUnitySDK; \
	zip -r KiiCloudUnitySDK_doc-${CONFIG}-$(GIT_BRANCH)-$(GIT_VER).zip ./docs-cloud)

doc-clean:
	rm -rf KiiCloudStorageSDK/KiiCloudStorageSDK_doc*.zip
	rm -rf KiiCloudStorageSDK/doc.xml
	rm -rf KiiCloudUnitySDK/KiiCloudUnitySDK_doc*.zip
	rm -rf KiiCloudUnitySDK/doc.xml
	rm -rf UnityPlugins/AndroidPushPlugin/docdir

targets:
	@make -qp | awk -F':' '/^[a-zA-Z0-9][^$$#\/\t=]*:([^=]|$$)/ {split($$1,A,/ /);for(i in A)print A[i]}'

