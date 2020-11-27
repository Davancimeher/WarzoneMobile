using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgressiveBehvaiour : ScriptableObject
{
    public abstract void AttackAction(IAController controller);
}
