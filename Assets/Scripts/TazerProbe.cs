using UnityEngine;

namespace Assets.Scripts
{
    public class TazerProbe : MonoBehaviour, IProjectile
    {
        private Vector3 _origin;
        private Vector3 _direction;
        private float _speed;

        public void Fire(Vector3 position, Vector3 direction, float speed)
        {
            _origin = position;
            transform.position = position;
            _direction = direction.normalized;
            _speed = speed;
        }

        void FixedUpdate()
        {
            var newPos = transform.position + (_direction * _speed * Time.fixedDeltaTime);
            if (Physics.Linecast(transform.position, newPos, out var hitInfo))
            {
                var events = hitInfo.transform.GetComponent<CharacterEventManager>();
                if (events != null)
                {
                    Destroy(gameObject);
                    events.RaiseTazed();
                }
            }

            transform.position = newPos;

            if ((transform.position - _origin).magnitude > 20)
                Destroy(gameObject);
        }
    }
}
