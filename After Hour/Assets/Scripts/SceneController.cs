using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public string level;
    public GameObject Mainmenu;
    public GameObject Exposition;
    public GameObject ControlMenu;


    public string fullText;
    public TextMeshProUGUI ContinueText;
    public TextMeshProUGUI SkipText;

    public float typingSpeed = 0.05f;
    public TextMeshProUGUI textUI;

    private Coroutine typingCoroutine;
    private bool TextComplete = false;
    public AudioSource typingSound;

    public AudioSource backgroundMusic;
    public float dimVolume = 0.2f;
    public float fadeSpeed = 2f;

    public AudioSource stepssound;


    private void Start()
    {

        ContinueText.gameObject.SetActive(false);
        SkipText.gameObject.SetActive(false);

    }
    public GameObject gmPanel;
    public float fadeDuration = 1.5f;

    private Image panelImage;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        panelImage = gmPanel.GetComponent<Image>();

        Color color = panelImage.color;
        color.a = 0f;
        panelImage.color = color;
    }

    public void FadeSequence()
    {
        StartCoroutine(FadeWithDelay());
    }

    private IEnumerator FadeWithDelay()
    {
        FadeToBlack();             // Fade to black
        yield return new WaitForSeconds(fadeDuration);  // Wait for 1 second
        FadeFromBlack();           // Then fade from black
        StartTyping();
    }

    public void FadeSequence2()
    {
        StartCoroutine(FadeWithDelay2());
    }

    private IEnumerator FadeWithDelay2()
    {
        FadeToBlack2();
        stepssound.PlayOneShot(stepssound.clip);
        yield return new WaitForSeconds(4f);  // Wait for 1 second
        SceneManager.LoadScene(level);
    }

    public void FadeToBlack2()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(0f, 1f, Mainmenu, false));
    }

    public void FadeToBlack()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(0f, 1f, Mainmenu, false));
    }

    public void FadeFromBlack()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(1f, 0f, Exposition, true));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, GameObject target, bool enable)
    {
        if (enable) target.SetActive(enable);
        float timeElapsed = 0f;
        Color color = panelImage.color;

        while (timeElapsed < fadeDuration)
        {
            float t = timeElapsed / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            panelImage.color = color;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        panelImage.color = color;
        if (!enable) target.SetActive(enable);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(level);
    }

    public void MainMenu()
    {
        print("Thank You for Playing");
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StopCoroutine(typingCoroutine);
            textUI.text = fullText;
            ContinueText.gameObject.SetActive(true);
            TextComplete = true;

        }

        if (TextComplete == true && Input.anyKeyDown)
        {
            FadeSequence2();
        }
    }


    public void StartTyping()
    {

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);


        if (backgroundMusic != null)
            StartCoroutine(FadeMusic(backgroundMusic.volume, dimVolume));

        typingCoroutine = StartCoroutine(TypeText(fullText));
        SkipText.gameObject.SetActive(true);
    }

    IEnumerator TypeText(string text)
    {
        textUI.text = "";
        foreach (char c in text)
        {
            textUI.text += c;
            if (typingSound != null && !char.IsWhiteSpace(c))
            {
                typingSound.pitch = Random.Range(0.95f, 1.05f);
                typingSound.PlayOneShot(typingSound.clip);
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        ContinueText.gameObject.SetActive(true);
        TextComplete = true;
    }

    IEnumerator FadeMusic(float startVolume, float endVolume)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            backgroundMusic.volume = Mathf.Lerp(startVolume, endVolume, t);
            yield return null;
        }
    }
}
