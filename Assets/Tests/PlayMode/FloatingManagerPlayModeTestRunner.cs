using System.Collections;
using System.Text;
using NUnit.Framework;
using Runtime.Command;
using Runtime.Component;
using Runtime.Manager;
using Runtime.View;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class FloatingManagerPlayModeTestRunner
    {
        [Test]
        public void IsProducingWorksTrue()
        {
            var gameManager = CreateGameManager();

            var scheme = gameManager.GetComponent<FloatingObjectSchemeProducer>().PrevScheme;

            Assert.True(gameManager.GetComponent<FloatingManager>().TestScheme(scheme));
        }

        
        [Test]
        public void PassSchemeUsingGridArea()
        {
            var gameManager = CreateGameManager();

            var scheme = gameManager.GetComponent<FloatingObjectSchemeProducer>().PrevScheme;

            StringBuilder str = new StringBuilder();

            str.Append(
                $"{scheme.line1} : {gameManager.GetComponent<FloatingManager>().GetFloatingObjectsPositionAtStart(0)} / ");
            str.AppendLine(
                $"{scheme.line2} : {gameManager.GetComponent<FloatingManager>().GetFloatingObjectsPositionAtStart(1)}");
            
            Assert.Pass(str.ToString());
            
        }

        [Test]
        public void CheckSorting()
        {
            var gameManager = CreateGameManager();

            var floatingManager = gameManager.GetComponent<FloatingManager>();
            
            StringBuilder builder = new StringBuilder();
            
            Utils.Sort(Direction.Right, floatingManager.ActiveFloatingObjects);
            
            builder.AppendLine("Right");
            foreach (var t in floatingManager.ActiveFloatingObjects)
            {
                builder.AppendLine(t.GetCoordinate().ToString());
            }
            
            Utils.Sort(Direction.Left, floatingManager.ActiveFloatingObjects);
            
            builder.AppendLine("Left");
            foreach (var t in floatingManager.ActiveFloatingObjects)
            {
                builder.AppendLine(t.GetCoordinate().ToString());
            }
            
            Assert.Pass(builder.ToString());
        }
        
        private GameManager CreateGameManager()
        {
            var gameManager = Object.Instantiate(Resources.Load<GameManager>("Prefabs/Game Manager Variant"));

            GameManager.Instance.CommandManager.InvokeCommand(new ProduceFloatingObjectCommand());
            return gameManager;
        }

        
    }
}
