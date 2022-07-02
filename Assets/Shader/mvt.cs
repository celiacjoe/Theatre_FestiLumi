using UnityEngine;

public class mvt : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    RenderTexture C;
    RenderTexture D;
    RenderTexture E;
    RenderTexture F;
    public Material material;
    int handle_main;
    public float _p1;
    public float _p2;
    public float _v1;
    public float _v2;
    public float _lh;
    public float _lb;
    public float _lg;
    public float _ld;
    void Start()
    {
        A = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(Display.main.systemWidth,  Display.main.systemHeight, 0);
        B.enableRandomWrite = true;
        B.Create();
        C = new RenderTexture(1,1, 0);
        C.enableRandomWrite = true;
        C.Create();
        D = new RenderTexture(1, 1, 0);
        D.enableRandomWrite = true;
        D.Create();
        E = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        E.enableRandomWrite = true;
        E.Create();
        F = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        F.enableRandomWrite = true;
        F.Create();
        handle_main = compute_shader.FindKernel("CSMain");
        compute_shader.SetFloat("_lh", _lh);
        compute_shader.SetFloat("_lb", _lb);
        compute_shader.SetFloat("_lg", _lg);
        compute_shader.SetFloat("_ld", _ld);
    }

    void Update()
    {
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.SetTexture(handle_main, "reader2", C);
        compute_shader.SetTexture(handle_main, "writer2", D);
        compute_shader.SetTexture(handle_main, "reader3", E);
        compute_shader.SetTexture(handle_main, "writer3", F);
        compute_shader.SetFloat("_r1", Display.main.systemWidth);
        compute_shader.SetFloat("_r2", Display.main.systemHeight);
        compute_shader.SetFloat("_p1", Input.mousePosition.x / Display.main.systemWidth);
        compute_shader.SetFloat("_p2", Input.mousePosition.y / Display.main.systemHeight);
        //compute_shader.SetFloat("_p1", _p1);
        //compute_shader.SetFloat("_p2", _p2);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.Dispatch(handle_main, A.width / 8, A.height / 8, 1);
        //compute_shader.Dispatch(handle_main, 1, 1, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.SetTexture(handle_main, "reader3", F);
        compute_shader.SetTexture(handle_main, "writer3", E);
        compute_shader.Dispatch(handle_main, 1, 1, 1);
        compute_shader.SetTexture(handle_main, "reader2", D);
        compute_shader.SetTexture(handle_main, "writer2", C);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        //compute_shader.Dispatch(handle_main, 1, 1, 1);
        material.SetTexture("_texf", F);
        material.SetTexture("_texf2", B);

    }
}