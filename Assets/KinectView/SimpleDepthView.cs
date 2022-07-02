using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class SimpleDepthView : MonoBehaviour
{

    public GameObject depthSourceManager;
    private DepthSourceManager depthSourceManagerScript;
    public float test;
    Texture2D texture;
    byte[] depthBitmapBuffer;
    FrameDescription depthFrameDesc;
   // public RenderTexture buff;
    public float scale = 1.0f;
   // public RenderTexture rd;

    void Start()
    {
        // Get the description of the depth frames.
        depthFrameDesc = KinectSensor.GetDefault().DepthFrameSource.FrameDescription;

        // get reference to DepthSourceManager (which is included in the distributed 'Kinect for Windows v2 Unity Plugin zip')
        depthSourceManagerScript = depthSourceManager.GetComponent<DepthSourceManager>();

        // allocate.
        depthBitmapBuffer = new byte[depthFrameDesc.LengthInPixels * 3];
        texture = new Texture2D(depthFrameDesc.Width, depthFrameDesc.Height, TextureFormat.RGB24, false);

        // arrange size of gameObject to be drawn
        //gameObject.transform.localScale = new Vector3(scale * depthFrameDesc.Width / depthFrameDesc.Height, scale, 1.0f);
    }

    void Update()
    {
        updateTexture();
         gameObject.GetComponent<Renderer>().material.mainTexture = texture;
       // gameObject.GetComponent<maximumScript>().inputTexture = texture;


    }

  
    void updateTexture()
    {
        // get new depth data from DepthSourceManager.
        ushort[] rawdata = depthSourceManagerScript.GetData();

        // convert to byte data (
        for (int i = 0; i < rawdata.Length; i++)
        {
            
            byte value = (byte)(rawdata[i] * scale);
           // byte value2 = (byte)255;
            if (value < 0.05f) { value = 0; value = 255; }
            int colorindex = i * 3;
            depthBitmapBuffer[colorindex + 0] = value;
            depthBitmapBuffer[colorindex + 1] = value;
            depthBitmapBuffer[colorindex + 2] = value;
        }
        
         
        // make texture from byte array
        texture.LoadRawTextureData(depthBitmapBuffer);
        texture.Apply();
    }
}