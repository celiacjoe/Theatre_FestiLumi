using UnityEngine;
using System.Collections;

public class NoteTrigger : MonoBehaviour
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
  //  [Header("Debug")]
  //  public GameObject SphereDebug;

    [Header("KinectStuff")]
  //  public bool KinectModeOK = false;
 //   public SPT_KinectDetection S_KinectPoint;
    //public pospt S_KinectPoint;
    public SPT_EventTrig S_Event;

   // float scale;

    void Start ()
        {
          //  MidiBridge.instance.Warmup();

        /*    if (delay > 0.0f) {
                yield return new WaitForSeconds (delay);
            }
            */
      /*  Debug.Log("CoroutineWorks");

                MidiOut.SendNoteOn (channel, noteNumber, velocity);
                scale = 2.0f;
                yield return new WaitForSeconds (length);
                MidiOut.SendNoteOff (channel, noteNumber);
                yield return new WaitForSeconds (interval - length);
            */
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
        yield return null;
    }

    public IEnumerator Note4On()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
        yield return null;
    }
    public IEnumerator Note4Off()
    {
        MidiOut.SendNoteOff(channel, noteNumber);
        yield return null;
    }


    public IEnumerator MidiTrig()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
        Debug.Log(" CoroutineOK ");
       // scale = 2.0f;
        yield return new WaitForSeconds(length);
        MidiOut.SendNoteOff(channel, noteNumber);
    }


    void Update()
    {

        if (SendCC)
        {
            interval = 150;
            MidiBridge.instance.Warmup();
            MidiOut.SendControlChange(channel, controlNumber,value);
          //  scale = 2.0f;
         }

      //  scale = 1.0f - (1.0f - scale) * Mathf.Exp (Time.deltaTime * -4.0f);
      //  transform.localScale = Vector3.one * scale;
    }

   /* public void SendNoteC1()
    {
        MidiOut.SendNoteOn(channel, noteNumber, velocity);
    }

    public void SendNoteOffC1()
    {
        MidiOut.SendNoteOff(channel, noteNumber);
    } */
}
