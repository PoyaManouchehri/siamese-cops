using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Rendering.HybridV2;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class CutSceneUi : MonoBehaviour
    {
        public GameState GameState;
        public Camera UiCamera;
        public CanvasRenderer MainPanel;
        public TextMeshProUGUI DeathNote;
        public TextMeshProUGUI LovingMemory;
        public RectTransform Scroller;

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

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
            {
                StartCoroutine(FadeIn("You fought the good fight on that day. Australia will not forget its heroes..."));
                StartCoroutine(Scroll());
            }

            else if (e.NewState == GameStates.PedestrianKilledByZombie)
            {
                StartCoroutine(FadeIn("Your courage was commendable, but the the loss of life was too great that day. It was time to regroup and plan again..."));
                StartCoroutine(Scroll());
            }

            else if (e.NewState == GameStates.LevelCleared)
            {
                StartCoroutine(FadeIn("The battle ended, but the war had just begun. You were reminded that the loss of life can't be measured in numbers..."));
                StartCoroutine(Scroll());
            }
        }

        private IEnumerator FadeIn(string deathNote)
        {
            UiCamera.enabled = true;
            DeathNote.text = deathNote;
            LovingMemory.text = GetLovingMemoryText();
            DeathNote.enabled = false;
            LovingMemory.enabled = false;

            yield return FadeIn(MainPanel, Color.black, 1f);
            yield return new WaitForSeconds(2f);

            DeathNote.enabled = true;
            LovingMemory.enabled = true;

            StartCoroutine(FadeIn(DeathNote.GetComponent<CanvasRenderer>(), Color.white, 2f));
            StartCoroutine(FadeIn(LovingMemory.GetComponent<CanvasRenderer>(), Color.white, 2f));
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
            LovingMemory.enabled = false;
            yield return new WaitForSeconds(1f);
            DeathNote.enabled = true;
            DeathNote.text = "You are Australia's first Siamese twin with conjoined brain function. You have had to work doubly hard to make it as a traffic cop. But on that historic day, you were about to face your biggest challenge yet...\n\n\n\nPress Space to start";
            yield return FadeIn(DeathNote.GetComponent<CanvasRenderer>(), Color.white, 3f);
        }

        string GetLovingMemoryText()
        {
            var stringBuilder = new StringBuilder();

            if (GameState.Deaths.Count == 0)
            {
                stringBuilder.Append("No-one died by your hand. That's no small achievement!");
            }
            else
            {
                stringBuilder.Append("In Loving Memory of...\n\n\n\n");

                foreach (var death in GameState.Deaths)
                {
                    var name = death.Gender == Gender.Male
                        ? Names.MaleNames[_random.Next(0, Names.MaleNames.Length)]
                        : Names.FemaleNames[_random.Next(0, Names.FemaleNames.Length)];

                    stringBuilder.Append($"{name} (shot), {GetDescriptor(death)}\n\n");
                }
            }

            return stringBuilder.ToString();
        }

        string GetRelation(Death death)
        {
            var titles = new List<string>();

            if (death.Gender == Gender.Female) titles.Add("sister");
            else titles.Add("brother");

            if (death.Age > 21)
            {
                if (death.Gender == Gender.Female) titles.AddRange(new[]{"mother", "aunt"});
                else titles.AddRange(new[] { "father", "uncle" });
            }

            return $"loving {titles[_random.Next(0, titles.Count)]} of {_random.Next(1, 5)}";
        }

        string GetDescriptor(Death death)
        {
            var extras = new List<string>();

            if (death.Age <= 15)
            {
                extras.AddRange(new[]
                {
                    "mathematics genius.",
                    "gifted athlete, destined to become a star.",
                    "the nicest kid at school, loved by students and teachers alike.",
                    "voted young Australian of the decade."
                });
            }
            else
            {
                extras.AddRange(new[]
                {
                    "extraordinary teacher.",
                    "best damn plumber this town has ever seen.",
                    "chef, baker, foster parent.",
                    "bit heart, huge smile, always caring.",
                    "Physicist, biochemist, botanist, quilting champion."
                });
            }

            return Math.Abs(_random.NextDouble()) < 0.25 ? extras[_random.Next(0, extras.Count)] : GetRelation(death);
        }

        private IEnumerator Scroll()
        {
            yield return new WaitForSeconds(3f);

            while (true)
            {
                Scroller.transform.position += Vector3.up * Time.deltaTime * 2f;
                yield return null;
            }
        }
    }
}
