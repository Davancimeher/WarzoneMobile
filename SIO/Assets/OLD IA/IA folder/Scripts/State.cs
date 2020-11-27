using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableIA/State", order = 1)]

public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;


    public Color sceneGizmoColor = Color.green;
    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        checkTransition(controller);
    }
    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }
    private void checkTransition(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
