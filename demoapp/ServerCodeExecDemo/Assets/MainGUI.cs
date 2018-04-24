using UnityEngine;
using System.Collections;
using System.Text;
using System;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;
using JsonOrg;

public class MainGUI : KiiInitializeBehaviour {

    private string entryNameValue = "entryName";
    private string versionValue = "version";
    private string labelText = "Result";
    private ScalableGUI gui = null;

    public override void Awake() {
        this.AppID = "3943f650";
        this.AppKey = "0675b0e0c96c33e3e8b9c282291dfe6f";
        this.Site = Kii.Site.JP;
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        gui = new ScalableGUI ();
    }
    // Update is called once per frame
    void Update () {
    
    }

    void OnGUI() {
        this.entryNameValue = gui.TextField (
                10,
                10,
                300,
                50,
            this.entryNameValue);
        this.versionValue = gui.TextField (
                10,
                70,
                300,
                50,
            this.versionValue);
        if (gui.Button (
                    10,
                    130,
                    300,
                    60,
                "ServerCodeEntry(entryName)"))
        {
            try {
                execEntry(Kii.ServerCodeEntry(this.entryNameValue));
            } catch (Exception e) {
                this.labelText = e.ToString();
            }
        }
        if (gui.Button (
                    10,
                    200,
                    300,
                    60,
                "ServerCodeEntry(entryName, version)"))
        {
            try {
                execEntry(Kii.ServerCodeEntry(this.entryNameValue,
                        this.versionValue));
            } catch (Exception e) {
                this.labelText = e.ToString();
            }
        }
        if (gui.Button (
                    10,
                    270,
                    145,
                    60,
                    "User login"))
        {
            this.labelText = "";
            string username = "u" + DateTime.Now.Ticks.ToString();
            KiiUser user = KiiUser.BuilderWithName (username).Build ();
            user.Register ("pa$$sword", (KiiUser u, Exception e1)=>{
                    if (e1 == null)
                    {
                    this.labelText = "SUCCESS:\nuser=" + u.Uri.ToString();
                    }
                    else
                    {
                    this.labelText = e1.ToString();
                    }
                    });
        }
        if (gui.Button (
                    165,
                    270,
                    145,
                    60,
                    "User logout"))
        {
            KiiUser.LogOut();
            this.labelText = "User logged out";
        }
        gui.TextField(
                10,
                350,
                300,
                120,
                this.labelText);
    }

    private void execEntry(KiiServerCodeEntry entry) {
        if (entry == null) {
            this.labelText = "entry is null.";
            return;
        }
        entry.Execute (
                KiiServerCodeEntryArgument.NewArgument (new JsonObject ()), 
                (KiiServerCodeEntry retEntry, KiiServerCodeEntryArgument retArgument, KiiServerCodeExecResult retResult, Exception retException) => {
                if (retException != null) {
                    this.labelText = retException.ToString ();
                    return;
                }
                if (retResult != null) {
                    this.labelText = ResultToString (retResult);
                } else {
                    this.labelText = "result is null.";
                }
        });
    }

    private static string ResultToString(KiiServerCodeExecResult result) {
        StringBuilder builder = new StringBuilder();
        builder.Append("ExecutedSteps = ");
        builder.AppendLine(result.ExecutedSteps.ToString());
        builder.Append("ReturnedValue = ");
        builder.AppendLine(result.ReturnedValue.ToString());
        
        return builder.ToString();
    }
}
