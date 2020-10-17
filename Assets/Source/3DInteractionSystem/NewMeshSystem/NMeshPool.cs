using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMeshPool : MonoBehaviour
{
    private static List<NMesh> Pool;
    private static int pointer;

    public static NMesh GetMesh(int id)
    {
        return Pool[id];
    }

    public static int Push(NMesh customMesh)
    {
        if (Pool == null)
            Pool = new List<NMesh>();

        pointer = GetAvailableIndex();

        if (pointer < Pool.Count)
            Pool[pointer] = customMesh;
        else
            Pool.Add(customMesh);

        return pointer;
    }

    public static bool Remove(int index)
    {
        if (Pool == null)
            return false;

        var b = Pool[index] == null;

        Pool[index] = null;

        return b;
    }

    public static int GetAvailableIndex()
    {
        if (Pool == null)
            return 0;

        var availableIndex = Pool.FindIndex(mesh => mesh == null);

        return availableIndex != -1 ? availableIndex : Pool.Count;
    }

    public static void Flush()
    {
        if (Pool != null)
            Pool.Clear();
    }
}
