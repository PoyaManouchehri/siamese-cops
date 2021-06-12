using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pedestrian : MonoBehaviour
    {
        public Transform Body;

        CharacterEventManager _eventManager;

        void Start()
        {
            _eventManager = gameObject.AddComponent<CharacterEventManager>();
            _eventManager.Shot += OnShot;
        }

        void Update()
        {
        }

        void OnShot(object sender, EventArgs e)
        {
            StartCoroutine(Die());
        }

        private IEnumerator Die()
        {
            var alpha = 1f;
            var material = Body.GetComponent<MeshRenderer>().material;

            while (alpha > 0)
            {
                material.color = new Color(1, 0, 0, alpha);
                alpha -= 1f * Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
