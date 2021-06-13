using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pedestrian : MonoBehaviour, IHealthOwner
    {
        public Animator Animator;
        public BoxCollider StandCollider;
        public BoxCollider DownCollider;

        private const int AnimShot = 1;
        private const int AnimStandUp = 2;
        private const float KnockDistance = 1f;

        CharacterEventManager _eventManager;
        int _health = 1;
        Vector3 _origin;

        public bool CanReceiveHealth()
        {
            return _health == 0;
        }

        void Start()
        {
            _origin = transform.position;
            StandCollider.enabled = true;
            DownCollider.enabled = false;
            _eventManager = gameObject.AddComponent<CharacterEventManager>();
            _eventManager.Shot += OnShot;
            _eventManager.Tazed += OnTazed;
            _eventManager.PickedUpHealth += OnPickedUpHealth;
        }

        void Update()
        {
            if ((transform.position - _origin).magnitude > 15)
                Destroy(gameObject);
        }

        void OnShot(object sender, EventArgs e)
        {
            if (_health == 1)
            {
                _health = 0;
                transform.position += transform.right * KnockDistance;
                StartCoroutine(DoGetShot());
            }
        }

        private void OnTazed(object sender, EventArgs e)
        {
            transform.position += transform.right * KnockDistance;
        }

        private void OnPickedUpHealth(object sender, EventArgs e)
        {
            _health = 1;

            StartCoroutine(StandUp());
        }

        private IEnumerator DoGetShot()
        {
            Animator.SetInteger("State", AnimShot);
            DownCollider.enabled = true;
            StandCollider.enabled = false;

            yield return new WaitForSeconds(7f);

            if (_health == 0)
                Destroy(gameObject);
        }

        private IEnumerator StandUp()
        {
            Animator.SetInteger("State", AnimStandUp);
            transform.position += transform.forward * 1.1f;
            yield return new WaitForSeconds(2f);
            StandCollider.enabled = true;
            DownCollider.enabled = false;
            _eventManager.RaiseRevived();
        }
    }
}
