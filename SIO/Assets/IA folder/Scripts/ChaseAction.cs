using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Actions/Chase")]

public class ChaseAction : Action
{
    public override void Act(StateController Controller)
    {
        Chase(Controller);
    }
    private void Chase(StateController controller)
    {
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.stoppingDistance = controller.enemyStats.attackRange;
        controller.animator.SetFloat("input", controller.navMeshAgent.remainingDistance-controller.navMeshAgent.stoppingDistance);
    }

}
