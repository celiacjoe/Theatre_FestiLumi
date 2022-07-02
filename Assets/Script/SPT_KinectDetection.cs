using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class SPT_KinectDetection : MonoBehaviour
{
    public GameObject DepthSourceManager;
    public Material Mat;
    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;
    private Vector3[] _Vertices;
    Texture2D texture;
    byte[] depthBitmapBuffer;
    /// Coordonnées Points 01
    public float PosZ1;
    public float PosX1;
    public float PosY1;
    private float PosX1b;
    private float PosY1b;
    private float PosX1c;
    private float PosY1c;
    /// Coordonnées Points 02
    public float PosZ2 ;
    public float PosX2 ;
    public float PosY2 ;
    public float H;
    public float W;
    public float Dist;
    public float DistPoint2;
    /// CoordonnéesValue condition trigger event
    private float t1 = 0;
    private float t2 = 0;
    public float scale = 1.0f;
    public float scale2 = 0.0f;
    private const int _DownsampleSizex =16;
    private const int _DownsampleSizey =14;
    private const double _DepthScale = 0.1f;
    private const int _Speed = 50;
    private DepthSourceManager _DepthManager;
    /// Width & Height zone canvas
    public float lh;
    public float lb;
    public float lg;
    public float ld;
    //public bool Coucou;
    public SPT_EventTrigV2 KinectEvent;
    private Vector2 g;
    private float vtot;
    private float Lh;
    private float Lb;
    private float Lg;
    private float Ld;
    float XVelocity = 0.0f;
    float YVelocity = 0.0f;

    public float distanceMinimum = 130;
    public float distanceMaximum = 150;
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
    void Start()
    {
        // KinectEvent.Touch1();
        

        gameObject.GetComponent<fluid_dynamics>()._lh = lh;
        gameObject.GetComponent<fluid_dynamics>()._lb = lb;
        gameObject.GetComponent<fluid_dynamics>()._lg = lg;
        gameObject.GetComponent<fluid_dynamics>()._ld = ld;
        g = new Vector2(0.0f, 0.0f);
        vtot = 0.0f;
    _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            H = frameDesc.Height / _DownsampleSizey;
            W = frameDesc.Width / _DownsampleSizex;
             Lh = lh * H; ;
             Lb = lb * H;
             Lg = lg * W;
             Ld = ld * W;
            // Downsample to lower resolution
            liste(frameDesc.Width / _DownsampleSizex, frameDesc.Height / _DownsampleSizey);
            depthBitmapBuffer = new byte[frameDesc.LengthInPixels * 3];
            texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGB24, false);
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }        
    }

    void liste(int width, int height)
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
    
    /*void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.EndGroup();
    }    */

    void Update()
    {
       /* compute_shader.SetFloat("_lh", _lh);
        compute_shader.SetFloat("_lb", _lb);
        compute_shader.SetFloat("_lg", _lg);
        compute_shader.SetFloat("_ld", _ld); */
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
            
        RefreshData(_DepthManager.GetData());

        PosZ1 =1000.0f;
        
        g = new Vector2(0.0f, 0.0f);
        vtot = 0.0f;
        for (int i = 1; i < _Vertices.Length; i++)
       
        {
            if (_Vertices[i].x >Ld)
            {
                if (_Vertices[i].x < Lg)
                {
                    if (_Vertices[i].y > Lb)
                    {
                        if (_Vertices[i].y < Lh)
                        {
                            if(_Vertices[i].z > distanceMinimum && _Vertices[i].z < distanceMaximum) { 
                                float nv = Mathf.Clamp01(map(_Vertices[i].z, distanceMaximum , distanceMinimum, 0.0f, 1.0f));
                                g += new Vector2(map(_Vertices[i].x, Ld, Lg, 1.0f, 0.0f), map(_Vertices[i].y, Lb, Lh, 1.0f, 0.0f)) * nv;
                                vtot += nv;
                                if (_Vertices[i].z < PosZ1)
                                {
                                    PosZ1 = _Vertices[i].z;
                                    PosX1c = _Vertices[i].x;
                                    PosY1c = _Vertices[i].y;
                                }
                            }
                        }
                    }
                }
            }
        }
        PosX2 = g.x/vtot;
        PosY2 = g.y/vtot;
        PosX1b = Mathf.SmoothDamp(PosX1b, PosX1c, ref XVelocity, 0.1f);
        PosY1b = Mathf.SmoothDamp(PosY1b, PosY1c, ref YVelocity, 0.1f);
        PosX1 = map(PosX1b, Ld, Lg, 1.0f, 0.0f);
        PosY1 = map(PosY1b, Lb, Lh, 1.0f, 0.0f);
        PosZ2 = Vector2.Distance(new Vector2(PosX1, PosY1), new Vector2(PosX2, PosY2));
        
        updateTexture();
        gameObject.GetComponent<fluid_dynamics>()._p1 = PosX1;
        gameObject.GetComponent<fluid_dynamics>()._p2 = PosY1;
        gameObject.GetComponent<fluid_dynamics>()._p3 = PosZ1;
        gameObject.GetComponent<fluid_dynamics>().tex = texture;    
         Mat.SetFloat("_p1", PosX1);
         Mat.SetFloat("_p2", PosY1);
         Mat.SetFloat("_p3",  PosX2 );
         Mat.SetFloat("_p4",  PosY2 );
         Mat.SetFloat("_p5", PosZ2);
         Mat.SetTexture("_texsec" , texture);  
         
       
        if (PosZ1 == 1000.0f)
        {
            KinectEvent.NoTouch_1();
        }
        if (PosZ1 < distanceMaximum)
        {
            if (t1 == 0)
            {
                KinectEvent.Touch1();
            }
            t1++;
        }
        if (PosZ1 > distanceMaximum)
        {
            if (t1 > 0)
            {
                KinectEvent.NoTouch_1();
                KinectEvent.NoTouch_12();
                KinectEvent.NoTouch_2();
            }
            t1 = 0;
        }
        if (PosZ2 > 0.1f)
        {
            if (t2 == 0)
            {
                KinectEvent.Touch3();
            }
            t2++;
            }
        if (PosZ2 < 0.1f)
        {
            if (t2 > 0)
            {
                KinectEvent.NoTouch_2();
            }
            t2 = 0;
        }
        //   if ( PosZ2 == 1000.0f) { KinectEvent.Nothing(); }        */
        if (float.IsNaN(PosZ2))
        {
            PosZ2 = 0;
            PosY2 = 0;
            PosX2 = 0;
        }


    }
    
    private void RefreshData(ushort[] depthData)
    {
        var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
    
        for (int y = 0; y < 420; y += _DownsampleSizey)
        {
            for (int x = 0; x < frameDesc.Width; x += _DownsampleSizex)
            {
                int indexX = x / _DownsampleSizex;
                int indexY = y / _DownsampleSizey;
                int v1 = indexY * (frameDesc.Width / _DownsampleSizex);
                if(v1 > 960 ) { v1 = 960; }
                int smallIndex = v1 + indexX;
               // double avg = GetAvg(depthData, x, y, frameDesc.Width, frameDesc.Height);
                double avg = depthData[(y * frameDesc.Width) + x];
                
                avg = avg * _DepthScale;
                
                _Vertices[smallIndex].z = (float )avg;
                // Update UV mapping with CDRP

            }
        }     
    }

    /* private double GetAvg(ushort[] depthData, int x, int y, int width, int height)
     {
         double sum = 0.0;

         for (int y1 = y; y1 < y + 14; y1++)
         {
             for (int x1 = x; x1 < x + 16; x1++)
             {
                 int fullIndex = (y1 * width) + x1;

                 if (depthData[fullIndex] == 0)
                     sum += 4500;
                 else
                     sum += depthData[fullIndex];

             }
         }
         return sum / 224;   

     }  */
    
    void updateTexture()
    {
        // get new depth data from DepthSourceManager.
        ushort[] rawdata = _DepthManager.GetData();

        // convert to byte data (
        for (int i = 0; i < rawdata.Length; i++)
        {

            byte value = (byte)(rawdata[i] * scale+scale2);
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
