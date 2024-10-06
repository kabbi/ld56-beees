using UnityEngine;
using UnityEngine.UI;

public class SensitivityAdjuster : MonoBehaviour
{
    public BzzController bzzer;
    public Slider levelBar;
    public Slider slider;

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
    }
}
