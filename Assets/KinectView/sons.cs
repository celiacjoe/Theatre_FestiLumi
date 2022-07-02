using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sons : MonoBehaviour
{

    public float frequency = 50.0f;
    private float increment;
    private float phase;
    private float sampling_frequency = 48000.0f;
    public float gain;

    float frac(float s) { return s - Mathf.Floor(s); }
    float rand(float s) { return frac(Mathf.Sin(Vector2.Dot(new Vector2(Mathf.Floor(s), 0.0f), new Vector2(12.654f, 0.0f))) * 4032.326f); }
    float noi(float s) { return Mathf.Lerp(rand(s), rand(s + 1.0f), Mathf.SmoothStep(0.0f, 1.0f, frac(s))); }

    private void Update()
    {
        // frequency =Mathf.Pow( noi(Time.time*10.0f),2.0f)*30.0f;

    }
    private void OnAudioFilterRead(float[] data, int channels)

    {
        increment = frequency * 2.0f / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0f;
            }
        }
    }

}
