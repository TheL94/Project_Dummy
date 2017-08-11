using System.Collections.Generic;
using System;
using UnityEngine;

namespace Framework.AI
{
    /// <summary>
    /// Called to recive instances of the AI_State. It also manages who need those Datas and eventually create new instances
    /// </summary>
    public static class AI_DataManager
    {
        /// <summary>
        /// All the Controllers that have asked for a State Instance
        /// </summary>
        static List<ControllerData> Controllers = new List<ControllerData>();

        /// <summary>
        /// Retrive a State's instance for a specific AI_Controller
        /// </summary>
        /// <param name="_controller"></param>
        /// <param name="_stateOriginalData"></param>
        /// <returns></returns>
        public static AI_State GetState(AI_Controller _controller, AI_State _stateOriginalData)
        {
            ControllerData controllerData = GetControllerData(_controller);
            AI_State stateInstance = controllerData.GetStateInstance(_stateOriginalData);

            return stateInstance;
        }

        /// <summary>
        /// Check if a ControllerData for a specific AI_Controller is already in the ControllerData list
        /// otherwise it creates a new one
        /// </summary>
        /// <param name="_controller">Controller to  verify</param>
        /// <returns>Relative ControllerData</returns>
        static ControllerData GetControllerData(AI_Controller _controller)
        {
            ControllerData controllerData = null;

            foreach (ControllerData ctrlData in Controllers)
            {
                if (ctrlData.Controller == _controller)
                    controllerData = ctrlData;
            }

            if (controllerData == null)
            {
                controllerData = new ControllerData(_controller);
                Controllers.Add(controllerData);
            }

            return controllerData;
        }

        /// <summary>
        /// Class that hold the instances of states relative to a specific AI_Controller
        /// </summary>
        class ControllerData
        {
            public AI_Controller Controller;
            List<AI_State> stateInstances = new List<AI_State>();

            public ControllerData(AI_Controller controller)
            {
                Controller = controller;
            }
            /// <summary>
            /// Get the instance of the State required. It creates a new one if missing.
            /// </summary>
            /// <param name="_state">Original State</param>
            /// <returns>Instance of the original State</returns>
            public AI_State GetStateInstance(AI_State _state)
            {
                AI_State stateInstance = null;
                foreach (AI_State instance in stateInstances)
                {
                    if (instance.name.StartsWith(_state.name))
                        stateInstance = instance;
                }

                if(stateInstance == null)
                {
                    stateInstance = GameObject.Instantiate(_state);
                    stateInstances.Add(stateInstance);
                }

                return stateInstance;
            }
        }
    }
}
