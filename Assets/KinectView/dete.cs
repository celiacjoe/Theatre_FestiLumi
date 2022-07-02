

using UnityEngine;

public class dete : MonoBehaviour
{
    public SPT_EventTrig EventTrig;
    private float t1 = 0 ;
    private float t2 = 0;
    public ComputeShader compute_shader;
    public Texture2D C;
    Texture2D texture;
    RenderTexture B;
    RenderTexture A;
    public Material material;
    int handle_main;
    public Vector3 _p1;
    public Vector2 _p2;
    public bool _p1_active = false;
    public bool _p2_active = false;
    int count_x = 32;
    int count_y = 16;
    public float _p3;
    public float _c1;
    public float _c2;
    public float _c3;
    public float _c4;
    public float _lh;
    public float _lb;
    public float _lg;
    public float _ld;
    void Start()
    {
        A = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
        texture = new Texture2D(count_x, count_y, TextureFormat.RGB24, false);
        
        compute_shader.SetFloat("_lh", _lh);
        compute_shader.SetFloat("_lb", _lb);
        compute_shader.SetFloat("_lg", _lg);
        compute_shader.SetFloat("_ld", _ld);


    }

    void Update()
    {
        compute_shader.SetFloat("_r1", Display.main.systemWidth);
        compute_shader.SetFloat("_r2", Display.main.systemHeight);
        compute_shader.SetFloat("_delta", Time.deltaTime);
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", C);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.SetFloat("_p3", _p3);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetFloat("_c2", _c2);
        compute_shader.SetFloat("_c3", _c3);
        compute_shader.SetFloat("_c4", _c4);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.Dispatch(handle_main, A.width / 8, A.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        material.SetTexture("_texf", B);
        Rect rectReadPicture = new Rect(0, 0, count_x, count_y);
        RenderTexture.active = B;
        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();
        Vector4 col1 = texture.GetPixel(0, 0);
        Vector4 col2 = texture.GetPixel(6, 0);
        Vector4 col3 = texture.GetPixel(10, 0);
        Vector4 col4 = texture.GetPixel(14, 0);
        Vector4 col5 = texture.GetPixel(18, 0);
        _p1.x = col1.y;
        _p1.y = col2.y;
        _p1.z = col5.y;
        _p2.x = col3.y;
        _p2.y = col4.y;
        if (col1.z > 0.1f)
        {
            if (t1 == 0)
            {
                EventTrig.Touch1();
            } t1++;
        }
        if (col1.z < 0.1f)
        {
            if (t1 > 0)
            {
                EventTrig.Nothing();
                EventTrig.Nothing4();
                //  _p1_active = false;
            }
            t1 = 0;
        }
        if (col2.z > 0.1f)
        {
            EventTrig.Touch3();
            //_p2_active = true;
        }
        else
        {
            EventTrig.Nothing4();
            //_p2_active = false;
        }    
        material.SetFloat("_p1", _p1.x);
        material.SetFloat("_p2", _p1.y);
        material.SetFloat("_p3", _p2.x);
        material.SetFloat("_p4", _p2.y);
    }
} 
