using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Defines action of topic. Applicable for <see cref="KiiTopic"/>.
    /// </summary>
    /// <remarks></remarks>
    public enum TopicAction
    {
        /// <summary>
        /// Action send message to a topic.
        /// </summary>
        SEND_MESSAGE_TO_TOPIC,

        /// <summary>
        /// Action of subscribe the topic.
        /// </summary>
        SUBSCRIBE_TO_TOPIC 
    }
}

