using System;
using Unity.VisualScripting;
using UnityEngine;

public class BzzController : MonoBehaviour
{
    private AudioClip microphoneInput;
    private bool microphoneInitialized;
    private PlayerController playerController;
    public float sensitivity;
    private bool flapped;
    private float lastLevel;
    private float lastVariance;
    private float lastZCR;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (Microphone.devices.Length > 0)
        {
            microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);
            microphoneInitialized = true;
        }
    }

    void Update()
    {
        //get mic volume
        int dec = 128;
        float[] waveData = new float[dec];
        int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
        microphoneInput.GetData(waveData, micPosition);

        lastVariance = CalculateVariance(waveData);
        lastZCR = CalculateZeroCrossingRate(waveData);

        // Getting a peak on the last 128 samples
        float levelMax = 0;
        for (int i = 0; i < dec; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        float level = Mathf.Sqrt(Mathf.Sqrt(levelMax));
        lastLevel = level;

        if (level > sensitivity && !flapped)
        {
            // Flap();
            playerController.verticalSpeed = 1;
            flapped = true;
        }
        if (level < sensitivity && flapped)
        {
            playerController.verticalSpeed = 0;
            flapped = false;
        }

    }

    private float CalculateVariance(float[] samples)
    {
        float mean = 0.0f;
        foreach (float sample in samples)
        {
            mean += sample;
        }
        mean /= samples.Length;

        float variance = 0.0f;
        foreach (float sample in samples)
        {
            variance += Mathf.Pow(sample - mean, 2);
        }
        variance /= samples.Length;

        return variance * 1000;
    }

    private float CalculateZeroCrossingRate(float[] samples)
    {
        int zeroCrossings = 0;
        for (int i = 1; i < samples.Length; i++)
        {
            if (samples[i] * samples[i - 1] < 0) // If the product is negative, a zero crossing occurred
            {
                zeroCrossings++;
            }
        }

        return (float)zeroCrossings / samples.Length * 10;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 10, 200), "");
        GUI.Box(new Rect(25, 10, 10, 200 * lastLevel), "");
        // GUI.Box(new Rect(35, 10, 10, 200 * Math.Abs(lastVariance)), "");
        // GUI.Box(new Rect(45, 10, 10, 200 * lastZCR), "");
    }
}
