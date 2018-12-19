using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "FSM/State")]
public class State : ScriptableObject {

    public Color sceneGizmoColor = Color.grey;
    public Action[] actions;
    public Transition[] transitions;
    public void UpdateState(CreepFSM controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(CreepFSM controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    //In this state, we will check if we have to change state based on some decisions. 
    //Depending on the outcome of the decision, we move to one state or another. 
    //If the state we move is the same state we are, the FSM will not change.
    private void CheckTransitions(CreepFSM controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.ChangeState(transitions[i].trueState);
            }
            else
            {
                controller.ChangeState(transitions[i].falseState);
            }
        }
    }
}
