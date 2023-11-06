using System;
using System.Collections;
using Runtime.Command;
using Runtime.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        [SerializeField] private InputManager inputManager;
        [SerializeField] private FloatingManager floatingManager;
        [SerializeField] private FloatingObjectSchemeProducer floatingObjectSchemeProducer;
        [SerializeField] private GridAreaView gridAreaViewPrefab;
        [SerializeField] private EndCanvasController endCanvasControllerPrefab;
        
        private GridAreaView _gridAreaView;
        
        public CommandManager CommandManager;
        public TaskExceptionHandler TaskExceptionHandler;
        public ScoreManager ScoreManager;
        
        private void Awake()
        {
            _instance = this;
            
            CommandManager = new CommandManager();
            TaskExceptionHandler = new TaskExceptionHandler();
            ScoreManager = new ScoreManager();
            
            _gridAreaView = Instantiate(gridAreaViewPrefab);
            _gridAreaView.Initialize();

            inputManager.Initialize(_gridAreaView.GetInputPlane(), _gridAreaView.GetColumnCount());
            floatingManager.Initialize(_gridAreaView);
            GetComponent<GoingDownHandler>().Initialize();
            GetComponent<ShiftingHandler>().Initialize();
            GetComponent<FrozenObjectHandler>().Initialize();
            
            floatingObjectSchemeProducer.Initialize();

            Instantiate(endCanvasControllerPrefab);
            
            StartCoroutine(StartFloating());
        }

        private IEnumerator StartFloating()
        {
            yield return new WaitForSeconds(0.5f);
            
            CommandManager.InvokeCommand(new ProduceFloatingObjectCommand());
        }

        private void OnDestroy()
        {
            TaskExceptionHandler.Cancel();
        }
    }
}
