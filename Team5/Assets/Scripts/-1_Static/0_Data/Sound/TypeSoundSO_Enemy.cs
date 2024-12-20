using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyTypeSound", menuName = "SO/Sound/Type_Enemy", order = int.MaxValue)]
public class TypeSoundSO_Enemy : TypeSoundSO<EnemyType>
{
    protected override EnemyType GetType(Transform t)
    {
        if(t.TryGetComponent(out Enemy e))
        {
            return e.data.type;
        }
        return default;
    }
}
