using UnityEngine;

namespace Assets.Scripts
{
    public interface IProjectile
    {
        void Fire(Vector3 position, Vector3 direction, float speed);
    }
}