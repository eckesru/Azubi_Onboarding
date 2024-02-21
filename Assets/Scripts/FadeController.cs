using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Initial, um Spiel zu starten ohne schwarzen Bildschirm
        FadeOut();
    }

    public void FadeIn()
    {
        StartCoroutine(DoFadeIn());
    }

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeIn()
    {
        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    IEnumerator DoFadeOut()
    {
        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
