using UnityEngine;
using System;
using Lomont;

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
    private float[] waveData = new float[64];
    private double[] lastFFT = new double[128];
    private LomontFFT fft;

    void Start()
    {
        fft = new LomontFFT();
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
        int micPosition = Microphone.GetPosition(null) - (waveData.Length + 1); // null means the first microphone
        microphoneInput.GetData(waveData, micPosition);

        // lastVariance = CalculateVariance(waveData);
        // lastZCR = CalculateZeroCrossingRate(waveData);
        // CalculateFFT(waveData);

        // Getting a peak on the last 128 samples
        float levelMax = 0;
        for (int i = 0; i < waveData.Length; i++)
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
            playerController.SetVerticalInput(1);
            flapped = true;
        }
        if (level < sensitivity && flapped)
        {
            playerController.SetVerticalInput(0);
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

    private void CalculateFFT(float[] samples)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            lastFFT[i * 2] = samples[i];
            lastFFT[i * 2 + 1] = 0;
        }
        fft.RealFFT(lastFFT, true);
        for (int i = 0; i < samples.Length; i++)
        {
            double real = lastFFT[i * 2];
            double imaginary = lastFFT[i * 2 + 1];
            lastFFT[i * 2] = Math.Sqrt(real * real + imaginary * imaginary);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 10), "");
        GUI.Box(new Rect(10, 10, 200 * lastLevel, 10), "");
        GUI.Box(new Rect(10 + 200 * sensitivity, 10, 2, 10), "");
        // for (int i = 2; i < lastFFT.Length / 2; i++)
        // {
        //     double v = lastFFT[i * 2] * 10;
        //     GUI.Box(new Rect(i * 15, 10, 10, (float)(200 * v)), "");
        // }
    }
}
