using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class DeathUi : MonoBehaviour
    {
        public GameState GameState;
        public Camera UiCamera;
        public CanvasRenderer MainPanel;
        public TextMeshProUGUI DeathNote;

        void Start()
        {
            GameState.GameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameStateChangedEventArgs e)
        {
            if (e.NewState == GameStates.PlayerKilledByZombie)
                StartCoroutine(FadeIn("You fought the good fight on that day. Australia will not forget its heroes..."));
            else if (e.NewState == GameStates.PedestrianKilledByZombie)
                StartCoroutine(FadeIn("The battled ended, but the war had just begun. You are reminded that the loss of life can't be measured in numbers..."));
        }

        private IEnumerator FadeIn(string deathNote)
        {
            UiCamera.enabled = true;
            DeathNote.text = deathNote;
            DeathNote.enabled = false;

            float alpha = 0;

            yield return FadeIn(MainPanel, Color.black, 1f);
            yield return new WaitForSeconds(2f);

            DeathNote.enabled = true;
            yield return FadeIn(DeathNote.GetComponent<CanvasRenderer>(), Color.white, 5f);
        }

        private IEnumerator FadeIn(CanvasRenderer canvasRenderer, Color color, float speed)
        {
            float alpha = 0;

            while (alpha < 1f)
            {
                canvasRenderer.SetColor(new Color(color.r, color.g, color.b, alpha));
                alpha += speed * Time.deltaTime;
                yield return null;
            }

            canvasRenderer.SetColor(new Color(color.r, color.g, color.b, 1));
        }
    }
}
