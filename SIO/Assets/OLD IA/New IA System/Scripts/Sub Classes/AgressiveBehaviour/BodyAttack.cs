using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/AgressiveBehvaiour/BodyAttack")]
public class BodyAttack : AgressiveBehvaiour
{
    private float stateTime = 0f;
    public override void AttackAction(IAController controller)
    {
        Attack(controller);
    }
    private void Attack(IAController controller)
    {
        if (controller.navMeshAgent.remainingDistance <= controller._EnemyStats.attackRange && !controller.navMeshAgent.pathPending)
        {
            if (controller.EnemyCrew != null)
            {
                GameObject enemy = controller.FindClosestTargetInCrew();
                controller.transform.LookAt(enemy.transform);
            }
            controller.animator.SetTrigger("Attack 01");
        }
    }
}
