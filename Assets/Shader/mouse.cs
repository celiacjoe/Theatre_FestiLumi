using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour {

    public float mousepox;
    public float mousepoy;
    public GameObject cam;
    void Start () {
		
	}

	void Update () {

        mousepox = Mathf.Clamp(  Input.mousePosition.x/ Display.main.systemWidth,0.0f,1.0f);
        mousepoy = Mathf.Clamp(  Input.mousePosition.y/ Display.main.systemHeight, 0.0f,1.0f);
        gameObject.GetComponent<ProceduralAudioController>().mainFrequency = 50.0f+75.0f * mousepox;
        gameObject.GetComponent<ProceduralAudioController>().frequencyModulationOscillatorIntensity = mousepoy*100.0f;
        gameObject.GetComponent<ProceduralAudioController>().masterVolume = 1.0f+(1.0f-mousepoy) ;
        gameObject.GetComponent<NReverb>().wetMix =mousepoy *0.5f;
        //gameObject.GetComponent<sons>().frequency = 500.0f + (mousepox * 1000.0f);
       // gameObject.GetComponent<sons>().gain = mousepoy ;

    }
}
