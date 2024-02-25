using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{

    [SerializeField] private float _fadeDuration;
    public float fadeDuration { get {return _fadeDuration;} private set {_fadeDuration = value;} }
    [SerializeField] private CanvasGroup fadePanel;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void FadeIn(string[] text, int sleepTime)
    {

        textMeshPro.SetText(text[0]);
        textMeshPro.ForceMeshUpdate();

        StartCoroutine(DoFadeIn(sleepTime));
    }

    public void FadeOut(string[] text, int sleepTime)
    {

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

        AudioListener.pause = true;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(sleepTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DoFadeOut(int sleepTime)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(sleepTime);
        Time.timeScale = 1f;
        
        AudioListener.pause = false;

        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
