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

        CharacterEventManager _eventManager;
        private bool _isPaused;

        public void Go(Vector3 direction)
        {
            Direction = direction;
        }

        void Start()
        {
            _eventManager = gameObject.GetComponent<CharacterEventManager>();
            _eventManager.Shot += OnShot;
            _eventManager.Tazed += OnTazed;
            _eventManager.Revived += OnRevived;
        }

        void Update()
        {
            if (_isPaused) return;

            transform.position += Direction * Speed * Time.deltaTime;
        }

        void OnShot(object sender, EventArgs e)
        {
            _isPaused = true;
        }

        void OnTazed(object sender, EventArgs e)
        {
            StartCoroutine(Taze());
        }

        void OnRevived(object sender, EventArgs e)
        {
            _isPaused = false;
        }

        private IEnumerator Taze()
        {
            _isPaused = true;
            yield return new WaitForSeconds(TazeRecoveryDuration);
            _isPaused = false;
        }
    }
}
