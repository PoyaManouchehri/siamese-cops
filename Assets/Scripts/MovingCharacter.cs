using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingCharacter : MonoBehaviour
    {
        public Vector3 Direction;
        public float Speed;
        public float TazeRecoveryDuration;
        public float HealthPickupDuration;
        public GameState GameState;

        CharacterEventManager _eventManager;
        private bool _isPaused;
        private bool _isStopped;

        public void Go(Vector3 direction)
        {
            Direction = direction;
        }

        void Start()
        {
            _eventManager = gameObject.GetComponent<CharacterEventManager>();
            _eventManager.FatallyShot += OnFatallyShot;
            _eventManager.Tazed += OnTazed;
            _eventManager.Revived += OnRevived;
            _eventManager.PickedUpHealth += OnPickedUpHealth;
        }

        void Update()
        {
            if (_isStopped || _isPaused || GameState.State != GameStates.Playing) return;

            transform.position += Direction * Speed * Time.deltaTime;
        }

        void OnFatallyShot(object sender, EventArgs e)
        {
            _isPaused = true;
            _isStopped = true;
        }

        void OnTazed(object sender, EventArgs e)
        {
            StartCoroutine(Taze());
        }

        void OnRevived(object sender, EventArgs e)
        {
            _isPaused = false;
            _isStopped = false;
        }

        private void OnPickedUpHealth(object sender, PickedUpHealthEventArgs e)
        {
            StartCoroutine(PickUpHealth(e.SpeedMultiplier));
        }

        private IEnumerator Taze()
        {
            _isPaused = true;
            yield return new WaitForSeconds(TazeRecoveryDuration);
            _isPaused = false;
        }

        private IEnumerator PickUpHealth(float speedMultiplier)
        {
            _isPaused = true;
            Speed *= speedMultiplier;
            yield return new WaitForSeconds(HealthPickupDuration);
            _isPaused = false;
        }
    }
}
