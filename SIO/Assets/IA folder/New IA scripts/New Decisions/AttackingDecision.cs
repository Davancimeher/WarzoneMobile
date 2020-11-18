using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Decisions/AttackingDecision")]

public class AttackingDecision : Decision
{
    GameObject target=null;
    public override bool Decide(StateController controller)
    {
        target = controller.FindClosestTarget();
        bool findEnemy = false;
        if (target != null)
        {
            controller.chaseTarget = target.transform;
            findEnemy = true;
        } 
        return findEnemy;
    }
}
