using UnityEngine;
using System.Collections;
using TMPro;

public class FadeController : MonoBehaviour
{

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup fadePanel;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = transform.parent.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void FadeIn(string[] text, int sleepTime)
    {

        textMeshPro.SetText(text[0]);
        textMeshPro.ForceMeshUpdate();

        StartCoroutine(DoFadeIn(sleepTime));
    }

    public void FadeOut(string[] text, int sleepTime)
    {
        AudioListener.volume = 0;

        textMeshPro.SetText(text[0]);
        textMeshPro.ForceMeshUpdate();

        
        StartCoroutine(DoFadeOut(sleepTime));
    }

    private IEnumerator DoFadeIn(int sleepTime)
    {
        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }

        AudioListener.volume = 0;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(sleepTime);
        Application.Quit();
    }

    private IEnumerator DoFadeOut(int sleepTime)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(sleepTime);
        Time.timeScale = 1f;
        
        AudioListener.volume = 1;

        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
