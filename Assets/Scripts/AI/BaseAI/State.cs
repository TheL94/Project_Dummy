using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewState")]
    public class State : ScriptableObject
    {
        public Color StateColor = Color.gray;
        public Action[] Actions;
        [Tooltip("Per un loop, inserire lo stesso stato come primo della lista di uscita dalla decision.")]
        public Transition[] Transitions;

        public void UpdateState(AIController _controller)
        {
            Debug.Log(this.name);
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
                int index = Transitions[i].Decision.Decide(_controller);
                _controller.TransitionToState(Transitions[i].NextPosiibleStates[index]);
            }
        }
    }
}