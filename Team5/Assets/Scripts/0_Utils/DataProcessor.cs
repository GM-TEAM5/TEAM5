using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BW.Util
{
    public static class DataProcessor 
    {
        public static Vector3 WithStandardHeight(this Vector3 pos)
        {
            return new Vector3(pos.x, 1f, pos.z);
        }

        public static Vector3 WithFloorHeight(this Vector3 pos)
        {
            return new Vector3(pos.x, 0f, pos.z);
        }
    }
}

