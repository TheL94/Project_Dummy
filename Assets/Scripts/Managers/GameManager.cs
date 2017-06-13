using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Grid;
using DumbProject.UI;
using DumbProject.Flow;

namespace DumbProject.Generic
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager I;
        [HideInInspector]
        public FlowManager flowMng;

        public RoomGenerator RoomGenertorPrefab;
        public GridController GridControllerPrefab;
        public UIManager UIManagerPrefab;
        public RoomPreviewController RoomPreviewControllerPrefab;

        [HideInInspector]
        public RoomGenerator RoomGenerator;
        [HideInInspector]
        public GridController MainGridCtrl;
        [HideInInspector]
        public UIManager UIMng;
        [HideInInspector]
        public RoomPreviewController RoomPreviewCtrl;

        private void Awake()
        {
            //Singleton paradigm
            if (I == null)
                I = this;
            else
                DestroyImmediate(gameObject);
        }

        private void Start()
        {
            flowMng = GetComponent<FlowManager>();
            flowMng.CurrentState = FlowState.MenuState;

            MainGridCtrl = Instantiate(GridControllerPrefab);
            UIMng = Instantiate(UIManagerPrefab);
            RoomPreviewCtrl = Instantiate(RoomPreviewControllerPrefab);
            RoomGenerator = Instantiate(RoomGenertorPrefab);
        }
        
        /// <summary>
        /// Crea la griglia chiamando vari setup e impostando lo stato di gameplay
        /// </summary>
        public void EnterGameplayMode()
        {
            MainGridCtrl.Setup();
            RoomPreviewCtrl.Setup();
            RoomGenerator.Setup();
        }

        public void ExitGameplayMode()
        {
            MainGridCtrl.DestroyGrid();
            RoomPreviewCtrl.DestroyUIGrid();
            RoomGenerator.Clean();
        }
    }
}