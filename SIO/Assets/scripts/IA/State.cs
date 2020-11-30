using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    public Action[] Actions;
    public Color sceenGizmoColor = Color.grey;
    public void UpdateState(IAController controller)
    {
        DoActions(controller);
    }
    private void DoActions(IAController controller)
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].Act(controller);
        }
    }
}
