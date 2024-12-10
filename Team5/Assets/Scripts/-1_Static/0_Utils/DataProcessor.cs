using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;




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


        // public static float GetSqrDistWith(this Vector3 pos, Vector3 targetPos)
        // {
        //     return (targetPos- pos).sqrMagnitude;
        // }
    }

    public static class Math
    {
        public static int GetRandom(int min, int max)
        {
            // byte[] buffer = new byte[4];
            // RandomNumberGenerator.Fill(buffer);
            // uint randomUint = BitConverter.ToUInt32(buffer, 0);

            // // 정수 범위로 변환
            // return (int)(randomUint % (max - min)) + min;
            return UnityEngine.Random.Range(min, max);
        }

        public static float GetRandom(float min, float max)
        {
            byte[] buffer = new byte[4];
            RandomNumberGenerator.Fill(buffer);
            uint randomUint = BitConverter.ToUInt32(buffer, 0);

            // 부동소수점 범위로 변환
            return (randomUint / (float)uint.MaxValue) * (max - min) + min;
        }
    }
}

