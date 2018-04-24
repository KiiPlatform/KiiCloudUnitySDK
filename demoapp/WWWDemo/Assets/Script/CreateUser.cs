using UnityEngine;
using System;
using System.Collections;
using JsonOrg;
using KiiCorp.Cloud.Storage;

[ExecuteInEditMode()]
public class CreateUser : ScriptBase {
	
	void Start () {
	}
	void Update () {
	}
	
	// void OnGUI() {
	// this.email = GUI.TextField (new Rect (10, 10, 800, 100), this.email);
	// GUI.Label (new Rect (10, 220, 500, 1000), this.message);
	// if (GUI.Button (new Rect (10, 115, 250, 100), "Create User"))
	// {
	// try
	// {
	// KiiUser user = KiiUser.BuilderWithEmail (this.email).WithName ("U" + Environment.TickCount).Build ();
	// user.Register ("pa$$sword");
	// this.message = "SUCCESS:";
	// }
	// catch (KiiCorp.Cloud.Storage.NetworkException e)
	// {
	// this.message = "ERROR: " + e.GetType () + "\n" +
	// "Status=" + e.Status + "\n" +
	// "Data=" + e.Data.ToString() + "\n" +
	// "InnerExcepton=" + e.InnerException.GetType() + "\n" +
	// "InnerExcepton.Message=" + e.InnerException.Message + "\n" +
	// "InnerExcepton.Stacktrace=" + e.InnerException.StackTrace + "\n" +
	// "Source=" + e.Source + "\n" +
	// e.Message + "\n" + e.StackTrace;
	// }
	// catch (Exception e)
	// {
	// this.message = "ERROR: " + e.GetType () + "\n";
	// if (e.Data != null)
	// {
	// this.message += "Data=" + e.Data.ToString() + "\n";
	// }
	// if (e.InnerException != null)
	// {
	// this.message += "InnerExcepton=" + e.InnerException.GetType() + "\n";
	// this.message += "InnerExcepton.Message=" + e.InnerException.Message + "\n";
	// this.message += "InnerExcepton.Stacktrace=" + e.InnerException.StackTrace + "\n";
	// }
	// this.message += "Source=" + e.Source + "\n";
	// this.message += e.Message + "\n" + e.StackTrace;
	// }
	// }
	// }
	void OnGUI() {
		GUI.Label (new Rect (10, 10, 800, 50), GetType().ToString());
		this.username = GUI.TextField (new Rect (10, 65, 800, 100), this.username);
		GUI.Label (new Rect (10, 275, 500, 1000), this.message);
		if (GUI.Button (new Rect (10, 170, 250, 100), "Create User"))
		{
			KiiUser user = KiiUser.BuilderWithName (this.username).Build ();
			user.Register ("pa$$sword", (KiiUser u, Exception e1)=>{
				if (e1 == null)
				{
					this.message = "SUCCESS:" + u.Uri.ToString();
				}
				else
				{
					this.ShowException("Failed to register user", e1);
				}
			});
		}
	}
	string GetDeviceID()
	{
		string deviceID = ReadDeviceIDFromStorage();
		if (deviceID == null)
		{
			deviceID = Guid.NewGuid().ToString();
			SaveDeviceID(deviceID);
		}
		return deviceID;
	}
	string ReadDeviceIDFromStorage()
	{
		string id = PlayerPrefs.GetString("deviceId", null);
		if (id == null || id.Length == 0)
		{
			id = System.Guid.NewGuid().ToString();
		}
		return id;
	}
	void SaveDeviceID(string id)
	{
		PlayerPrefs.SetString("deviceId", id);
		PlayerPrefs.Save();
	}
}
