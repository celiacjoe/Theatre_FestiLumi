using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class pospt1 : MonoBehaviour
{
    public GameObject DepthSourceManager;
    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;
    Texture2D texture;
    //public Material mat;
    byte[] depthBitmapBuffer;
    public float scale = 1.0f;
    private const int _DownsampleSize = 4;
    private const double _DepthScale = 0.1f;
    private const int _Speed = 50;
    private DepthSourceManager _DepthManager;


    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            depthBitmapBuffer = new byte[frameDesc.LengthInPixels * 3];
            texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGB24, false);
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
        
    }

    
    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.EndGroup();
    }

    void Update()
    {

        if (_Sensor == null)
        {
            return;
        }

            if (DepthSourceManager == null)
            {
                return;
            }
            
            _DepthManager = DepthSourceManager.GetComponent<DepthSourceManager>();
            if (_DepthManager == null)
            {
                return;
            }

        updateTexture();

        gameObject.GetComponent<dete>().C = texture;
        

    }
    

   
    void updateTexture()
    {
        // get new depth data from DepthSourceManager.
        ushort[] rawdata = _DepthManager.GetData();

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
    void OnApplicationQuit()
    {
        if (_Mapper != null)
        {
            _Mapper = null;
        }
        
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}
