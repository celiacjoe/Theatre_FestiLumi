using UnityEngine;
using System.Collections;

public class NoteTriggerV2 : MonoBehaviour
{
    public string Titre;
    [Header("Channel")]
    public MidiChannel channel = MidiChannel.Ch1;
    [Header("Note")]
    public int noteNumber = 48;
    [HideInInspector] float velocity = 0.9f;
    public float delay = 2.0f;
    public float length = 0.1f;
    public float interval = 1.0f;

    [Header("SignalContinue")]
    public bool SendCC = false;
    public int controlNumber = 48;
    [Range(0.0f, 127f)]
    public float value = 1.0f;
    public int IntervalValue = 200;
  //  [Header("Debug")]
  //  public GameObject SphereDebug;

    [Header("KinectStuff")]
  //  public bool KinectModeOK = false;
 //   public SPT_KinectDetection S_KinectPoint;
    //public pospt S_KinectPoint;
    public SPT_EventTrigV2 S_Event;
    //public bool Touch_1 = false;
   // public bool Touch_2 = false;
   
        void Start ()
        {
        //  MidiBridge.instance.Warmup();

        /*    if (delay > 0.0f) {
                yield return new WaitForSeconds (delay);
            }
            
          Debug.Log("CoroutineWorks");

                  MidiOut.SendNoteOn (channel, noteNumber, velocity);
                  scale = 2.0f;
                  yield return new WaitForSeconds (length);
                  MidiOut.SendNoteOff (channel, noteNumber);
                  yield return new WaitForSeconds (interval - length);
      */
          }
       
        void Update()
        {

        if (SendCC)
        {
            interval = IntervalValue;
            MidiBridge.instance.Warmup();
            MidiOut.SendControlChange(channel, controlNumber, value);
        }

        }


    public IEnumerator Note1On()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
        yield return null;
    }
    public IEnumerator Note1Off()
    {
        Debug.Log("NoteOff");
        
        MidiOut.SendNoteOff(channel, noteNumber);
        Debug.Log("Note1Off__OK");
        yield return null;
    }

    public IEnumerator Note2On()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
        yield return null;
    }
    public IEnumerator Note2Off()
    {
        MidiOut.SendNoteOff(channel, noteNumber);
       // Debug.Log("Note2Off__OK");
        yield return null;
    }

    public IEnumerator Note3On()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
        yield return null;
    }
    public IEnumerator Note3Off()
    {
        MidiOut.SendNoteOff(channel, noteNumber);
       // Debug.Log("Note3Off__OK");
        yield return null;
    }


    public IEnumerator MidiTrig()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
       // Debug.Log(" CoroutineOK ");
       // scale = 2.0f;
        yield return new WaitForSeconds(length);
        MidiOut.SendNoteOff(channel, noteNumber);
    }


    
/*
    public void SendNoteC1()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
    }

    public void SendNoteOffC1()
    {
        MidiOut.SendNoteOff(channel, noteNumber);
    } */
         
}
