using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TypingEffect : MonoBehaviour
{


    public float typingSpeed = 0.05f;
    public TextMeshProUGUI textUI;
    public TextMeshProUGUI OptionQ;
    public TextMeshProUGUI OptionE;

    public AudioSource typingSound;
    public Animator animator;

    public Light spotlight;        
    public float dimIntensity = 0.5f;
    public float fadeSpeed = 2f;

    private Coroutine typingCoroutine;
    private float originalIntensity;

    void Start()
    {
        textUI.text = "";
        OptionE.text = "";
        OptionQ.text = "";
        if (spotlight != null)
            originalIntensity = spotlight.intensity;
    }

    public void StartTyping(string newText,string Qtext, string Etext)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        animator.SetTrigger("Start");
        if (spotlight != null)
            StartCoroutine(FadeLight(originalIntensity, dimIntensity));
        typingCoroutine = StartCoroutine(TypeText(newText,Qtext,Etext));
        
    } 

    IEnumerator TypeText(string text, string Qtext, string Etext)
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

        OptionE.text = Etext + "\n\n\n Press E";
        OptionQ.text = Qtext + "\n\n\n Press Q";
    }

    public void RestoreLight()
    {
        if (spotlight != null)
            StartCoroutine(FadeLight(dimIntensity, originalIntensity));
    }

    IEnumerator FadeLight(float startValue, float endValue)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            spotlight.intensity = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
    }
}
