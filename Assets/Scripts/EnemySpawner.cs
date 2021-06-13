using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform[] Lanes;
        public SpawnPlan SpawnPlan;
        public GameState GameState;
        public GameObject[] Prefabs;

        public int PlanPosition;
        private float _lastSpawn;
        private int[] _shuffledLaneIndices;
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
        private List<GameObject> _lastWave = new List<GameObject>();

        void Start()
        {
            PlanPosition = 0;
            _shuffledLaneIndices = Enumerable.Range(0, Lanes.Length).Select(i => i).ToArray();
            GameState.GameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameStateChangedEventArgs e)
        {
            if (e.NewState == GameStates.Playing)
                StartCoroutine(Spawn());
        }

        void ShuffleLaneIndices()
        {
            for (var i = 0; i < _shuffledLaneIndices.Length / 2; i++)
            {
                var randIndex = _random.Next(i + 1, _shuffledLaneIndices.Length);
                var temp = _shuffledLaneIndices[randIndex];
                _shuffledLaneIndices[randIndex] = _shuffledLaneIndices[i];
                _shuffledLaneIndices[i] = temp;
            }
        }

        IEnumerator Spawn()
        {
            Debug.Log("PlanPosition: " + PlanPosition);
            _lastSpawn = Time.time;

            while (PlanPosition < SpawnPlan.Items.Length)
            {
                if (GameState.State != GameStates.Playing)
                    yield break;

                var planItem = SpawnPlan.Items[PlanPosition];

                var timeSinceLastSpawn = Time.time - _lastSpawn;

                if (timeSinceLastSpawn < planItem.TimestampDelta)
                {
                    yield return null;
                    continue;
                }

                _lastSpawn = Time.time;
                PlanPosition++;
                ShuffleLaneIndices();
                _lastWave.Clear();

                for (var i = 0; i < Math.Min(planItem.Count, Lanes.Length); i++)
                {
                    var laneIndex = _shuffledLaneIndices[i];
                    var lane = Lanes[laneIndex];
                    var prefab = Prefabs[_random.Next(0, Prefabs.Length)];
                    var instance = Instantiate(prefab, lane.transform.position, lane.transform.rotation);
                    _lastWave.Add(instance);
                    instance.GetComponent<MovingCharacter>().Go(lane.transform.forward);
                    yield return new WaitForSeconds((float) Math.Abs(_random.NextDouble() * 0.5));
                }

                yield return null;
            }

            while (true)
            {
                if (GameState.State == GameStates.Playing && _lastWave.All(i => i == null))
                {
                    GameState.SetState(GameStates.LevelCleared);
                    yield break;
                }

                yield return null;
            }
        }
    }
}
