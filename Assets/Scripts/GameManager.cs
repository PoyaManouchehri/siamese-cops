using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameState GameState;

        void Start()
        {
            GameState.Init();
        }
    }
}