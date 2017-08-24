using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using DumbProject.Rooms;
using DumbProject.Grid;
using DumbProject.UI;
using DumbProject.Flow;
using DumbProject.Items;

namespace DumbProject.Generic
{
    [RequireComponent(typeof(FlowManager))]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public static GameManager I;

        public AI_Controller DumbyToTest;
        public RoomGenerator RoomGenertorPrefab;
        public GridController GridControllerPrefab;
        public UIManager UIManagerPrefab;
        public RoomPreviewController RoomPreviewControllerPrefab;
        public DungeonManager DungeonManagerPrefab;
        public ItemsManager ItemManagerPrefab;
        public PoolManager PoolManagerPrefab;

        public DeviceType DeviceEnvironment { get { return SystemInfo.deviceType; } }
        public FlowState CurrentState { get { return FlowMng.CurrentState; } }
        public bool IsGamePaused {
            get
            {
                if (CurrentState == FlowState.Pause)
                    return true;
                else
                    return false;
            }
        }

        [HideInInspector]
        public RoomGenerator RoomGenerator;
        [HideInInspector]
        public GridController MainGridCtrl;
        [HideInInspector]
        public UIManager UIMng;
        [HideInInspector]
        public RoomPreviewController RoomPreviewCtrl;
        [HideInInspector]
        public DungeonManager DungeonMng;
        [HideInInspector]
        public ItemsManager ItemManager;
        [HideInInspector]
        public PoolManager PoolMng;

        FlowManager FlowMng;

        void Awake()
        {
            //Singleton paradigm
            if (I == null)
                I = this;
            else
                DestroyImmediate(gameObject);
        }

        void Start()
        {
            FlowMng = GetComponent<FlowManager>();
            ChageFlowState(FlowState.Loading);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                DumbyToTest = Instantiate<AI_Controller>(DumbyToTest, MainGridCtrl.GetGridCenter().WorldPosition, Quaternion.identity);
            }

            // per test
            // --------------------------------------
            if(CurrentState == FlowState.Gameplay)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    GameWon();
                if (Input.GetKeyDown(KeyCode.L))
                    GameLost();
            }
            // --------------------------------------

            if (Input.GetKeyDown(KeyCode.Escape))
                ChageFlowState(FlowState.Pause);
        }

        #region API
        #region Game Flow
        /// <summary>
        /// Funzione che innesca il cambio di stato
        /// </summary>
        public void ChageFlowState(FlowState _stateToSet)
        {
            FlowMng.ChageState(_stateToSet);
        }

        public void LoadingActions()
        {
            PoolMng = Instantiate(PoolManagerPrefab);
            PoolMng.Setup();
            UIMng = Instantiate(UIManagerPrefab);
            UIMng.Init();
            MainGridCtrl = Instantiate(GridControllerPrefab);
            RoomPreviewCtrl = Instantiate(RoomPreviewControllerPrefab);
            RoomGenerator = Instantiate(RoomGenertorPrefab);
            DungeonMng = Instantiate(DungeonManagerPrefab);
            ItemManager = Instantiate(ItemManagerPrefab);
            ItemManager.Init();
            ChageFlowState(FlowState.Menu);
        }

        public void MenuActions()
        {
            UIMng.ActivateMenuPanel(true);
            UIMng.ActivateGamePlayPanel(false);
            UIMng.ActivateEndGameplayPanel(false);
            UIMng.ActivateCameraPanel(false);
        }

        /// <summary>
        /// Crea la griglia chiamando vari setup e impostando lo stato di gameplay
        /// </summary>
        public void EnterGameplayActions()
        {
            DungeonMng.Setup();
            MainGridCtrl.Setup();
            RoomPreviewCtrl.Setup();
            RoomGenerator.Setup();

            UIMng.ActivateGamePlayPanel(true);
            UIMng.ActivateMenuPanel(false);
            UIMng.ActivateCameraPanel(true);
            UIMng.GamePlayCtrl.ActivateLateralGUI(true);
            UIMng.GamePlayCtrl.ActivatePauseButton(true);

            ChageFlowState(FlowState.Gameplay);
        }

        public void PauseActions(bool _status)
        {
            UIMng.GamePlayCtrl.ActivatePausePanel(_status);
        }

        public void RecapGameActions()
        {
            UIMng.ActivateEndGameplayPanel(true);
            UIMng.ActivateGamePlayPanel(false);
        }

        /// <summary>
        /// Pulisce gli elementi presenti nel gameplay
        /// </summary>
        public void ExitGameplayActions()
        {
            DungeonMng.Clean();
            RoomGenerator.Clean();
            MainGridCtrl.DestroyGrid();
            RoomPreviewCtrl.DestroyUIGrid();
            UIMng.GamePlayCtrl.LateralGUI.InventoryController.CleanInventory();
            //------------------
            Destroy(DumbyToTest.gameObject);
            //------------------
            UIMng.GamePlayCtrl.ActivateLateralGUI(false);
            UIMng.GamePlayCtrl.ActivatePausePanel(false);
            ChageFlowState(FlowState.Menu);
        }

        public void QuitGameActions()
        {
            Application.Quit();
        }

        public void GameWon()
        {
            ChageFlowState(FlowState.RecapGame);
            UIMng.EndGameCtrl.SetRecapText("Victory");
        }

        public void GameLost()
        {
            ChageFlowState(FlowState.RecapGame);
            UIMng.EndGameCtrl.SetRecapText("Defeat");
        }
        #endregion
        #endregion
    }
}