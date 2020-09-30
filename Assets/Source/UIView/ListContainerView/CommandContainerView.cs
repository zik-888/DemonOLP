using System.Collections;
using UnityEngine;

public enum CommandType { EdgeMovementCommand, SurfaceMovementCommand }

public class CommandContainerView : ListContainerView
{
    public RobotCommand GetCommand(int num)
    {
        return reorderableList.Content.GetChild(num).GetComponent<CommandView>().RC;
    }

    override protected IEnumerator ContainerUpdateInNextFrame()
    {
        yield return new WaitForEndOfFrame();

        int contentCount = reorderableList.Content.childCount;

        for (int i = 0; contentCount > i; i++)
        {
            reorderableList.Content.GetChild(i).GetComponent<CommandView>().UpdateNumElement(i);
        }
    }
}
