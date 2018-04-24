list=`find storage -name '*.cs'`
list+=' '
list+=`find abtesting -name '*.cs'`
list+=' '
list+=`find analytics -name '*.cs'`
list+=' ../UnityPlugins/UnityPushPlugin/Assets/Plugins/KiiPushPlugin.cs '
list+=' AssemblyInfo.cs'
echo $list
mcs /target:library /doc:doc.xml /reference:libs/JsonOrg.dll,../UnityPlugins/libs/UnityEngine.dll /define:UNITY_ANDROID $list 
rm -rf storage/AccessControllable.dll
