using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public Gun[] Guns;
        public float Speed;
        public GameState GameState;
        public Animator AnimatorTwin1;
        public Animator AnimatorTwin2;

        private const int AnimIdle = 0;
        private const int AnimStrafeRight = 1;
        private const int AnimStrafeLeft = 2;

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
            {
                transform.Translate(Vector3.left * Speed * Time.deltaTime);
                SetAnimationState(AnimStrafeLeft);
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                transform.Translate(-Vector3.left * Speed * Time.deltaTime);
                SetAnimationState(AnimStrafeRight);
            }
            else
            {
                SetAnimationState(AnimIdle);
            }

            if (Input.GetKey(KeyCode.Alpha1))
                SetActiveGun(GunType.Pistol);
            if (Input.GetKey(KeyCode.Alpha2))
                SetActiveGun(GunType.Tazer);
            if (Input.GetKey(KeyCode.Alpha3))
                SetActiveGun(GunType.HealthPackLauncher);
        }

        private void SetAnimationState(int state)
        {
            var currState = AnimatorTwin1.GetInteger("State");
            if (currState == state) return;

            AnimatorTwin1.SetInteger("State", state);
            AnimatorTwin2.SetInteger("State", state);
        }
    }
}
