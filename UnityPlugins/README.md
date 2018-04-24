# KiiUnityPushPlugin
This project is native plugin to receive push notification for the Unity3D.  
You can customize the behavior when app receives a push notification. 


## Overview about the push notification mechanism

      +--------------------------+
      |                          |
      |  Google Cloud Messaging  |
      |                          |
      +--------------------------+
                  |
                  | 1. Push Notification from Google server
                  |
    +-------------+-----------------+---------------------------------------------------------------+
    |             |                 |                                                               |
    |             V                 |                                                               |
    | +--------------------------+  |                        +------------------+                   |
    | |                          |  | 2. UnitySendMessage()  |                  |                   |
    | |  androidpushplugin.jar   |--+----------+-----------> | KiiPushPlugin.cs |                   |
    | |                          |  |          |             |                  |                   |
    | +--------------------------+  |          |             +------------------+                   |
    |                               |          |                      |                             |
    |                               |          |                      |                             |
    |                               |          |                      | 3. OnPushMessageReceived()  |
    |                               |          |                      |                             |
    |                               |          |                      V                             |
    | +--------------------------+  |          |             +------------------+                   |             
    | |                          |  |          |             |                  |                   |
    | | UIApplication+KiiCloud.m |--+----------+             | YourUnityCode.cs |                   |
    | |                          |  |                        |                  |                   |
    | +--------------------------+  |                        +------------------+                   |
    |            4                  |                                                               |
    |            |                  |                                                               |
    +-----[Native|Plugin Layer]-----+-----------------------[Unity Layer]---------------------------+
                 |
                 | 1. Push Notification from Apple server
                 |
      +------------------------+
      |                        |
      |          APNS          |
      |                        |
      +------------------------+

## Support for iOS 8 
1. Plugin supports for iOS8 remote push registration as well as earlier iOS version on XCode 6 development environment.
1. If your iOS development environment still using XCode 5.xx, you should remove iOS8 specific push API on `UnityPushPlugin/Assets/Plugins/iOS/DefaultPush.m` to avoid compile error.
1. For further explanations, please refer to [documentation](http://documentation.kii.com/en/guides/unity/managing-push-notification/customize-ios-push/).

## Customize the behavior of iOS
1. implement a custom code by editing `UnityPushPlugin/Assets/Plugins/iOS/UIApplication+KiiCloud.m`
1. add `DllImport` statement to `UnityPushPlugin/Assets/Plugins/KiiPushPlugin.cs` if you want to use the native method which you've implemented. 

## Customize the behavior of Android
1. import `AndroidPushPlugin/` project into the Eclipse.
1. implement a custom code.
1. add code to call native code using AndroidJavaObject class to `UnityPushPlugin/Assets/Plugins/KiiPushPlugin.cs`.


## Build

### Requirements
1. [AndroidSDK](http://developer.android.com/sdk/index.html)
1. [Apache Ant](http://ant.apache.org/)
1. [Unity Pro](http://unity3d.com/)

### How to build
1. `$ sh build.sh`
1. KiiPushPlugin.unitypackage will be generated in bin directory.


## Support
If you have any questions, please feel free to ask at [community](http://community.kii.com/).

