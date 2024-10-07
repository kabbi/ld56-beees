using UnityEngine;

public class NoMicButton : MonoBehaviour
{
    public GameObject modal;
    public GameObject beeBox;

    public void OnClick()
    {
        modal.SetActive(true);
        beeBox.SetActive(false);
        if (SettingsManager.Instance)
        {
            SettingsManager.Instance.microphoneUnusable = true;
        }
    }
}
