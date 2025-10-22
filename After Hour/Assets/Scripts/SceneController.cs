using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string level;
    public GameObject Mainmenu;
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


    private void Start()
    {

        ContinueText.gameObject.SetActive(false);
        SkipText.gameObject.SetActive(false);

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

        if(TextComplete == true && Input.anyKeyDown)
        {
            SceneManager.LoadScene(level);
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
