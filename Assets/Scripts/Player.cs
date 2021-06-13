using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public Gun[] Guns;
        public float Speed;
        public GameState GameState;

        private Gun[] _activeGuns;

        void SetActiveGun(GunType type)
        {
            _activeGuns = Guns
                .Where(g => g.Type == type)
                .ToArray();
        }

        void Start()
        {
            SetActiveGun(GunType.Pistol);
        }

        // Update is called once per frame
        void Update()
        {
            if (GameState.State != GameStates.Playing)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var gun in _activeGuns)
                    gun.Fire();
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.left * Speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                transform.Translate(-Vector3.left * Speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Alpha1))
                SetActiveGun(GunType.Pistol);
            if (Input.GetKey(KeyCode.Alpha2))
                SetActiveGun(GunType.Tazer);
            if (Input.GetKey(KeyCode.Alpha3))
                SetActiveGun(GunType.HealthPackLauncher);
        }
    }
}
