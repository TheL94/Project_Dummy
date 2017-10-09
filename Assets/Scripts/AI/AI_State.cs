using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewState")]
    public class AI_State : ScriptableObject
    {
        public List<ActionStructureForState> Actions = new List<ActionStructureForState>();

        protected bool isToSetUp = true;

        /// <summary>
        /// Initialize the state with starting settings
        /// </summary>
        public void Init()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Init(isToSetUp);
            }
            isToSetUp = false;
        }

        /// <summary>
        /// Clean Actions instances in order to reset
        /// </summary>
        public void Clean()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Clean();
            }
        }
        /// <summary>
        /// Executions of all the action, menaging the loops
        /// </summary>
        /// <param name="_controller"></param>
        public void Run(AI_Controller _controller)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Run(_controller);
            }
        }
    }
}
