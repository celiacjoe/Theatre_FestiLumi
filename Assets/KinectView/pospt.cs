using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class pospt : MonoBehaviour
{
    public GameObject DepthSourceManager;
    public float po;
    public float pox;
    public float poy;
    public float po2;
    public float pox2;
    public float poy2;
    public float h;
    public float w;
    public float dist;
    public Material mat;
    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;
    private Vector3[] _Vertices;
    Texture2D texture;
    byte[] depthBitmapBuffer;
    public float scale = 1.0f;
    public float scale2 = 1.0f;
    private const int _DownsampleSize = 4;
    private const double _DepthScale = 0.1f;
    private const int _Speed = 50;
    private DepthSourceManager _DepthManager;
    public float lh;
    public float lb;
    public float lg;
    public float ld;
    public bool Coucou;

    void Start()
    {
        gameObject.GetComponent<fluid_dynamics>()._lh = lh;
        gameObject.GetComponent<fluid_dynamics>()._lb = lb;
        gameObject.GetComponent<fluid_dynamics>()._lg = lg;
        gameObject.GetComponent<fluid_dynamics>()._ld = ld;
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            h = frameDesc.Height / _DownsampleSize;
            w = frameDesc.Width / _DownsampleSize;
            // Downsample to lower resolution
            CreateMesh(frameDesc.Width / _DownsampleSize, frameDesc.Height / _DownsampleSize);
            depthBitmapBuffer = new byte[frameDesc.LengthInPixels * 3];
            texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGB24, false);
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
        
    }

    void CreateMesh(int width, int height)
    { 
        _Vertices = new Vector3[width * height];

        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;

                _Vertices[index] = new Vector3(x, y, 0);               
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
                       
       /* float yVal = Input.GetAxis("Horizontal");
        float xVal = -Input.GetAxis("Vertical");

        transform.Rotate(
            (xVal * Time.deltaTime * _Speed), 
            (yVal * Time.deltaTime * _Speed), 
            0, 
            Space.Self); */

            if (DepthSourceManager == null)
            {
                return;
            }
            
            _DepthManager = DepthSourceManager.GetComponent<DepthSourceManager>();
            if (_DepthManager == null)
            {
                return;
            }
            
        RefreshData(_DepthManager.GetData());
        //dist = 1000.0f;
        po =1000.0f;
        po2 = 1000.0f;
        for (int i = 1; i < _Vertices.Length; i++)
        {
            //&& _Vertices[i].y > lh*h && _Vertices[i].y < lb*h &&  _Vertices[i].x > lg*w &&  _Vertices[i].x < ld*w
            if (_Vertices[i].x > ld * w)
            {
                if (_Vertices[i].x < lg * w)
                {
                    if (_Vertices[i].y > lb * h)
                    {
                        if (_Vertices[i].y < lh * h)
                        {
                            

                            if (_Vertices[i].z < po )
                            {
                               // Coucou = true;
                                po = _Vertices[i].z;
                                pox = _Vertices[i].x;
                                poy = _Vertices[i].y;
                               
                                
                            }  
                           
                             
                            
                        }
                    }
                }
            }
        }
        pox2 = 0.0f;
        poy2 = 0.0f;
            for (int j = 1; j < _Vertices.Length; j++)
            {
                //&& _Vertices[i].y > lh*h && _Vertices[i].y < lb*h &&  _Vertices[i].x > lg*w &&  _Vertices[i].x < ld*w
                if (_Vertices[j].x > ld * w)
                {
                    if (_Vertices[j].x < lg * w)
                    {
                        if (_Vertices[j].y > lb * h)
                        {
                            if (_Vertices[j].y < lh * h)
                            {
                            
                                if (_Vertices[j].z < po2 && Vector2.Distance(new Vector2(pox, poy), new Vector2(_Vertices[j].x, _Vertices[j].y)) > 30.0f
                                && _Vertices[j].z < 150.0f)
                                {

                                    Coucou = true;
                                    po2 = _Vertices[j].z;
                                    pox2 = _Vertices[j].x;
                                    poy2 = _Vertices[j].y;
                                }
                                

                            }
                        }
                    }
                }   

            }  
        updateTexture();
        mat.SetFloat("_p1", 1.0f-pox/w);
        mat.SetFloat("_p2", 1.0f-poy/h);
       mat.SetFloat("_p3", 1.0f - pox2 / w);
        mat.SetFloat("_p4", 1.0f - poy2 / h);
        mat.SetTexture("_texsec" , texture); 
        //gameObject.GetComponent<fluid_dynamics>()._p1 = 1.0f - pox / w;
        //gameObject.GetComponent<fluid_dynamics>()._p2 = 1.0f - poy / h;
        //gameObject.GetComponent<fluid_dynamics>()._p3 = po;
        //gameObject.GetComponent<fluid_dynamics>().tex = texture;



    }
    
    private void RefreshData(ushort[] depthData)
    {
        var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
    
        for (int y = 0; y < frameDesc.Height; y += _DownsampleSize)
        {
            for (int x = 0; x < frameDesc.Width; x += _DownsampleSize)
            {
                int indexX = x / _DownsampleSize;
                int indexY = y / _DownsampleSize;
                int smallIndex = (indexY * (frameDesc.Width / _DownsampleSize)) + indexX;
                
                double avg = GetAvg(depthData, x, y, frameDesc.Width, frameDesc.Height);
                
                avg = avg * _DepthScale;
                
                _Vertices[smallIndex].z = (float)avg;
                
                // Update UV mapping with CDRP
               
            }
        }     
    }
    
    private double GetAvg(ushort[] depthData, int x, int y, int width, int height)
    {
        double sum = 0.0;
        
        for (int y1 = y; y1 < y + 4; y1++)
        {
            for (int x1 = x; x1 < x + 4; x1++)
            {
                int fullIndex = (y1 * width) + x1;
                
                if (depthData[fullIndex] == 0)
                    sum += 4500;
                else
                    sum += depthData[fullIndex];
                
            }
        }
        return sum / 16;
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
