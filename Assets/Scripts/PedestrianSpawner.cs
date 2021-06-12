using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class PedestrianSpawner : MonoBehaviour
    {
        public GameObject CharacterPrefab;
        public float[] PauseDurations;

        readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        void Start()
        {
            StartCoroutine(Spawn());
        }

        IEnumerator Spawn()
        {
            while (true)
            {
                var pause = PauseDurations[_random.Next(0, PauseDurations.Length)];
                yield return new WaitForSeconds(pause);

                var instance = Instantiate(CharacterPrefab, transform.position, transform.rotation);
                instance.GetComponent<MovingCharacter>().Go(transform.forward);
            }
        }
    }
}
