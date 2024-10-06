using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public float sensitivity;
    public bool microphoneUnusable;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Two settings managers, killing myself");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
