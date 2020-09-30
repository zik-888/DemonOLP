using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TopicPublisher : UnityPublisher<MessageTypes.Std.String>
    {
        protected MessageTypes.Std.String message;

        protected override void Start()
        {
            base.Start();
            message = new MessageTypes.Std.String("Hello World");
        }

        private void FixedUpdate()
        {
            Publish(message);
        }
    }
}
