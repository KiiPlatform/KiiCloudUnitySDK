using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides Topic ACL operations.
    /// </summary>
    /// <remarks>
    /// To get this instance, <see cref="KiiTopic.Acl"/>
    /// </remarks>
    public class KiiTopicACL : KiiACL<KiiTopic, TopicAction>
    {
        private const string ACTION_SEND_MESSAGE_TO_TOPIC = "SEND_MESSAGE_TO_TOPIC";
        private const string ACTION_SUBSCRIBE_TO_TOPIC = "SUBSCRIBE_TO_TOPIC";

        private static string[] ACTION_NAMES = {
            ACTION_SEND_MESSAGE_TO_TOPIC,
            ACTION_SUBSCRIBE_TO_TOPIC,
        };

        internal KiiTopicACL(KiiTopic parent)
        {
            Parent = parent;
        }
        
        internal KiiTopicACL(KiiTopic parent, TopicAction action)
        {
            Parent = parent;
            Action = action;
        }

        #region override methods
        internal override string ToActionString(TopicAction action)
        {
            switch (action)
            {
                case TopicAction.SEND_MESSAGE_TO_TOPIC:
                    return ACTION_SEND_MESSAGE_TO_TOPIC;
                case TopicAction.SUBSCRIBE_TO_TOPIC:
                    return ACTION_SUBSCRIBE_TO_TOPIC;
                default:
                    throw new SystemException ("unexpected error." + action.GetType ().ToString ());
            }
        }        
        internal override TopicAction ToAction(string actionName)
        {
            switch (actionName)
            {
                case ACTION_SEND_MESSAGE_TO_TOPIC:
                    return TopicAction.SEND_MESSAGE_TO_TOPIC;
                case ACTION_SUBSCRIBE_TO_TOPIC:
                    return TopicAction.SUBSCRIBE_TO_TOPIC;
                default:
                    throw new ArgumentException("actionName is not TopicAction");
            }
        }
        internal override KiiACL<KiiTopic, TopicAction> CreateFromAction(KiiTopic parent, TopicAction action)
        {
            return new KiiTopicACL(parent, action);
        }
        #endregion

        #region properties
        internal override string ParentID
        {
            get
            {
                return Parent.Name;
            }
        }
        internal override string ParentUrl
        {
            get
            {
                return Parent.Url;
            }
        }
        internal override string[] ActionNames
        {
            get
            {
                return ACTION_NAMES;
            }
        }
        #endregion
    }
}

