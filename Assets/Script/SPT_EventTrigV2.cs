using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPT_EventTrigV2 : MonoBehaviour
{
    // Start is called before the first frame update
    //  public GameObject GOTest;
     public SPT_KinectDetection S_Kinect;
     public fluid_dynamics S_Fluid;
    //public dete S_Kinect;
    public bool Debug_Red = false;
    public bool Touch_1 = false;
    private float t1 = 0;
    [Range(0.0f, 1f)]
    public float Touch1PosX;
    [Range(0.0f, 1f)]
    public float Touch1PosY;
    public bool Touch_1Continue = false;
    private float t2 = 0;
    // [Range(0.0f, 120f)]
     //  public float Touch2PosX;
    [Range(0.0f, 1f)]
    public float DistanceT2;

    public bool Touch02 = false;
    private float t3 = 0;

    public GameObject MidiTouch01;
    public GameObject MidiTouch02;
    public GameObject MidiTouch03;
    public GameObject MidiTouch04;
    // public GameObject MidiTouch05;
    public GameObject MidiTouchSound;
   // public GameObject SphereDebug;
   // public GameObject SphereDebug2;

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    void Start()
    {
        MidiBridge.instance.Warmup();
    }

    // Update is called once per frame
    void Update()
    {
        //Touch1PosX = SphereDebug.transform.position.x;
        //Touch2PosX = SphereDebug2.transform.position.x;
        /////////////////////////////// Kinect Setup
        if (Input.GetKeyDown("space"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        /////////////////////////////// Kinect Setup dete
        Touch1PosX = S_Kinect.PosX1;
        Touch1PosY = S_Kinect.PosY1;
        //DistanceT2 = S_Kinect.PosZ2;

        if (Input.GetKeyDown("a"))
        {
            Touch1();
        }
        if (Input.GetKeyDown("z"))
        {
            Touch3();
        }

        if (Input.GetKeyDown("v")) /// If no Touch at all
        {
            NoTouch_1();
            NoTouch_12();
            NoTouch_2();
            Debug.Log("v is Pressed");
        }

        if (Input.GetKeyDown("b")) /// If no Touch 2
        {
            NoTouch_2();
            Debug.Log("b is Pressed");
        }

        /////////////////////////////// Detect if Touch 1 is still pushed & assign Float
        if (Touch_1 == true)
        {
            MidiTouch01.GetComponent<NoteTriggerV2>().SendCC = true;               
            MidiTouch01.GetComponent<NoteTriggerV2>().value = map(Touch1PosX, 0.0f, 1f, 20f, 100f);
            MidiTouch02.GetComponent<NoteTriggerV2>().value = map(Touch1PosY, 0.0f, 1f, 60f, 80f);
           // MidiTouch02.GetComponent<NoteTriggerV2>().value = map(Touch1PosY, 0.0f, 1f, 60f, 85f);
            Touch_1Continue = true;
        }else
        {
            Touch_1Continue = false;
            MidiTouch01.GetComponent<NoteTriggerV2>().SendCC = false;
        }
            /////////////////////////////// Envoie les valeur Pos Y en continu
            if (Touch_1Continue == true)
            {
              //  MidiTouch02.GetComponent<NoteTrigger>().value = map(MidiTouch01.GetComponent<NoteTrigger>().value, 0f, 1f, 80f, 110f);
                MidiTouch02.GetComponent<NoteTriggerV2>().SendCC = true;
            }else
            {
                MidiTouch02.GetComponent<NoteTriggerV2>().SendCC = false;
            }
            /////////////////////////////// Assign Dynamic Value to Touch 02
             if (Touch02 == true)
            {
               // MidiTouch03.GetComponent<NoteTriggerV2>().value = map(DistanceT2, 0f, 0.1f, 0f, 60f);
                MidiTouch03.GetComponent<NoteTriggerV2>().SendCC = true;
            }else
            {
                MidiTouch03.GetComponent<NoteTriggerV2>().SendCC = false;
            }

            /////////////////////////////// Son Rouge
             if (S_Fluid.Red ==true)
            {
             SonRouge();
             S_Fluid.Red =false;
            }

       /* if (Debug_Red == true)
            {
            SonRouge();
            Debug_Red = false;
            }
       */

        }

        public void NoTouch_1()
        {
            if (t1 ==1 )
            {
                MidiTouch01.GetComponent<NoteTriggerV2>().StopCoroutine("NoteOn");
                MidiTouch01.GetComponent<NoteTriggerV2>().StartCoroutine("Note1Off");        
            }
        // MidiTouch01.GetComponent<NoteTrigger>().StopCoroutine("MidiTrig");
        MidiTouch01.GetComponent<NoteTriggerV2>().StopCoroutine("NoteOn");
        MidiTouch01.GetComponent<NoteTriggerV2>().StartCoroutine("Note1Off");
        Touch_1 = false;
            t1 = 0;
        }

        public void NoTouch_12()
        {
            if (t2 == 1)
            {
            MidiTouch02.GetComponent<NoteTriggerV2>().StopCoroutine("Note2On");
            MidiTouch02.GetComponent<NoteTriggerV2>().StartCoroutine("Note2Off");
            }
        MidiTouch02.GetComponent<NoteTriggerV2>().StopCoroutine("Note2On");
        MidiTouch02.GetComponent<NoteTriggerV2>().StartCoroutine("Note2Off");
        Touch_1Continue = false;
            t2 = 0;
        }

        public void NoTouch_2()
        {
            if (t3 == 1)
            {
                MidiTouch03.GetComponent<NoteTriggerV2>().StopCoroutine("Note3On");
                MidiTouch03.GetComponent<NoteTriggerV2>().StartCoroutine("Note3Off");
            }
        MidiTouch03.GetComponent<NoteTriggerV2>().StopCoroutine("Note3On");
        MidiTouch03.GetComponent<NoteTriggerV2>().StartCoroutine("Note3Off");
        // MidiTouch02.GetComponent<NoteTrigger>().StopCoroutine("MidiTrig");
        Touch02 = false;
            t3 = 0;
        }

        ////////// Event Midi trig Gate TOUCH1
        public void Touch1()
        {
            ///////////////////// Premier Touch Declenche Le trigger du GO MidisoundControl associé au Touch01
            if (t1 == 0)
            {
                MidiTouch01.GetComponent<NoteTriggerV2>().StartCoroutine("Note1On");
            }
            t1++;
            Touch_1 = true;
            Debug.Log("Touch1");
        }
         ////////// Event Midi trig Gate TOUCH CONTINU
        public void Touch2()
        {
            if (t2 == 0)
            {
                MidiTouch02.GetComponent<NoteTriggerV2>().StartCoroutine("Note2On");
            }
        Touch_1Continue = true;
            t2++;
        Debug.Log("Touch12");
    }

        ////////// Event Midi trig Gate TOUCH2
        public void Touch3()
        {
            if (t3 == 0 )
            {
                MidiTouch03.GetComponent<NoteTriggerV2>().StartCoroutine("Note3On");          
            }
            Touch02 = true;
            t3++;
           Debug.Log("Touch3");
        }

        public void SonRouge()
        {
                MidiTouchSound.GetComponent<NoteTriggerV2>().StartCoroutine("MidiTrig");
        }

        public void DoubleTouchDist()
        {


        }
        
    }
