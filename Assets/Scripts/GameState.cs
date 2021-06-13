using System;
using UnityEngine;

namespace Assets.Scripts
{
    public enum GameStates
    {
        OpeningScreen,
        Playing,
        PlayerKilledByZombie,
        PedestrianKilledByZombie
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/GameState")]
    public class GameState : ScriptableObject
    {
        public GameStates State;

        public event EventHandler<GameStateChangedEventArgs> GameStateChanged = delegate { };

        public void Init()
        {
            State = GameStates.OpeningScreen;
        }

        public void StartGame()
        {
            SetState(GameStates.Playing);
        }

        public void SetState(GameStates newState)
        {
            if (newState == State) return;

            State = newState;
            GameStateChanged(this, new GameStateChangedEventArgs(newState));
        }
    }

    public class GameStateChangedEventArgs : EventArgs
    {
        public GameStates NewState { get; private set; }

        public GameStateChangedEventArgs(GameStates newState)
        {
            NewState = newState;
        }
    }
}