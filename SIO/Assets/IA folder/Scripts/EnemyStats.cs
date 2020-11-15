using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public float moveSpeed = 1;
    public float lookRange = 40f;
    public float lookSphereCastRaduis = 1f;

    public float attackRange = 1f;
    public float attaqueRate = 1f;
    public float attaqueForce = 15f;
    public int attaqueDamage = 50;

    public float searchDuration = 4f;
    public float serachingTurnSpeed = 120f;
}
