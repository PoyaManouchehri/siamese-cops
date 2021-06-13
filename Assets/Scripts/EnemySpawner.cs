using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform[] Lanes;
        public SpawnPlan SpawnPlan;
        public GameState GameState;

        public int PlanPosition;
        private float _lastSpawn;
        private int[] _shuffledLaneIndices;
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        void Start()
        {
            PlanPosition = 0;
            _shuffledLaneIndices = Enumerable.Range(0, Lanes.Length).Select(i => i).ToArray();
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

                for (var i = 0; i < Math.Min(planItem.Count, Lanes.Length); i++)
                {
                    var laneIndex = _shuffledLaneIndices[i];
                    var lane = Lanes[laneIndex];
                    var instance = Instantiate(planItem.Prefab, lane.transform.position, lane.transform.rotation);
                    instance.GetComponent<MovingCharacter>().Go(lane.transform.forward);
                }

                yield return null;
            }
        }
    }
}
