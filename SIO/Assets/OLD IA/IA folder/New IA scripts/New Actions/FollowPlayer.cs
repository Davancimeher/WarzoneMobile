using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Actions/FollowPlayer")]
public class FollowPlayer : Action
{
    private Vector3 oldDestination;
    public override void Act(StateController Controller)
    {
        MoveToPosition(Controller);
    }
    public void MoveToPosition(StateController controller)
    {
        controller.animator.SetFloat("input", controller.navMeshAgent.remainingDistance);
        if (oldDestination != controller.NewDestination)
        {
            controller.navMeshAgent.SetDestination(controller.NewDestination);
            oldDestination = controller.NewDestination;
        }
    }
}
