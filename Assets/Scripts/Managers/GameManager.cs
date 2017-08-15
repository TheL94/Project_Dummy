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

            ChageFlowState(FlowState.Gameplay);
        }

        public void MenuActions()
        {
            UIMng.ActivateMenuPanel(true);
            UIMng.ActivateGamePlayPanel(false);
            UIMng.ActivateCameraPanel(false);
        }

        public void PauseActions(bool _status)
        {
            UIMng.GamePlayCtrl.ActivatePausePanel(_status);
            //if (_status)
            //{
            //    // azioni da fare quando si entra in pausa
            //}
            //else
            //{
            //    // azioni da fare quando si esce dalla pausa
            //}
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
            //UIMng.GamePlayCtrl.GamePlayElements.InventoryController.CleanInventory();
            //------------------
            Destroy(DumbyToTest.gameObject);
            //------------------
            UIMng.GamePlayCtrl.ActivateLateralGUI(false);
            ChageFlowState(FlowState.Menu);
        }

        public void QuitGameActions()
        {
            Application.Quit();
        }
        #endregion
        #endregion
    }
}