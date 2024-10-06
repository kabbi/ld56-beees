using UnityEngine;
using UnityEngine.UI;

public class SensitivityAdjuster : MonoBehaviour
{
    public BzzController bzzer;
    public Slider levelBar;
    public Slider slider;
    public Image handle;
    public Color handleColorTriggered;
    public Color handleColorDefault;

    void Start()
    {
        slider.value = bzzer.sensitivity;
    }

    public void OnChange()
    {
        bzzer.sensitivity = slider.value;
        if (SettingsManager.Instance)
        {
            SettingsManager.Instance.sensitivity = slider.value;
        }
    }

    void Update()
    {
        levelBar.value = bzzer.lastLevel;
        handle.color = bzzer.lastLevel > slider.value ? handleColorTriggered : handleColorDefault;
    }
}
