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

        bool IsGamePlaying;

        private bool _isGamePaused;

        public bool IsGamePaused
        {
            get { return _isGamePaused; }
            set { _isGamePaused = value; }
        }


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
            MainGridCtrl = Instantiate(GridControllerPrefab);
            UIMng = Instantiate(UIManagerPrefab);
            RoomPreviewCtrl = Instantiate(RoomPreviewControllerPrefab);
            RoomGenerator = Instantiate(RoomGenertorPrefab);

            flowMng.CurrentState = FlowState.Loading;
        }
        
        public void Init()
        {
            UIMng.Init();
            flowMng.CurrentState = FlowState.MenuState;
        }

        /// <summary>
        /// Crea la griglia chiamando vari setup e impostando lo stato di gameplay
        /// </summary>
        public void EnterGameplayMode()
        {
            if (!IsGamePlaying)
            {
                MainGridCtrl.Setup();
                RoomPreviewCtrl.Setup();
                RoomGenerator.Setup();
                IsGamePlaying = true;
            }

        }

        /// <summary>
        /// Pulisce gli elementi presenti nel gameplay
        /// </summary>
        public void ExitGameplayMode()
        {
            MainGridCtrl.DestroyGrid();
            RoomPreviewCtrl.DestroyUIGrid();
            RoomGenerator.Clean();
            IsGamePlaying = false;
        }

        /// <summary>
        /// Richiamata dal bottone di Resume nel pause Panel per tornare nello stato di gameplay
        /// </summary>
        public void DeactivatePauseMode()
        {
            flowMng.CurrentState = DumbProject.Flow.FlowState.GameplayState;
        }

        /// <summary>
        /// Attiva il pannello della pausa
        /// </summary>
        public void ActivePauseMode()
        {
            UIMng.GamePlayCtrl.ActivatePause();
        }

    }
}