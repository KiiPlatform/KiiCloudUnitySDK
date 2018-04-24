using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Storage.Connector;
using KiiCorp.Cloud.Unity;

public class MainGUI : KiiInitializeBehaviour
{
    ScalableGUI gui = null;
    Vector2 currentScrollPosition;

    static MainGUI()
    {
    }

    public override void Awake()
    {
        this.AppID = "9ab34d8b";
        this.AppKey = "7a950d78956ed39f3b0815f0f001b43b";
        this.Site = Kii.Site.JP;
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        gui = new ScalableGUI(width, height);
    }

    const int width = 320;
    const int height = 480;
    const int baseMargin = 10;

    const int providerColumn = 3;
    const int providerMargin = 5;
    const int providerHeight = 30;
    const int providerWidth = (width - (baseMargin * 2) - (providerMargin * (providerColumn - 1))) / providerColumn;

    void OnGUI()
    {
        int columnCount = 0;
        int rowCount = 0;
        int providerAreaHeight = 0;
        foreach (Provider p in Enum.GetValues(typeof(Provider)))
        {
            if (gui.Button(
                    baseMargin + (providerMargin + providerWidth) * columnCount,
                    baseMargin + (providerMargin + providerHeight) * rowCount,
                    providerWidth,
                    providerHeight,
                    p.ToString()))
            {
                var connector = this.gameObject.AddComponent<KiiSocialNetworkConnector>();
                connector.LogIn(p,
                    (KiiUser user, Provider provider, Exception exception) =>
                    {
                        if (user != null)
                        {
                            ConsoleLog.D(KiiUserToString(user));
                        }
                        else
                        {
                            ConsoleLog.D(exception.ToString());
                        }
                        UnityEngine.Object.Destroy(connector);
                    });
            }
            columnCount++;
            if (columnCount >= providerColumn)
            {
                columnCount = 0;
                rowCount++;
            }
            providerAreaHeight = (baseMargin * 2) + (providerMargin * rowCount) + (providerHeight * (rowCount + 1));
        }

        // Clear Log
        if (gui.Button(
                baseMargin,
                providerAreaHeight,
                (width - (baseMargin * 3)) / 2,
                providerHeight,
                "Clear Log"))
        {
            ConsoleLog.Clear();
        }
        // Show user info
        if (gui.Button(
                baseMargin * 2 + (width - (baseMargin * 3)) / 2,
                providerAreaHeight,
                (width - (baseMargin * 3)) / 2,
                providerHeight,
                "Show UserInfo"))
        {
            ConsoleLog.D(KiiUserToString(KiiUser.CurrentUser));
        }

        int logHeight = providerAreaHeight + providerHeight + baseMargin;
        // Log area
        currentScrollPosition = gui.ScrollView(baseMargin,
            logHeight,
            width - baseMargin * 2,
            height - logHeight,
            currentScrollPosition,
            ConsoleLog.GetLogsByString());
    }

    static string KiiUserToString(KiiUser user)
    {
        StringBuilder builder = new StringBuilder();
        if (user == null)
        {
            builder.Append("No user logged in!");
            return builder.ToString();
        }

        builder.AppendLine("[Kii UserID]");
        builder.Append("* ");
        builder.AppendLine(user.ID);
        Dictionary<string, object> userToken = user.GetAccessTokenDictionary();
        builder.AppendLine("[KiiUser AccessToken]");
        builder.AppendLine(ExpandDictionary(userToken));

        // Add linked social account information.
        builder.AppendLine("[Linked social accounts]");
        Dictionary<Provider, SocialAccountInfo> socialAccounts = user.LinkedSocialAccounts;
        foreach (KeyValuePair<Provider, SocialAccountInfo> account in socialAccounts)
        {
            builder.Append("* ");
            builder.Append(account.Key.ToString());
            builder.Append(" = { ");
            builder.Append("id :" + account.Value.SocialAccountId + " createdAt: " + account.Value.CreatedAt);
            builder.AppendLine(" }");
        }

        // Social tokens
        var socialToken = user.GetSocialAccessTokenDictionary();
        builder.AppendLine("[Social AccessToken]");
        builder.Append(ExpandDictionary(socialToken));

        return builder.ToString();
    }

    static string ExpandDictionary(Dictionary<string, object> dict)
    {
        JsonOrg.JsonObject json = new JsonOrg.JsonObject(dict);
        return json.ToString(2);
    }
}
