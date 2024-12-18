using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BW
{
    public static class GameConstants 
    {
        public static int playerLayer =  1<<LayerMask.NameToLayer("Player");
        public static int enemyLayer =  1<<LayerMask.NameToLayer("Enemy");
    }
}


