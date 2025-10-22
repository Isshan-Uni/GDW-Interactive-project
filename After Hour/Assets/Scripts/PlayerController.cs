using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float groundDrag = 6f;


    private Rigidbody rb;
    private bool grounded;
    private bool TextOn;
    private bool WrittingText;
    private GameObject TextE;
    private string fullText;
    private string OptionE;
    private string OptionQ;
    public string finalOutcome;

    public AudioSource backgroundMusic;    
    public float dimVolume = 0.2f;         
    public float fadeSpeed = 2f;

    public GameObject Laptop;

    private Animator Robot;
    public TextMeshProUGUI ObjectText;

    int guiltLevel;
    int ObjectNo;

    public TypingEffect Texteffect;
    private float originalVolume;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Robot = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Laptop.SetActive(false);

        if (backgroundMusic != null)
            originalVolume = backgroundMusic.volume;
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetButtonDown("Jump") && grounded)
            rb.linearDamping = grounded ? groundDrag : 0f;

        
        if (Input.GetKey(KeyCode.E) && TextOn && !WrittingText)
        {
            Destroy(TextE);
            WrittingText = true;
            TextOn = false;

          
            if (backgroundMusic != null)
                StartCoroutine(FadeMusic(backgroundMusic.volume, dimVolume));

            Texteffect.StartTyping(fullText, OptionQ, OptionE);
        }

        
        if (Texteffect.OptionQ.text != "" && Input.GetKeyDown(KeyCode.Q))
        {
            guiltLevel += 1;
            WrittingText = false;
            ClearText();
            ObjectNo += 1;
            Texteffect.animator.SetTrigger("Done");

           
            Texteffect.RestoreLight();
            if (backgroundMusic != null)
                StartCoroutine(FadeMusic(backgroundMusic.volume, originalVolume));
        }
     
        else if (Texteffect.OptionE.text != "" && Input.GetKeyDown(KeyCode.E))
        {
            guiltLevel += 0;
            WrittingText = false;
            ClearText();
            ObjectNo += 1;
            Texteffect.animator.SetTrigger("Done");

          
            Texteffect.RestoreLight();
            if (backgroundMusic != null)
                StartCoroutine(FadeMusic(backgroundMusic.volume, originalVolume));
        }

        ObjectText.text = "Objects = " + ObjectNo + "/3";

        if (ObjectNo == 3)
        {
            Laptop.SetActive(true);
            ObjectText.text = "";

            if (guiltLevel == 3)
            {
                finalOutcome = "Alex begins typing again.\nThe words come slowly, but they’re honest this time.\n“I’m sorry.”\nThey press send and finally feel at peace.";
            }
            else if (guiltLevel == 2 || guiltLevel == 1)
            {
                finalOutcome = "Alex closes the laptop without sending anything.\nNo anger, no excuses.\nJust quiet acceptance.\nSome things don’t need to be fixed.";
            }
            else if (guiltLevel == 0)
            {
                finalOutcome = "Alex deletes the email.\nNo message, no apology.\nIt’s gone for good.\nAt least, that’s what they keep telling themself.";
            }
        }
    }

    void ClearText()
    {
        Texteffect.OptionQ.text = "";
        Texteffect.OptionE.text = "";
        Texteffect.textUI.text = "";
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

    void MovePlayer()
    {
        if (WrittingText) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = (transform.forward * v + transform.right * h).normalized;

        Vector3 targetVel = moveDir * moveSpeed;
        Vector3 vel = rb.linearVelocity;
        vel.x = Mathf.Lerp(vel.x, targetVel.x, 10f * Time.deltaTime);
        vel.z = Mathf.Lerp(vel.z, targetVel.z, 10f * Time.deltaTime);
        rb.linearVelocity = vel;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Text"))
        {
            Transform text = collision.transform.Find("PressEText");
            if (text != null)
            {
                text.gameObject.SetActive(true);
                TextE = text.gameObject;
            }

            TextOn = true;
            fullText = collision.gameObject.GetComponent<Object>().FullText;
            OptionE = collision.gameObject.GetComponent<Object>().OptionE;
            OptionQ = collision.gameObject.GetComponent<Object>().OptionQ;
            Texteffect.animator = collision.gameObject.GetComponentInChildren<Animator>();
        }

        if (collision.gameObject.CompareTag("Laptop"))
        {
            WrittingText = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Text"))
        {
            Transform text = collision.transform.Find("PressEText");
            if (text != null)
                text.gameObject.SetActive(false);

            TextOn = false;
        }
    }
}



