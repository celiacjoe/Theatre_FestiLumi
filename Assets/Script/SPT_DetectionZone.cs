using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPT_DetectionZone : MonoBehaviour
{
    public SPT_KinectToPoint S_Kinect;
    public ProceduralAudioController S_Audio;
    public GameObject Traker;
    // Rule for Sound Var value
    public int limitX;
    public int limitY;
    public float TransitionValue;
    // Var ready to sound
    public float SoundValueX;
    public float SoundValueY;
    // Values max min  for sound
    public float ValueX1;
    public float ValueX2;
    public float ValueY1;
    public float ValueY2;
    //Zone Screen var
    public bool ZoneA = false;
    public bool ZoneB = false;
    public bool ZoneC = false;
    public bool ZoneD = false;

    // Function Custom 
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        // Traker.transform.position = new Vector3(S_Kinect.PosX, S_Kinect.PosY, S_Kinect.PosZ);
        if (Input.GetKeyDown("space"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if ( Traker.transform.position.y > limitY )
        {
            ZoneC = false;
            ZoneD = false;

            if ( Traker.transform.position.x < limitX )
            {
                ZoneB = false;
                ZoneA = true;
            }else 
            {
                ZoneA = false;
                ZoneB = true;
            }
            
        }else
        {
            ZoneA = false;
            ZoneB = false;

            if (Traker.transform.position.x < limitX)
            {
                ZoneD = false;
                ZoneC = true;
            }
            else
            {
                ZoneC = false;
                ZoneD = true;
            }
           
        }

        S_Audio.GetComponent<ProceduralAudioController>().mainFrequency = 50.0f + 75.0f * SoundValueX;

        /////////////////////////////////////////////////////////  Sound Value TransitionValue
        SoundValueX = Mathf.Clamp(map(Traker.transform.position.x, -TransitionValue, TransitionValue, ValueX1, ValueX2), ValueX1, ValueX2);
        SoundValueY = Mathf.Clamp(map(Traker.transform.position.y, -TransitionValue, TransitionValue, ValueY1, ValueY2), ValueY1, ValueY2);
        /////////////////////////////////////////////////////////  Assign sound Value to control
        S_Audio.GetComponent<ProceduralAudioController>().mainFrequency = 50.0f + 75.0f * SoundValueX;
        S_Audio.GetComponent<ProceduralAudioController>().frequencyModulationOscillatorIntensity = SoundValueY * 100.0f;
        S_Audio.GetComponent<ProceduralAudioController>().masterVolume = 1.0f + (1.0f - SoundValueY);
        /////////////////////////////////////////////////////////   ChekerZone
        Renderer R = Traker.GetComponent<Renderer>();

        if (ZoneA == true)
        {
            R.material.color = Color.red;
        }else if ( ZoneB == true)
        {
            R.material.color = Color.blue;
        }
        else if (ZoneC == true)
        {
            R.material.color = Color.green;
        }else if ( ZoneD == true)
        {
            R.material.color = Color.white;
        }



    }
}
