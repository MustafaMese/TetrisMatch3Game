using System;
using System.Collections.Generic;
using Runtime.Command;
using Runtime.Component;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Manager
{
    public class FloatingObjectSchemeProducer : MonoBehaviour
    {
        [SerializeField] private List<FloatingObjectScheme> schemes = new();

        private FloatingObjectScheme _prevScheme;

        public FloatingObjectScheme PrevScheme => _prevScheme;
        
        public void Initialize()
        {
            GameManager.Instance.CommandManager.AddCommandListener<ProduceFloatingObjectCommand>(Command_ProduceScheme);
        }

        private void Command_ProduceScheme(ProduceFloatingObjectCommand command)
        {
            var scheme = schemes[Random.Range(0, schemes.Count)];

            _prevScheme = scheme;
            
            GameManager.Instance.CommandManager.InvokeCommand(new FloatingObjectSchemeProducedCommand(scheme.line1, scheme.line2));
        }

        [Serializable]
        public class FloatingObjectScheme
        {
            public string line1;
            public string line2;
        }
    }
}
