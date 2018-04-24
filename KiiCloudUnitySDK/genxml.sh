list=`find unity -name '*.cs'`
list+=' '
list+=`find ../KiiCloudStorageSDK/analytics/ -name '*.cs'`
list+=' '
list+=`find ../KiiCloudStorageSDK/storage/ -name '*.cs'`
list+=' '
list+=`find ../KiiCloudStorageSDK/abtesting/ -name '*.cs'`
list+=' ../KiiServerSideAuth/Assets/Plugins/KiiSocialNetworkConnector.cs '
list+=' ../UnityPlugins/UnityPushPlugin/Assets/Plugins/KiiPushPlugin.cs '
list+=' '
list+=' ../KiiCloudStorageSDK/AssemblyInfo.cs'

config=$1
if [ -z $config ]
then
    config="Release"
fi

echo $list
mcs /target:library /doc:doc.xml /reference:libs/JsonOrg.dll,libs/UnityEngine.dll,../KiiCloudStorageSDK/bin/$config/KiiCloudStorageSDK.dll,bin/$config/KiiCloudUnitySDK.dll $list
rm -rf unity/http/impl/KiiAsyncHttpUnityClientImpl.dll
