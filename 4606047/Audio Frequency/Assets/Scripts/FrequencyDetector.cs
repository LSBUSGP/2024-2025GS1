using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class FrequencyDetector : MonoBehaviour
{
    // Audio source playing the music
    public AudioSource audioSource;

    // Frequencies to detect in Hz (4 values) 
    public float[] frequencies = new float[4] { 100f, 500f, 1000f, 3000f };
    public float[] thresholds = new float[4] { 0.01f, 0.01f, 0.01f, 0.01f };

    // GameObjects to show/hide when frequencies are detected
    public GameObject[] targets = new GameObject[4];

    // Number of samples for the FFT (must be a power of two)
    public int sampleSize = 1024;

    private float[] spectrum;
    private float sampleRate;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        spectrum = new float[sampleSize];
        sampleRate = AudioSettings.outputSampleRate;
    }

    void Update()
    {
        // Get the spectrum data using a simple rectangular window
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 0; i < frequencies.Length && i < targets.Length; i++)
        {
            int index = FrequencyToIndex(frequencies[i]);
            bool detected = spectrum[index] >= thresholds[i];
            if (targets[i] != null)
                targets[i].SetActive(detected);
        }
    }

    int FrequencyToIndex(float freq)
    {
        // Convert frequency (Hz) into an index for the spectrum array
        float fraction = freq / sampleRate;
        int i = Mathf.FloorToInt(fraction * sampleSize);
        return Mathf.Clamp(i, 0, sampleSize - 1);
    }
}
