using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class PedestrianSpawner : MonoBehaviour
    {
        public GameObject[] CharacterPrefabs;
        public float[] PauseDurations;
        public GameState GameState;

        readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        void Start()
        {
            GameState.GameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameStateChangedEventArgs e)
        {
            if (e.NewState == GameStates.Playing)
                StartCoroutine(Spawn());
        }

        IEnumerator Spawn()
        {
            while (true)
            {
                if (GameState.State != GameStates.Playing)
                    yield break;

                var pause = PauseDurations[_random.Next(0, PauseDurations.Length)];
                yield return new WaitForSeconds(pause + (float)Math.Abs(_random.NextDouble()) * 0.5f);

                var prefab = CharacterPrefabs[_random.Next(0, CharacterPrefabs.Length)];
                var instance = Instantiate(prefab, transform.position, transform.rotation);
                instance.GetComponent<MovingCharacter>().Go(transform.forward);
            }
        }
    }
}
