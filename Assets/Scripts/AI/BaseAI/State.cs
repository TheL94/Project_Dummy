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
            if (!isInitialized)
                Init();

            DoActions(_controller);
            CheckTransitions(_controller);
        }

        public void DoActions(AIController _controller)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Act(_controller);
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

        Action[] actions;
        Transition[] transitions;
        bool isInitialized = false;
        void Init()
        {
            actions = new Action[Actions.Length];
            transitions = new Transition[Transitions.Length];

            for (int i = 0; i < Actions.Length; i++)
            {
                actions[i] = Instantiate(Actions[i]);
            }

            for (int i = 0; i < Transitions.Length; i++)
            {
                transitions[i] = new Transition();
                transitions[i].Decision = Instantiate(Transitions[i].Decision);
                transitions[i].NextPosiibleStates = new State[Transitions[i].NextPosiibleStates.Length]; 
                for (int j = 0; j < Transitions[i].NextPosiibleStates.Length; j++)
                {
                    transitions[i].NextPosiibleStates[j] = Instantiate(Transitions[i].NextPosiibleStates[j]);
                }
            }
            isInitialized = true;
        }
    }
}