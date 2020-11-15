using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Actions/Attack")]

public class AttaqueAction : Action
{
    private float attackRate;
    public override void Act(StateController Controller)
    {
        Attack(Controller);
    }

    private void Attack(StateController controller)
    {

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            if(controller.chaseTarget != null)
            {
                controller.transform.LookAt(controller.chaseTarget);
            }
            if (controller.checkIfCountDownElapased(controller.enemyStats.attaqueRate))
            {
                var RateTime = (int)controller.GetTimeElapased() % controller.enemyStats.attaqueRate;
                Debug.Log(RateTime);
                if (RateTime <= 0)
                {
                    controller.animator.SetTrigger("Attack 01");
                }
            }
        }
    }
  
}
