using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MeshSystem
{
    public static class CustomMeshPool
    {
        private static List<CustomMesh> Pool;
        private static int pointer;

        public static CustomMesh GetMesh(int id)
        {
            return Pool[id];
        }

        public static int Push(CustomMesh customMesh)
        {
            if (Pool == null)
                Pool = new List<CustomMesh>();

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
}
