// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    internal abstract class JsonMapper
    {
        protected JsonObject json;
        internal JsonMapper(JsonObject json)
        {
            this.json = json;
        }
        internal JsonObject Json {get{return this.json;}}
        internal abstract string Origin {get;}
        internal abstract string Sender {get;}
        internal abstract string ObjectScopeType {get;}
        internal abstract string Topic {get;}
        internal abstract string ObjectScopeGroupID {get;}
        internal abstract string ObjectScopeUserID {get;}
        internal abstract string BucketID {get;}
        internal abstract string ObjectID {get;}
        internal abstract string BucketType {get;}
        internal abstract bool IsPushToAppMessage();
        internal abstract bool IsPushToUserMessage();
    }
}

