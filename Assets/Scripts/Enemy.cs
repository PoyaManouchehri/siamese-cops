using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour, IHealthOwner
    {
        public Transform Body;
        public GameState GameState;
        public Animator Animator;

        CharacterEventManager _eventManager;
        int _health = 1;
        BoxCollider _collider;

        public bool CanReceiveHealth()
        {
            return _health == 1;
        }

        public float HealthSpeedMultiplier()
        {
            return 2f;
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
            if (_health > 0)
            {
                _health--;

                if (_health == 0)
                {
                    _eventManager.RaiseFatallyShot();
                    StartCoroutine(Die());
                }
            }
        }

        private void OnTazed(object sender, EventArgs e)
        {
            transform.position += transform.right * 0.6f;
        }

        void OnPickedUpHealth(object sender, EventArgs e)
        {
            if (_health == 1)
                _health = 2;

            StartCoroutine(PowerUp());
        }

        private IEnumerator Die()
        {
            _collider.enabled = false;
            Animator.SetTrigger("Death");
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        private IEnumerator PowerUp()
        {
            Animator.SetTrigger("PowerUp");
            Animator.SetFloat("WalkSpeed", HealthSpeedMultiplier());
            yield return new WaitForSeconds(2f);
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
