/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;

namespace RosSharp.RosBridgeClient.MessageTypes.ScanningSystemCore
{
    public class ScanningActionFeedback : ActionFeedback<ScanningFeedback>
    {
        public const string RosMessageName = "scanning_system_core/ScanningActionFeedback";

        public ScanningActionFeedback() : base()
        {
            this.feedback = new ScanningFeedback();
        }

        public ScanningActionFeedback(Header header, GoalStatus status, ScanningFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
    }
}