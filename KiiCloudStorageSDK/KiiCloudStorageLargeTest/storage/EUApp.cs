using System;

namespace KiiCorp.Cloud.Storage
{
    public class EUApp : BaseApp
    {
        const string APP_ID = "4cc92350";
        const string APP_KEY = "008dc38b1ee13770800f6d9c8cbe54c2";
        const string CLIENT_ID = "857a8b06f1c14c27e18760d3437c2f34";
        const string CLIENT_SECRET = "cd58518e880dd8c4dbdb49c63029cf4552eb9f75ed7a684de35f4fd051113e3b";
        const string BASEURL = "https://api-eu.kii.com/api";

        public string AppId {
            get {
                return APP_ID;
            }
        }

        public string AppKey {
            get {
                return APP_KEY;
            }
        }

        public string BaseUrl {
            get {
                return BASEURL;
            }
        }

        public string ClientId {
            get {
                return CLIENT_ID;
            }
        }

        public string ClientSecret {
            get {
                return CLIENT_SECRET;
            }
        }
    }
}

