/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.HaoAsakura
{
    public class StartServiceRequest : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "HaoAsakura/StartService";

        public long a;

        public StartServiceRequest()
        {
            this.a = 0;
        }

        public StartServiceRequest(long a)
        {
            this.a = a;
        }
    }
}
