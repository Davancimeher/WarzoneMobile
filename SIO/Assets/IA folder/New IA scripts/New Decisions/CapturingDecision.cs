using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Decisions/CapturingDecision")]
public class CapturingDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool actualOwner = false;
        if (controller.ownerPV != null)
        {
            actualOwner = true;
            controller.capturingState = CapturingState.Captured;
        }
        return actualOwner;
    }
}
