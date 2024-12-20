using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

namespace BW
{
    public static class Util
    {
        public static void AlignObjects_X(this List<GameObject> objects, Vector3 center, float space = 5f)
        {
            int count = objects.Count;
            // 짝수
            if(count % 2 == 0)
            {
                for(int i=0;i<count;i+=2)
                {
                    float offset = space * i/2 + space*0.5f;
                    objects[i].transform.position = center + new Vector3( -offset, 0, 0 );
                    objects[i+1].transform.position = center + new Vector3( offset, 0, 0 );
                }
                
                
            }
            // 홀수
            else
            {
                objects[0].transform.position = center;
                
                for(int i=1;i<count;i+=2)
                {
                    float offset = space * ( (i+1)/2 );
                    objects[i].transform.position = center + new Vector3( -offset, 0, 0 );
                    objects[i+1].transform.position = center + new Vector3( offset, 0, 0 );
                }
            }
        
        }

    }
}

