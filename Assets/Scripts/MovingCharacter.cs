using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingCharacter : MonoBehaviour
    {
        public Vector3 Direction;
        public float Speed;
        public Vector3 TazeKnock;
        public float TazeRecoveryDuration;

        CharacterEventManager _eventManager;
        private bool _isShot;
        private bool _isTazed;

        public void Go(Vector3 direction, float speed, Vector3 tazeKnock, float tazeRecoveryDuration)
        {
            Direction = direction;
            Speed = speed;
            TazeKnock = tazeKnock;
            TazeRecoveryDuration = tazeRecoveryDuration;
        }

        void Start()
        {
            _eventManager = gameObject.GetComponent<CharacterEventManager>();
            _eventManager.Shot += OnShot;
            _eventManager.Tazed += OnTazed;
        }

        void Update()
        {
            if (_isShot || _isTazed) return;

            transform.position += Direction * Speed * Time.deltaTime;
        }

        void OnShot(object sender, EventArgs e)
        {
            _isShot = true;
            GetComponent<BoxCollider>().enabled = false;
        }

        void OnTazed(object sender, EventArgs e)
        {
            StartCoroutine(Taze());
        }

        private IEnumerator Taze()
        {
            _isTazed = true;
            transform.position += TazeKnock;
            yield return new WaitForSeconds(TazeRecoveryDuration);
            _isTazed = false;
        }
    }
}
