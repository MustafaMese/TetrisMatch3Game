using System;
using Runtime.Command;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Runtime.Manager
{
    public class FailEndUnityEvent : UnityEvent {}
    
    public class EndCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject failUI;
        
        private readonly FailEndUnityEvent _failEvent = new();
        
        private void Awake()
        {
            GameManager.Instance.CommandManager.AddCommandListener<HandleEndGameCommand>(HandleEndGame);
            
            _failEvent.AddListener(Fail);
        }

        private void Fail()
        {
            failUI.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void HandleEndGame(HandleEndGameCommand command)
        {
            _failEvent.Invoke();
                
        }
    }
}
