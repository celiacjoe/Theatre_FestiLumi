
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class sonsmaster : MonoBehaviour
{


    SawWave sawAudioWave;
    SquareWave squareAudioWave;
    SinusWave sinusAudioWave;

    SinusWave amplitudeModulationOscillator;
    SinusWave frequencyModulationOscillator;

    

    [Header("Volume / Frequency")]
    [Range(0.0f, 20.0f)]
    public float masterVolume = 0.5f;
    [Range(0, 2000)]
    public double mainFrequency = 500;
    [Space(10)]

    [Header("Tone Adjustment")]
    public bool useSinusAudioWave;
    [Range(0.0f, 1.0f)]
    public float sinusAudioWaveIntensity = 0.25f;
    [Space(5)]
    public bool useSquareAudioWave;
    [Range(0.0f, 1.0f)]
    public float squareAudioWaveIntensity = 0.25f;
    [Space(5)]
    public bool useSawAudioWave;
    [Range(0.0f, 1.0f)]
    public float sawAudioWaveIntensity = 0.25f;
    [Space(10)]

    public float modifCurve;
    public float speed = 1.0f;
    public float bpm = 1.0f;
    public float noise1 = 0.0f;
    public float noise2 = 0.0f;
    public float speedNoise1 = 1.0f;
    public float speedNoise2 = 1.0f;


    private float tempo01 = 0.0f;
    private float tempo02 = 0.0f;
    private float tempo03 = 0.0f;
    private float tempo04 = 0.0f;
    private float tempo05 = 0.0f;
    private float tempoBpm = 0.0f;
    [Range(0, 500)]
    private float noise01 = 0.0f;
    [Range(0, 1)]
    private float noise02 = 0.0f;
    private float phase;
    private float phasenoise;


    float mainFrequencyPreviousValue;
    private System.Random RandomNumber = new System.Random();

    private double sampleRate;  
                               
    private double dataLen;     
    double chunkTime;
    double dspTimeStep;
    double currentDspTime;

    void Awake()
    {
        sawAudioWave = new SawWave();
        squareAudioWave = new SquareWave();
        sinusAudioWave = new SinusWave();

        amplitudeModulationOscillator = new SinusWave();
        frequencyModulationOscillator = new SinusWave();

        sampleRate = AudioSettings.outputSampleRate;
    }
    float frac(float s) { return s - Mathf.Floor(s); }
    float rand(float s) { return frac(Mathf.Sin(Vector2.Dot(new Vector2(Mathf.Floor(s), 0.0f), new Vector2(12.654f, 0.0f))) * 4032.326f); }
    float noi(float s) { return Mathf.Lerp(rand(s), rand(s + 1.0f), Mathf.SmoothStep(0.0f, 1.0f, frac(s))); }
    private void Update()
    {

        float t = Time.time * speed + 0.00003f;
        tempo01 = Mathf.Floor(frac(t) * 2.0f);
        tempo02 = Mathf.Pow(1.0f - frac(t * 2.0f), modifCurve);
        tempo03 = Mathf.Pow(frac(t * 2.0f), modifCurve);
        tempo04 = Mathf.Pow(Mathf.Sin(t) * 0.5f + 0.5f, modifCurve);
        tempo05 = Mathf.Pow(Mathf.Abs(frac(t) - 0.5f) * 2.0f, modifCurve);
        tempoBpm = Mathf.Floor(Mathf.Clamp((frac(t / Mathf.Floor(bpm)) * Mathf.Floor(bpm)) - Mathf.Floor(bpm) + 1.0f, 0.0f, 1.0f) * 2.0f);
        noise01 = noi(Time.time * speedNoise1 * 10.0f)-0.5f;
        noise02 = noi(Time.time * speedNoise2 * 10.0f + 78.236f)-0.5f;


    }
    void OnAudioFilterRead(float[] data, int channels)
    {

        currentDspTime = AudioSettings.dspTime;
        dataLen = data.Length / channels;   
        chunkTime = dataLen / sampleRate;   
        dspTimeStep = chunkTime / dataLen; 

        double preciseDspTime;
        for (int i = 0; i < dataLen; i++)
        {
            preciseDspTime = currentDspTime + i * dspTimeStep;
            double signalValue = 0.0;
            double currentFreq = mainFrequency +noise01 * noise1;
            
            if (useSinusAudioWave)
            {
                signalValue += sinusAudioWaveIntensity * sinusAudioWave.calculateSignalValue(preciseDspTime, currentFreq);
            }
            if (useSawAudioWave)
            {
                signalValue += sawAudioWaveIntensity * sawAudioWave.calculateSignalValue(preciseDspTime, currentFreq);
            }
            if (useSquareAudioWave)
            {
                signalValue += squareAudioWaveIntensity * squareAudioWave.calculateSignalValue(preciseDspTime, currentFreq);
            }

            signalValue *= tempo03 * tempoBpm;
            signalValue += noise02 * noise2;
           float x = masterVolume * 0.5f * (float)signalValue;

            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] = x;
            }
        }

    }

}
