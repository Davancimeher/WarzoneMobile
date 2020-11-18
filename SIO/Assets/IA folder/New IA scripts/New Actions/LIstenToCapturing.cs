using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Actions/ListenToCapturing")]

public class LIstenToCapturing : Action
{
    
    public override void Act(StateController Controller)
    {
        CheckActualCapturingTime(Controller);
    }

    private void CheckColliders(StateController controller)
    {
        //send states
        switch (controller.capturingState)
        {
            case CapturingState.OnCapturing:

                controller.actualCapturingTime -= Time.deltaTime;
                //Update time on server
                break;
        }
    }
    private void CheckActualCapturingTime(StateController controller)
    {
        if(controller.actualCapturingTime > 0)
        {
            //check colliders
            CheckColliders(controller);
        }
        else
        {
            controller.SetNewOwner();
        }
    }

}
