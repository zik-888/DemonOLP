/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.RosBookSamples
{
    public class AddTwoIntsResponse : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "ros_book_samples/AddTwoInts";

        public uint sum;

        public AddTwoIntsResponse()
        {
            this.sum = 0;
        }

        public AddTwoIntsResponse(uint sum)
        {
            this.sum = sum;
        }
    }
}
