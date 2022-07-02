
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SPT_Soundmaster : MonoBehaviour
{


    SawWave sawAudioWave;
    SquareWave squareAudioWave;
    SinusWave sinusAudioWave;

    SinusWave amplitudeModulationOscillator;
    SinusWave frequencyModulationOscillator;


    [Header("Volume / Frequency")]
    [Range(0.0f, 10.0f)]
    public float masterVolume = 0.5f;
    [Range(-5, 1500)]
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

    [Header("Noise")]
    [Range(0.0f, 300.0f)]
    public float IntensityVariation = 0.0f;
    public float SpeedVarFrequence = 1.0f;
    [Range(0.0f, 5f)]
    public float IntensityNoise = 0.0f;
    public float SpeedNoise = 1.0f;
    [Range(-5.0f, 5f)]
    public float modifCurve;
    public float Multiplier = 1.0f;
    public float BPM = 1.0f;


    [Header("Attaque")]
    public bool Continu = true;
    public bool Square = false;
    public bool Cisor = false;
    public bool Cisor2 = false;
    public bool WaveSmooth = false;
    public bool WaveHard = false;

    public bool Noise = false;
    public bool TempoBpm = false;

    private float fSquare = 0.0f;
    private float fCisor = 0.0f;
    private float fCisor2 = 0.0f;
    private float fWaveSmooth = 0.0f;
    private float fWaveHard = 0.0f;

    private float fTempoBpm = 0.0f;

    private float fVarFrequence = 0.0f;
    private float fNoise = 0.0f;
    private float phase;
    private float phasenoise;

    private float CurrentfActtack;
    private float CurrentfNoise;


    float mainFrequencyPreviousValue;
    private System.Random RandomNumber = new System.Random();

    private double sampleRate;  
                               
    private double dataLen;     
    double chunkTime;
    double dspTimeStep;
    double currentDspTime;

    //////////////::::: Funtion

    float frac(float s) { return s - Mathf.Floor(s); }
    float rand(float s) { return frac(Mathf.Sin(Vector2.Dot(new Vector2(Mathf.Floor(s), 0.0f), new Vector2(12.654f, 0.0f))) * 4032.326f); }
    float noi(float s) { return Mathf.Lerp(rand(s), rand(s + 1.0f), Mathf.SmoothStep(0.0f, 1.0f, frac(s))); }

 /*   //////////////::::: Funtion Attack

    float fSquare = Mathf.Floor(frac(t) * 2.0f);
    float fCisor = Mathf.Pow(1.0f - frac(t* 2.0f), modifCurve);
    float fCisor2 = Mathf.Pow(frac(t* 2.0f), modifCurve);
    float fWaveSmooth = Mathf.Pow(Mathf.Sin(t) * 0.5f + 0.5f, modifCurve);
    float fWaveHard = Mathf.Pow(Mathf.Abs(frac(t) - 0.5f) * 2.0f, modifCurve);
    float ftempoBpm = Mathf.Floor(Mathf.Clamp((frac(t / Mathf.Floor(BPM)) * Mathf.Floor(BPM)) - Mathf.Floor(BPM) + 1.0f, 0.0f, 1.0f) * 2.0f);
    */

    void Awake()
    {
        sawAudioWave = new SawWave();
        squareAudioWave = new SquareWave();
        sinusAudioWave = new SinusWave();

        amplitudeModulationOscillator = new SinusWave();
        frequencyModulationOscillator = new SinusWave();

        sampleRate = AudioSettings.outputSampleRate;
    }

    private void Update()
    {
        //////UPDATE//////////::::::::::: Function Attack

        float t = Time.time * Multiplier + 0.00003f;
        fSquare = Mathf.Floor(frac(t) * 2.0f);
        fCisor = Mathf.Pow(1.0f - frac(t * 2.0f), modifCurve);
        fCisor2 = Mathf.Pow(frac(t * 2.0f), modifCurve);
        fWaveSmooth = Mathf.Pow(Mathf.Sin(t) * 0.5f + 0.5f, modifCurve);
        fWaveHard = Mathf.Pow(Mathf.Abs(frac(t) - 0.5f) * 2.0f, modifCurve);
        
        fTempoBpm = Mathf.Floor(Mathf.Clamp((frac(t / Mathf.Floor(BPM)) * Mathf.Floor(BPM)) - Mathf.Floor(BPM) + 1.0f, 0.0f, 1.0f) * 2.0f);

        fVarFrequence = noi(Time.time * SpeedVarFrequence * 10.0f)-0.5f;
        fNoise = noi(Time.time * SpeedNoise * 10.0f + 78.236f)-0.5f;

        /////UPDATE///////////::::::::::: Condition Attack
        if (Continu)
        {
            Square = false;
            Cisor = false;
            Cisor2 = false;
            WaveSmooth = false;
            WaveHard = false;
        }
        if (Square)
        {
            CurrentfActtack = fSquare;
            Cisor = false;
            Cisor2 = false;
            WaveSmooth = false;
            WaveHard = false;
        }

        if (Cisor)
        {
            CurrentfActtack = fCisor;
            Square = false;
            Cisor2 = false;
            WaveSmooth = false;
            WaveHard = false;
        }
        if (Cisor2)
        {
            CurrentfActtack = fCisor2;
            Square = false;
            Cisor = false;
            WaveSmooth = false;
            WaveHard = false;
        }
        if (WaveSmooth)
        {
            CurrentfActtack = fWaveSmooth;
            Square = false;
            Cisor = false;
            Cisor2 = false;
            WaveHard = false;
        }
        if (WaveHard)
        {
            CurrentfActtack = fWaveHard;
            Square = false;
            Cisor = false;
            Cisor2 = false;
            WaveSmooth = false;
        }

        if (Noise)
        {
            CurrentfNoise = fNoise;

        }
        else
        {
            CurrentfNoise = 0f;
        }

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
            double currentFreq = mainFrequency +fVarFrequence * IntensityVariation;
            
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

            if (Continu)
            {
               signalValue *= 1;
               signalValue += CurrentfNoise * IntensityNoise;
            }
            else
            {
                signalValue *= CurrentfActtack * fTempoBpm;
                signalValue += CurrentfNoise * IntensityNoise;
            }
           float x = masterVolume * 0.5f * (float)signalValue;

            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] = x;
            }
        }

    }

}
