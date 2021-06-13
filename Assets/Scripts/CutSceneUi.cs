using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class CutSceneUi : MonoBehaviour
    {
        public GameState GameState;
        public Camera UiCamera;
        public CanvasRenderer MainPanel;
        public TextMeshProUGUI DeathNote;

        void Start()
        {
            GameState.GameStateChanged += OnGameStateChanged;

            StartCoroutine(PlayOpening());
        }

        void Update()
        {
            if (GameState.State == GameStates.OpeningScreen && Input.GetKeyDown(KeyCode.Space))
            {
                GameState.StartGame();
                StartCoroutine(FadeOut());
            }
        }

        private void OnGameStateChanged(object sender, GameStateChangedEventArgs e)
        {
            if (e.NewState == GameStates.PlayerKilledByZombie)
                StartCoroutine(FadeIn("You fought the good fight on that day. Australia will not forget its heroes..."));
            else if (e.NewState == GameStates.PedestrianKilledByZombie)
                StartCoroutine(FadeIn("The battle ended, but the war had just begun. You are reminded that the loss of life can't be measured in numbers..."));
        }

        private IEnumerator FadeIn(string deathNote)
        {
            UiCamera.enabled = true;
            DeathNote.text = deathNote;
            DeathNote.enabled = false;

            yield return FadeIn(MainPanel, Color.black, 1f);
            yield return new WaitForSeconds(2f);

            DeathNote.enabled = true;
            yield return FadeIn(DeathNote.GetComponent<CanvasRenderer>(), Color.white, 5f);
        }

        private IEnumerator FadeOut()
        {
            StartCoroutine(FadeOut(MainPanel, Color.black, 2f));
            StartCoroutine(FadeOut(DeathNote.GetComponent<CanvasRenderer>(), Color.white, 2f));
            yield return new WaitForSeconds(2f);
            UiCamera.enabled = false;
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

        private IEnumerator FadeOut(CanvasRenderer canvasRenderer, Color color, float speed)
        {
            float alpha = 1f;

            while (alpha > 0f)
            {
                canvasRenderer.SetColor(new Color(color.r, color.g, color.b, alpha));
                alpha -= speed * Time.deltaTime;
                yield return null;
            }

            canvasRenderer.SetColor(new Color(color.r, color.g, color.b, 0));
        }

        private IEnumerator PlayOpening()
        {
            DeathNote.enabled = false;
            yield return new WaitForSeconds(2f);
            DeathNote.enabled = true;
            DeathNote.text = "You are Australia's first Siamese twin with a conjoined brain. You had to work doubly hard to make it as a traffic cop. But on that historic day, you were about to face your biggest challenge yet...\n\n\n\nPress Space to start";
            yield return FadeIn(DeathNote.GetComponent<CanvasRenderer>(), Color.white, 3f);
        }
    }
}
