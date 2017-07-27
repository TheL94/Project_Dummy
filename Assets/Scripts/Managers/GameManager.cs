using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public Dumby DumbyToTest;
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
        bool IsGamePlaying;

        private bool _isGamePaused;

        public bool IsGamePaused
        {
            get { return _isGamePaused; }
            set { _isGamePaused = value; }
        }

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
                DumbyToTest = Instantiate<Dumby>(DumbyToTest, MainGridCtrl.GetGridCenter().WorldPosition, Quaternion.identity);
                DumbyToTest.Setup();
            }
        }

        #region API
        public void Init()
        {
            PoolMng = Instantiate(PoolManagerPrefab);
            PoolMng.Setup();
            UIMng = Instantiate(UIManagerPrefab);
            UIMng.Init();
            MainGridCtrl = Instantiate(GridControllerPrefab);
            RoomPreviewCtrl = Instantiate(RoomPreviewControllerPrefab);
            RoomGenerator = Instantiate(RoomGenertorPrefab);
            ItemManager = Instantiate(ItemManagerPrefab);
            ItemManager.Init();
            ChageFlowState(FlowState.MenuState);
        }

        #region ChangeStateCalls
        /// <summary>
        /// Funzione che innesca il cambio di stato
        /// </summary>
        public void ChageFlowState(FlowState _stateToSet)
        {
            FlowMng.ChageState(_stateToSet);
        }

        /// <summary>
        /// Crea la griglia chiamando vari setup e impostando lo stato di gameplay
        /// </summary>
        public void EnterGameplayMode()
        {
            if (!IsGamePlaying)
            {
                if (DungeonMng == null)
                    DungeonMng = Instantiate(DungeonManagerPrefab);
                DungeonMng.Setup();
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
            //PoolMng.ForceAllPoolsReset();
            DungeonMng.Clean();
            RoomGenerator.Clean();
            MainGridCtrl.DestroyGrid();
            RoomPreviewCtrl.DestroyUIGrid();
            //UIMng.GamePlayCtrl.GamePlayElements.InventoryController.CleanInventory();
            IsGamePlaying = false;
            ChageFlowState(FlowState.MenuState);
        }
        #endregion
        #endregion
    }
}