using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Framework.AI;
using DumbProject.Rooms;
using DumbProject.Grid;
using DumbProject.UI;
using DumbProject.Flow;
using DumbProject.GDR_System;

namespace DumbProject.Generic
{
    [RequireComponent(typeof(FlowManager))]
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public static GameManager I;

        public AI_Controller DumbyPrefab;
        public RoomGenerator RoomGenertorPrefab;
        public GridController GridControllerPrefab;
        public UIManager UIManagerPrefab;
        public RoomPreviewController RoomPreviewControllerPrefab;
        public DungeonManager DungeonManagerPrefab;
        public GDR_Element_Manager GDR_Element_ManagerPrefab;
        public PoolManager PoolManagerPrefab;

        public DeviceType DeviceEnvironment { get { return SystemInfo.deviceType; } }
        public bool IsTouchAvailable { get { return Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer; } }
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

        public CameraHandler CameraHndl;

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
        public GDR_Element_Manager GDR_Element_Mng;
        [HideInInspector]
        public PoolManager PoolMng;
        [HideInInspector]
        public RoomDataManager RoomDataMng;
        [HideInInspector]
        public GDR_ElementDataManger GDR_ElementDataMng;
        [HideInInspector]
        public InputHandler InputHndl;

        [HideInInspector]
        public AI_Controller Dumby;

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
            // TODO : per test 
            if(Input.GetKeyDown(KeyCode.Space))
                Dumby.IsActive = true;
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
            CameraHndl.Init();

            InputHndl = GetComponent<InputHandler>();
            InputHndl.Init(CameraHndl);

            RoomDataMng = GetComponent<RoomDataManager>();
            RoomDataMng.InitData();

            GDR_ElementDataMng = GetComponent<GDR_ElementDataManger>();
            GDR_ElementDataMng.InitData();

            PoolMng = Instantiate(PoolManagerPrefab);
            PoolMng.Init(RoomDataMng.RoomGraphicComponentDatasInst);
            PoolMng.Setup();

            UIMng = Instantiate(UIManagerPrefab);
            UIMng.Init();
            //-----------------------------------------
            InputHndl.SetupLogger();
            //-----------------------------------------

            MainGridCtrl = Instantiate(GridControllerPrefab);

            RoomPreviewCtrl = Instantiate(RoomPreviewControllerPrefab);

            RoomGenerator = Instantiate(RoomGenertorPrefab);
            RoomGenerator.Init(RoomDataMng.MainRoomDataInst, RoomDataMng.ObjectiveRoomDataInst, RoomDataMng.RoomDataInst);

            DungeonMng = Instantiate(DungeonManagerPrefab);

            GDR_Element_Mng = Instantiate(GDR_Element_ManagerPrefab);
            GDR_Element_Mng.Init(GDR_ElementDataMng.Istances_GDR_Element_Data);

            ChageFlowState(FlowState.Menu);
        }

        public void MenuActions()
        {
            UIMng.ActivateMenuPanel(true);
            UIMng.ActivateGamePlayPanel(false);
            UIMng.ActivateEndGameplayPanel(false);
        }

        public void EnterGameplayActions()
        {
            DungeonMng.Setup();
            MainGridCtrl.Setup();
            RoomPreviewCtrl.Setup();
            RoomGenerator.Setup();

            UIMng.ActivateGamePlayPanel(true);
            UIMng.ActivateMenuPanel(false);
            UIMng.GamePlayCtrl.ActivateLateralGUI(true);
            UIMng.GamePlayCtrl.ActivatePauseButton(true);

            Dumby = Instantiate(DumbyPrefab, DungeonMng.RoomInDungeon[0].CellsInRoom[0].RelativeNode.WorldPosition, Quaternion.identity);
            GDR_Controller Dumby_GDR = Dumby.GetComponent<GDR_Controller>();
            if (Dumby_GDR != null)
                Dumby_GDR.Init(GDR_ElementDataMng.GetGDR_DataByID("Dumby"));

            SetAllAIStatus(true);

            ChageFlowState(FlowState.StartGameplay);
        }

        public void StartGameplayActions()
        {
            // TODO : implementare bene questa perte del flow
            InputHndl.enabled = false;
            Camera_CinematicController cameraCinematic = CameraHndl.GetComponent<Camera_CinematicController>();
            cameraCinematic.Init();
            cameraCinematic.Execute(DungeonMng.FirstRoom.transform.position ,DungeonMng.ObjectiveRoom.transform.position);
        }

        public void PauseActions(bool _status)
        {
            SetAllAIStatus(!_status);
            UIMng.GamePlayCtrl.ActivatePausePanel(_status);
        }

        public void RecapGameActions()
        {
            SetAllAIStatus(false);
            UIMng.ActivateEndGameplayPanel(true);
            UIMng.ActivateGamePlayPanel(false);
        }

        /// <summary>
        /// Pulisce gli elementi presenti nel gameplay
        /// </summary>
        public void ExitGameplayActions()
        {
            DungeonMng.ResetGraphics();
            RoomGenerator.ResetGraphics();
            DungeonMng.Clean();
            RoomGenerator.Clean();
            MainGridCtrl.DestroyGrid();
            RoomPreviewCtrl.DestroyUIGrid();
            //------------------
            Destroy(Dumby.gameObject);
            //------------------
            UIMng.GamePlayCtrl.ActivateLateralGUI(false);
            UIMng.GamePlayCtrl.ActivatePausePanel(false);
            CameraHndl.ResetValues();
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

        public void SetAllAIStatus(bool _status)
        {
            Dumby.IsActive = _status;
        }
        #endregion

        // da lasciare, fastidioso per i test
        //private void OnApplicationFocus(bool focus)
        //{
        //    if (!focus)
        //        ChageFlowState(FlowState.Pause);
        //}
    }
}