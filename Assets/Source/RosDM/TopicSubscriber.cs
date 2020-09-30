using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TopicSubscriber : UnitySubscriber<MessageTypes.Std.String>
    {
        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            Debug.Log(message.data);
        }
    }
}
