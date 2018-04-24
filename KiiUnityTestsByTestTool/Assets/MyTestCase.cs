using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class MyTestCase : MonoBehaviour {
	bool testResult = false;
	void Start () {
		long unixTime = (long)(DateTime.UtcNow - new DateTime (1970, 1, 1, 0, 0, 0)).TotalSeconds;
		string username = "username" + unixTime;
		string password = "123456";
		KiiUser user = KiiUser.BuilderWithName (username).Build ();
		user.Register (password, (KiiUser u, Exception e) => {
			if (e != null) {
				return;
			}
			Debug.Log ("In callback, username: " + u.Username);
			testResult = username.Equals(u.Username);
		});
	}
	
	void Update () {
		if (testResult) {
			throw new TestSuccessException();
		}
	}
	
	private class TestSuccessException : Exception
	{
	}

}
