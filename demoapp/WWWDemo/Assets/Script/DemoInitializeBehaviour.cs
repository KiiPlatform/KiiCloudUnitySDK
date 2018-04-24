using UnityEngine;
using KiiCorp.Cloud.Unity;
using KiiCorp.Cloud.Storage;
using System.Collections;

public class DemoInitializeBehaviour : KiiInitializeBehaviour {
	public override void Awake()
	{
		AppID = "{APP ID}";
		AppKey = "{APP KEY}";
		ServerUrl = "http://api-jp.kii.com/api";
		base.Awake();
	}
}
