using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;
using KiiCorp.Cloud.Storage.Connector;

[ExecuteInEditMode()]
public class Main : MonoBehaviour {

	private static AppInfo APP_US = new AppInfo("f7df20fc", "864b6211ba96ed86992b584b11756ee8", Kii.Site.US);
	private static AppInfo APP_JP = new AppInfo("abdebab3", "286457fa6ecb7a2aa694e90855bbcc4e", Kii.Site.JP);
	private AppInfo current = APP_US;
	private static string topicname = "push_topic_" + Environment.TickCount;
	protected string message = "";
	private ScalableGUI gui = null;
	private KiiPushPlugin kiiPushPlugin = null;
	private int screenWidth = 520;
	private int screenHeight = 924;

	void Start () {
		gui = new ScalableGUI (screenWidth, screenHeight);

		this.kiiPushPlugin = GameObject.Find ("KiiPushPlugin").GetComponent<KiiPushPlugin> ();
		this.kiiPushPlugin.OnPushMessageReceived += (ReceivedMessage message) => {
			this.message = "Push message received :)\n";
			switch (message.PushMessageType)
			{
			case ReceivedMessage.MessageType.PUSH_TO_APP:
				PushToAppMessage appMsg = (PushToAppMessage)message;
				this.message += "Bucket=" + appMsg.KiiBucket.Uri.ToString() + "\n";
				this.message += "Object=" + appMsg.KiiObject.Uri.ToString() + "\n";
				break;
			case ReceivedMessage.MessageType.PUSH_TO_USER:
				PushToUserMessage userMsg = (PushToUserMessage)message;
				this.message += "User=" + userMsg.ObjectScopeUser.Uri.ToString() + "\n";
				this.message += "Topic=" + userMsg.KiiTopic.Uri.ToString() + "\n";
				break;
			case ReceivedMessage.MessageType.DIRECT_PUSH:
				this.message += "Title=" + message.GetString("Title") + "\n";
				this.message += "Body=" + message.GetString("Body") + "\n";
				break;
			}
			this.message += "Type=" + message.PushMessageType + "\n";
			this.message += "Sender=" + message.Sender + "\n";
			this.message += "Scope=" + message.ObjectScope + "\n";
			this.message += "Payload=" + message.ToJson().ToString();
		};
	}
	void Update () {
	}
	void OnGUI() {
		gui.Label (10, 10, 500, 50, (current == APP_US ? "US:" : "JP:") + current.AppID);
		if (gui.Button (10, 55, 245, 100, "Switch APP to US"))
		{
			this.current = APP_US;
			KiiInitializeBehaviour.Instance.SwitchApp(this.current.AppID, this.current.AppKey, this.current.Site);
			this.message = string.Format ("Switch APP to {0} : {1}\n", this.current.Site, this.current.AppID);
		}
		if (gui.Button (265, 55, 245, 100, "Switch APP to JP"))
		{
			this.current = APP_JP;
			KiiInitializeBehaviour.Instance.SwitchApp(this.current.AppID, this.current.AppKey, this.current.Site);
			this.message = string.Format ("Switch APP to {0} : {1}\n", this.current.Site, this.current.AppID);
		}
		if (gui.Button (10, 160, 160, 100, "Create Random User"))
		{
			this.message = "";
			string username = "u" + DateTime.Now.Ticks.ToString();
			KiiUser user = KiiUser.BuilderWithName (username).Build ();
			user.Register ("pa$$sword", (KiiUser u, Exception e1)=>{
				if (e1 == null)
				{
					this.message = "SUCCESS:\nuser=" + u.Uri.ToString();
				}
				else
				{
					this.ShowException("Failed to register user", e1);
				}
			});
		}
		if (gui.Button (180, 160, 160, 100, "Facebook Login"))
		{
			this.message = "";
			var connector = this.gameObject.AddComponent<KiiSocialNetworkConnector> ();
			connector.LogIn (Provider.FACEBOOK,
			                 (KiiUser user, Provider provider, Exception exception) => {
				if (exception == null) {
					this.message = "SUCCESS:\nuser=" + user.Uri.ToString();
				}
				else
				{
					this.ShowException("Failed to login with social connector", exception);
				}
				// Destroy connector if required.
				Destroy (connector);
			});
		}
		if (gui.Button (350, 160, 160, 100, "Show user info"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			this.message = "*** User tokens ***\n";
			this.message += GetDictionaryContents(u.GetAccessTokenDictionary());
			this.message += "\n*** Social tokens ***\n";
			this.message += GetDictionaryContents(u.GetSocialAccessTokenDictionary());
		}
		if (gui.Button (10, 265, 160, 100, "Create 5-Object\n(1 Object/sec)"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			StartCoroutine(CreateNewObjects(5));
		}
		if (gui.Button (10, 370, 160, 100, string.Format("Create topic\n({0})", topicname)))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			KiiTopic topic = u.Topic(topicname);
			topic.Save((KiiTopic retTopic, Exception retException) => {
				if (retException == null)
				{
					this.message = "SUCCESS:\ntopic=" + retTopic.Uri.ToString();
				}
				else
				{
					this.ShowException("Failed to create topic", retException);
				}
			});
		}
		if (gui.Button (180, 370, 160, 100, "Subscribe topic"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			KiiTopic topic = u.Topic(topicname);
			u.PushSubscription.Subscribe(topic, (KiiSubscribable retSub, Exception retException) => {
				if (retException == null)
				{
					this.message = "SUCCESS:\nsubscribed topic=" + ((KiiTopic)retSub).Uri.ToString();
				}
				else
				{
					this.ShowException("Failed to subscribe topic", retException);
				}
			});
		}
		if (gui.Button (350, 370, 160, 100, "Check topic existence"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			KiiTopic topic = u.Topic(topicname);
			try
			{
				bool retExists = topic.Exists();
				this.message = "SUCCESS:\ntopic existence=" + retExists;
			}
			catch (Exception e)
			{
				this.ShowException("Failed to check topic existence", e);
			}
		}
		if (gui.Button (10, 480, 160, 100, "Install Push"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			#if UNITY_IPHONE
			bool development = true;
			KiiPushInstallation.DeviceType deviceType = KiiPushInstallation.DeviceType.IOS;
			#elif UNITY_ANDROID
			bool development = true;
			KiiPushInstallation.DeviceType deviceType = KiiPushInstallation.DeviceType.ANDROID;
			#else
			this.message = "Push feature does not support on this platform :P";
			return;
			#endif
			
			this.kiiPushPlugin.RegisterPush((string pushToken, Exception e0) => {
				if (e0 != null)
				{
					this.ShowException("Failed to register push : " + this.kiiPushPlugin.SenderID, e0);
					return;
				}
				this.message = "Push token : " + pushToken;
				KiiUser.PushInstallation(development).Install(pushToken, deviceType, (Exception e1) => {
					if (e1 != null)
					{
						this.ShowException("Failed to install push", e1);
					}
					else
					{
						this.message += "SUCCESS:\ninstall push=" + pushToken;
						SavePushInformation(deviceType, pushToken);
					}
				});
			});
		}
		if (gui.Button (180, 480, 160, 100, "Uninstall Push"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			#if UNITY_IPHONE
			bool development = true;
			KiiPushInstallation.DeviceType deviceType = KiiPushInstallation.DeviceType.IOS;
			#elif UNITY_ANDROID
			bool development = true;
			KiiPushInstallation.DeviceType deviceType = KiiPushInstallation.DeviceType.ANDROID;
			#else
			this.message = "Push feature does not support on this platform :P";
			return;
			#endif

			string pushToken = LoadPushInformation(deviceType);
			KiiUser.PushInstallation(development).Uninstall(pushToken, deviceType, (Exception e0) => {
				if (e0 != null)
				{
					this.ShowException("Failed to uninstall push", e0);
					return;
				}
				this.kiiPushPlugin.UnregisterPush((Exception e1) => {
					if (e1 != null)
					{
						this.ShowException("Failed to unregister push", e1);
					}
					else
					{
						this.message = "SUCCESS:\nuninstall push=" + pushToken;
						ClearPushInformation(deviceType);
					}
				});
			});
		}
		if (gui.Button (350, 480, 160, 100, "Send Push"))
		{
			this.message = "";
			KiiUser u = KiiUser.CurrentUser;
			if (u == null)
			{
				this.message = "User is not logged in :P";
				return;
			}
			
			KiiTopic topic = u.Topic(topicname);

			// Build a push message.
			KiiPushMessageData data = new KiiPushMessageData()
				.Put("message", "Hi, switch app :)");
			KiiPushMessage message = KiiPushMessage.BuildWith(data).SendAppID(true).Build();
			
			// Send the push message.
			topic.SendMessage(message, (KiiPushMessage retMessage, Exception retException) => {
				if (retException == null)
				{
					this.message = "SUCCESS:\nsend message=" + retMessage.ToString();
				}
				else
				{
					this.ShowException("Failed to send message", retException);
				}
			});
		}
		int messageHeight = 590;
		gui.Label (10, messageHeight, 500, screenHeight - messageHeight, this.message);
	}
	private IEnumerator CreateNewObjects(int count)
	{
		for (int index = 0; index < count; index++)
		{
			yield return new WaitForSeconds (1f);
			KiiObject obj = Kii.Bucket("user_bucket").NewKiiObject();
			obj["time"] = DateTime.Now.Ticks;
			obj.Save((KiiObject o, Exception e) => {
				if (e == null)
				{
					this.message += "SUCCESS:\nobject=" + o.Uri.ToString() + "\n";
				}
				else
				{
					this.ShowException("Failed to save object", e);
				}
			});
		}
	}
	private string GetDictionaryContents(Dictionary<string, object> dictionary)
	{
		if (dictionary == null)
		{
			return "Dictionary is null";
		}
		Dictionary<string, object>.KeyCollection.Enumerator keys = dictionary.Keys.GetEnumerator();
		string contents = "";
		while (keys.MoveNext())
        {
			if (contents != "")
			{
				contents += "\n";
			}
            string key = keys.Current;
			contents += string.Format ("[key]: {0} / [value]: {1}", key, dictionary[key].ToString());
        }
		return contents;
	}
	private static string LoadPushInformation (KiiPushInstallation.DeviceType deviceType)
	{
		if (!PlayerPrefs.HasKey (deviceType.ToString ()))
		{
			return null;
		}
		return PlayerPrefs.GetString (deviceType.ToString ());
	}
	private static void SavePushInformation (KiiPushInstallation.DeviceType deviceType, string pushToken)
	{
		PlayerPrefs.SetString (deviceType.ToString (), pushToken);
	}
	private static void ClearPushInformation (KiiPushInstallation.DeviceType deviceType)
	{
		PlayerPrefs.DeleteKey (deviceType.ToString ());
	}
	private void ShowException(string msg, Exception e)
	{
		this.message += "ERROR: " + msg + "\n" + e.GetType () + "\n";
		if (e.Data != null)
		{
			this.message += "Data=" + e.Data.ToString() + "\n";
		}
		if (e.InnerException != null)
		{
			this.message += "InnerExcepton=" + e.InnerException.GetType() + "\n";
			this.message += "InnerExcepton.Message=" + e.InnerException.Message + "\n";
			this.message += "InnerExcepton.Stacktrace=" + e.InnerException.StackTrace + "\n";
		}
		this.message += "Source=" + e.Source + "\n";
		this.message += e.Message + "\n" + e.StackTrace;
	}
	private class AppInfo
	{
		public string AppID;
		public string AppKey;
		public Kii.Site Site;
		public AppInfo(string appID, string appKey, Kii.Site site)
		{
			this.AppID = appID;
			this.AppKey = appKey;
			this.Site = site;
		}
	}
}
