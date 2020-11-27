using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableIA/Decisions/ActiveState")]
public class ActiveStateDecision : Decision{
    public override bool Decide(StateController controller)
    {
        bool chaseTragetIsActive = controller.chaseTarget.gameObject.activeSelf;
        return chaseTragetIsActive;
    }
}
