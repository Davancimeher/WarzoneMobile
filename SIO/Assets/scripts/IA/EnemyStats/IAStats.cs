using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/IAStats")]
public class IAStats : ScriptableObject
{
    public float capturingTime = 5f;

    public float moveSpeed = 1;
    public float lookRange = 40f;

    public float attackRange = 1f;
    public float attackRate = 1f;
    public float attackForce = 15f;
    public float attackDamage = 50;
}
