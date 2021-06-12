using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public float BulletSpeed;
        public GunType Type;

        public void Fire()
        {
            var instance = Instantiate(ProjectilePrefab);
            instance.GetComponent<IProjectile>().Fire(transform.position, transform.forward, BulletSpeed);
        }
    }
}
