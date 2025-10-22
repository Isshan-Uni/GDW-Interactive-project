using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Laptop : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    public TextMeshProUGUI textUI;
    public Animator animator;

    private GameObject Player;
    public GameObject Light;
    private string FinalOutcome;
    private string BeforeOutput = "The old laptop still works.  \nThe unsent email is open on the screen.  \nIt’s been sitting here for years, waiting.";
    private Coroutine typingCoroutine;

    public AudioSource backgroundMusic;
    public float dimVolume = 0.2f;
    public float fadeSpeed = 2f;

    private float originalVolume;
    public AudioSource LaptopSound;

    public AudioSource typingSound;
    void Start()
    {
        textUI.text = "";
        if (backgroundMusic != null)
            originalVolume = backgroundMusic.volume;
    }


    void Update()
    {
        
    }


    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (backgroundMusic != null)
            StartCoroutine(FadeMusic(backgroundMusic.volume, dimVolume));
        typingCoroutine = StartCoroutine(TypeText(BeforeOutput));

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
        yield return new WaitForSeconds(1);
        textUI.text = "";
        yield return new WaitForSeconds(1);
        typingCoroutine = StartCoroutine(TypeFinalText(FinalOutcome));
    }

    IEnumerator TypeFinalText(string text)
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
        if (backgroundMusic != null)
            StartCoroutine(FadeMusic(backgroundMusic.volume, originalVolume));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            FinalOutcome = collision.gameObject.GetComponent<PlayerController>().finalOutcome + "\n\n THE END";
            print("Hi");
            animator.SetTrigger("Collided");
            Light.SetActive(false);
            LaptopSound.PlayOneShot(LaptopSound.clip);
        }
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
