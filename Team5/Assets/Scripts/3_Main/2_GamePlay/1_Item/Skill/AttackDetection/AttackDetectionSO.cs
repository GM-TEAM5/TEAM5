using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackDetectionSO : ScriptableObject
{
    public abstract void Detect(Vector3 attackDir, Vector3 effectPos, BasicAttackSO data);
}
