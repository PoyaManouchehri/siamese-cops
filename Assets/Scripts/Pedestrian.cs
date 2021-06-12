using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pedestrian : MonoBehaviour, IHealthOwner
    {
        public Transform Body;

        CharacterEventManager _eventManager;
        int _health = 1;

        public bool CanReceiveHealth()
        {
            return _health == 0;
        }

        void Start()
        {
            _eventManager = gameObject.AddComponent<CharacterEventManager>();
            _eventManager.Shot += OnShot;
            _eventManager.Tazed += OnTazed;
            _eventManager.PickedUpHealth += OnPickedUpHealth;
        }

        void OnShot(object sender, EventArgs e)
        {
            if (_health == 1)
            {
                _health = 0;
                transform.position += transform.right * 0.6f;
                StartCoroutine(Die());
            }
        }

        private void OnTazed(object sender, EventArgs e)
        {
            transform.position += transform.right * 0.6f;
        }

        private void OnPickedUpHealth(object sender, EventArgs e)
        {
            _health = 1;
            _eventManager.RaiseRevived();
        }

        private IEnumerator Die()
        {
            var alpha = 1f;
            var material = Body.GetComponent<MeshRenderer>().material;

            while (alpha > 0)
            {
                material.color = new Color(1, 0, 0, alpha);
                alpha -= 0.2f * Time.deltaTime;

                if (_health > 0)
                {
                    material.color = new Color(1, 1, 1, 1);
                    yield break;
                }

                yield return null;
            }

            if (_health == 0)
                Destroy(gameObject);
        }
    }
}
