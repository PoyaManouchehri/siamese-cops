using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public enum GameStates
    {
        OpeningScreen,
        Playing,
        PlayerKilledByZombie,
        PedestrianKilledByZombie,
        LevelCleared
    }

    public enum Gender
    {
        Male,
        Female
    }

    public struct Death
    {
        public int Age;
        public Gender Gender;
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/GameState")]
    public class GameState : ScriptableObject
    {
        public GameStates State;
        public List<Death> Deaths;

        public event EventHandler<GameStateChangedEventArgs> GameStateChanged = delegate { };

        public void Init()
        {
            State = GameStates.OpeningScreen;
            Deaths = new List<Death>();
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

        public void RecordDeath(Death death)
        {
            Deaths.Add(death);
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