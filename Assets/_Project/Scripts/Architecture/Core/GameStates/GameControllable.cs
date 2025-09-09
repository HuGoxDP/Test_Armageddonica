using System;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.GameStates
{
    public abstract class GameControllable : MonoBehaviour, IDisposable
    {
        protected MatchController MatchController { get; private set; }
        public void SetMatchController(MatchController matchController)
        {
            MatchController = matchController;
            MatchController.OnGameStateChanged += OnGameStateChanged;
        }
        public void Dispose()
        {
            MatchController.OnGameStateChanged -= OnGameStateChanged;
        }
        
        protected virtual void OnDestroy()
        {
            Dispose();
        }

        protected abstract void OnGameStateChanged(object sender, GameState newState);
    }
}