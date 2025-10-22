using UnityEngine;

public class GuiltManager : MonoBehaviour
{
    public static GuiltManager Instance;   

    public int GuiltLevel;  

    void Awake()
    {
        // Keep only one instance between scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void SetGuilt(int value)
    {
        GuiltLevel += value;
    }


    public int GetGuilt()
    {
        return GuiltLevel;
    }

}
