using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State")]
    public class State : ScriptableObject
    {
        public Color StateColor = Color.gray;
        public Action[] Actions;
        public Transition[] Transitions;

        public void UpdateState(AIController _controller)
        {
            DoActions(_controller);
            CheckTransitions(_controller);
        }

        public void DoActions(AIController _controller)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].Act(_controller);
            }
        }

        public void CheckTransitions(AIController _controller)
        {
            for (int i = 0; i < Transitions.Length; i++)
            {
                // seleziona come stato successivo quello con l'indice pari al risultato di Decision.Decide().
                _controller.TransitionToState(Transitions[i].NextPosiibleStates[Transitions[i].Decision.Decide(_controller)]);
            }
        }
    }
}