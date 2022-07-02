using UnityEngine;

public class fluid_dynamics : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    public Material material;
    public Texture tex;
    int handle_main;
    public bool Red;
    public float _p1;
    public float _p2;
    public float _p3;
    public float _c1;
    public float _c2;
    public float _c3;
    public float _c4;
    public float _lh;
    public float _lb;
    public float _lg;
    public float _ld;
    public float _maxdist;
    float frac(float s) { return s - Mathf.Floor(s); }
    float rand(float s) { return frac(Mathf.Sin(Vector2.Dot(new Vector2(Mathf.Floor(s), 0.0f), new Vector2(12.45f, 0.0f))) * 4597.268f); }
    void Start()
    {
        A = new RenderTexture(1920, 1080, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(1920,  1080, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
        compute_shader.SetFloat("_lh", _lh);
        compute_shader.SetFloat("_lb", _lb);
        compute_shader.SetFloat("_lg", _lg);
        compute_shader.SetFloat("_ld", _ld);
        compute_shader.SetFloat("_maxdist", _maxdist);
    }

    void Update()
    {
        float rd = rand(Time.time * 8.0f);
        if (rd > 0.9f)
        {
            Red = true;
        }
        else
        {
            Red = false;
        }
        /*compute_shader.SetFloat("_lh", _lh);
        compute_shader.SetFloat("_lb", _lb);
        compute_shader.SetFloat("_lg", _lg);
        compute_shader.SetFloat("_ld", _ld);  */
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", tex);
        compute_shader.SetFloat("_r1",1920);
        compute_shader.SetFloat("_r2", 1080);
        compute_shader.SetFloat("_p1", _p1 );
        compute_shader.SetFloat("_p2", _p2);
        compute_shader.SetFloat("_p3", _p3);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetFloat("_c2", _c2);
        compute_shader.SetFloat("_c3", _c3);
        compute_shader.SetFloat("_c4", _c4);
        compute_shader.SetFloat("_time",Time.time);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, A.width / 8, A.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        material.SetTexture("_texf", B);
        material.SetFloat("_t", rd);
    }
}