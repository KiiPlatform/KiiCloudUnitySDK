using KiiCorp.Cloud.Storage;

using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	private const string APP_ID = "7c1b0f46";
	private const string APP_KEY = "b13521a595a0df19aa9f67fb0966218f";
	private IPage currentPage;
	private Stack pageStack = new Stack();

	public GUISkin DemoGUISkin;

	void Awake ()
	{
		Debug.Log("Initialize");

		Kii.Initialize(APP_ID, APP_KEY, Kii.Site.JP);
	}

	// Use this for initialization
	void Start () {
		currentPage = new TitlePage(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = DemoGUISkin;

		currentPage.OnGUI();
	}

	public void PushPage(IPage next)
	{
		pageStack.Push(currentPage);
		currentPage = next;
	}

	public void PopPage()
	{
		if (pageStack.Count == 0)
		{
			Debug.Log("Stack is empty!");
			return;
		}
		currentPage = (IPage)pageStack.Pop();
	}
}
