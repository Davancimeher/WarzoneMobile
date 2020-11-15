using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/Decisions/PassiveDecision")]
public class PassiveDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool OwnerIsActive = controller.Owner.gameObject.activeSelf;
        return OwnerIsActive;
    }
}
