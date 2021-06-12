using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealthPack : MonoBehaviour, IProjectile
    {
        public float Gravity = 3;
        public float EffectiveDuration = 3;

        private Vector3 _origin;
        private Vector3 _direction;
        private float _speed;
        private bool _isLaunching;
        private float _landTime;

        public void Fire(Vector3 position, Vector3 direction, float speed)
        {
            _origin = position;
            transform.position = position;
            _direction = direction.normalized + Vector3.up * 2;
            _speed = speed;
            _isLaunching = true;
        }

        void FixedUpdate()
        {
            if (_isLaunching)
            {
                var newPos = transform.position + (_direction * _speed * Time.fixedDeltaTime);
                if (newPos.y <= 0)
                {
                    newPos = new Vector3(newPos.x, 0, newPos.z);
                    _isLaunching = false;
                    _landTime = Time.time;
                }

                transform.position = newPos;
                _direction -= Vector3.up * Gravity * Time.fixedDeltaTime;
                return;
            }

            Debug.DrawLine(transform.position, transform.position + Vector3.up * 0.3f, Color.red);
            Debug.DrawLine(transform.position, transform.position + Vector3.forward * 0.3f, Color.red);

            var hits = Physics.OverlapSphere(transform.position, 0.3f);
            if (hits.Any())
            {
                foreach (var hit in hits)
                {
                    var events = hit.transform.GetComponent<CharacterEventManager>();
                    var healthOwner = hit.transform.GetComponent<IHealthOwner>();
                    if (healthOwner != null && events != null)
                    {
                        if (healthOwner.CanReceiveHealth())
                        {
                            events.RaisedPickedUpHealth();
                            Destroy(gameObject);
                            break;
                        }
                    }
                }
            }

            if (Time.time - _landTime > EffectiveDuration)
                Destroy(gameObject);
        }
    }
}
