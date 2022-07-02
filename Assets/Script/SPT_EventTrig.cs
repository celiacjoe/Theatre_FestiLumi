using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPT_EventTrig : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GOTest;
    public SPT_KinectDetection S_Kinect;
    public fluid_dynamics S_Fluid;
    //public dete S_Kinect;


    public bool Touch01 = false;
    private float t1 = 0;
    [Range(0.0f, 1f)]
    public float Touch1PosX;
  //  public float Touch1PosY;
    public bool Touch02 = false;
    private float t2 = 0;
    // [Range(0.0f, 120f)]
    //   public float Touch2PosX;
    [Range(0.0f, 1f)]
    public float DistanceT2;
    [Range(0.0f, 0.5f)]
    public float Touch2PosY;
    public bool Touch03 = false;
   // public bool Sound = false;
    //[Range(0.0f, 1f)]
    // public float Touch3PosX;
    //  [Range(0.0f, 120f)]
    // public float Touch3PosY;
    private float t3 = 0;
    //  public GameObject SphereDebug;
    //  public GameObject SphereDebug2;

    public GameObject MidiTouch01;
    public GameObject MidiTouch02;
    public GameObject MidiTouch03;
    public GameObject MidiTouch04;
    public GameObject MidiTouch05;
    public GameObject MidiTouchSound;

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    void Start()
    {
        MidiBridge.instance.Warmup();
       // Touch1PosX = SphereDebug.transform.position.x;
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
        Touch2PosY = S_Kinect.PosY1;
        DistanceT2 = S_Kinect.PosZ2;
        /////////////////////////////// Debug Touch 01 with Space

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("SPACE");
            if (t1 == 0)
            {
                Debug.Log("SPACE2");
                MidiTouch01.GetComponent<NoteTrigger>().StartCoroutine("Note1On");         
            }
            t1++;
            Touch01 = true;
        }

            if (Input.GetKeyDown("return"))
        {
            if (t3 == 0 && Touch01 ==true)
            {
               // MidiTouch03.GetComponent<NoteTrigger>().StartCoroutine("MidiTrig");
                MidiTouch04.GetComponent<NoteTrigger>().StartCoroutine("Note4On");
            }
            t3++;
            Touch03 = true;
        }

        if (S_Fluid.Red ==true)
        {
            Debug.Log("Red");

                MidiTouchSound.GetComponent<NoteTrigger>().StartCoroutine("MidiTrig");

          //  Sound = true;
        }

        if (Input.GetKeyDown("v"))
        {
            Nothing();
            Nothing4();
            Debug.Log("v is Pressed");
        }

        if (Input.GetKeyDown("b"))
        {
            Nothing4();
            Debug.Log("b is Pressed");
        }

        /////////////////////////////// Detect if Touch 1 is still pushed & assign Float
        if (Touch01 == true)
        {
            MidiTouch01.GetComponent<NoteTrigger>().SendCC = true;
            MidiTouch01.GetComponent<NoteTrigger>().value = map(Touch1PosX, 0.0f, 1f, 30f, 100f);
            MidiTouch02.GetComponent<NoteTrigger>().value = map(Touch2PosY, 0.0f, 1f, 60f, 85f);
            Touch02 = true;

        } else
        {
            //MidiTouch01.GetComponent<NoteTrigger>().SendNoteOffC2();
            Touch02 = false;
            MidiTouch01.GetComponent<NoteTrigger>().SendCC = false;
        }

        if (Touch02 == true)
        {
           //  MidiTouch02.GetComponent<NoteTrigger>().value = map(Touch2PosX, 0f, 127f, 85f, 110f);
          //  MidiTouch02.GetComponent<NoteTrigger>().value = map(MidiTouch01.GetComponent<NoteTrigger>().value, 0f, 1f, 80f, 110f);
            MidiTouch02.GetComponent<NoteTrigger>().SendCC = true;
        }
        else
        {
            MidiTouch02.GetComponent<NoteTrigger>().SendCC = false;
        }

        if (Touch03 == true)
        {
            MidiTouch04.GetComponent<NoteTrigger>().value = map(DistanceT2, 0f, 0.5f, 60f, 127f);
            MidiTouch04.GetComponent<NoteTrigger>().SendCC = true;
        }
        else
        {
            MidiTouch04.GetComponent<NoteTrigger>().SendCC = false;
        }

    }

    public void Nothing()
    {
        if (t1 ==1 )
        {
            MidiTouch01.GetComponent<NoteTrigger>().StopCoroutine("NoteOn");
            MidiTouch01.GetComponent<NoteTrigger>().StartCoroutine("Note1Off");        
        }
       // MidiTouch01.GetComponent<NoteTrigger>().StopCoroutine("MidiTrig");
        Touch01 = false;
        t1 = 0;

    }

    public void Nothing2()
    {
        if (t2 == 1)
        {
            MidiTouch02.GetComponent<NoteTrigger>().StartCoroutine("Note2Off");
        }
        // MidiTouch02.GetComponent<NoteTrigger>().StopCoroutine("MidiTrig");
        Touch02 = false;
        t2 = 0;
    }

    public void Nothing4()
    {
        if (t3 == 1)
        {
            MidiTouch04.GetComponent<NoteTrigger>().StopCoroutine("Note4On");
            MidiTouch04.GetComponent<NoteTrigger>().StartCoroutine("Note4Off");
        }
        // MidiTouch02.GetComponent<NoteTrigger>().StopCoroutine("MidiTrig");
        Touch03 = false;
        t3 = 0;
    }
    public void Touch1()
    {
        ///////////////////// Premier Touch Declenche Le trigger du GO MidisoundControl associé au Touch01
        if (t1 == 0)
        {
            MidiTouch01.GetComponent<NoteTrigger>().StartCoroutine("Note1On");
        }
        t1++;
        Touch01 = true;
       // Debug.Log("Touch1");
    }

    public void Touch2()
    {
        if (t2 == 0)
        {
            MidiTouch02.GetComponent<NoteTrigger>().StartCoroutine("MidiTrig");
        }
        Touch02 = true;
        t2++;        
    }

    public void Touch3()
    {
        if (t3 == 0 )
        {
           // MidiTouch03.GetComponent<NoteTrigger>().StartCoroutine("MidiTrig");
            MidiTouch04.GetComponent<NoteTrigger>().StartCoroutine("Note4On");          
        }
        Touch03 = true;
        t3++;
       // Debug.Log("Touch3");
    }

    public void SonRouge()
    {

            MidiTouchSound.GetComponent<NoteTrigger>().StartCoroutine("MidiTrig");
        
    //Sound = true;
    }

    public void DoubleTouchDist()
    {

      
    }


}
