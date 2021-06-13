using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour, IHealthOwner
    {
        public Transform Body;
        public GameState GameState;

        CharacterEventManager _eventManager;
        int _health = 1;
        BoxCollider _collider;

        public bool CanReceiveHealth()
        {
            return _health == 1;
        }

        void Start()
        {
            _eventManager = gameObject.AddComponent<CharacterEventManager>();
            _eventManager.Shot += OnShot;
            _eventManager.Tazed += OnTazed;
            _eventManager.PickedUpHealth += OnPickedUpHealth;
            _collider = GetComponent<BoxCollider>();
        }

        void Update()
        {
            CheckCollisions();
        }

        void OnShot(object sender, EventArgs e)
        {
            _health = Math.Max(0, _health - 1);

            if (_health == 0)
                StartCoroutine(Die());
        }

        private void OnTazed(object sender, EventArgs e)
        {
            transform.position += transform.right * 0.6f;
        }

        void OnPickedUpHealth(object sender, EventArgs e)
        {
            if (_health == 1)
                _health = 2;
        }

        private IEnumerator Die()
        {
            var alpha = 1f;
            var material = Body.GetComponent<MeshRenderer>().material;

            while (alpha > 0)
            {
                material.color = new Color(0, 1, 0, alpha);
                alpha -= 1f * Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }

        void CheckCollisions()
        {
            var hits = Physics.OverlapBox(transform.position + _collider.center, _collider.size * 0.5f,
                transform.rotation);
            foreach (var hit in hits)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    GameState.SetState(GameStates.PlayerKilledByZombie);
                    break;
                }

                if (hit.GetComponent<Road>() != null)
                {
                    GameState.SetState(GameStates.PedestrianKilledByZombie);
                    break;
                }
            }
        }
    }
}
