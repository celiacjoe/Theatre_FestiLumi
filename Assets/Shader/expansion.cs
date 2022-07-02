using UnityEngine;

public class expansion : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    public Material material;
    int handle_main;
    public float v1;
    public float v2;
    public float v3;
    public float v4;
    public float v5;
    public float v6;
    public Texture tex;
    void Start()
    {
        A = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
    }

    void Update()
    {
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "texsec",tex);
        compute_shader.SetFloat("_r1", Display.main.systemWidth);
        compute_shader.SetFloat("_r2", Display.main.systemHeight);
        compute_shader.SetFloat("_p1", Input.mousePosition.x);
        compute_shader.SetFloat("_p2", Input.mousePosition.y);
        compute_shader.SetFloat("_b1", v1);
        compute_shader.SetFloat("_b2", v2);
        compute_shader.SetFloat("_b3", v3);
        compute_shader.SetFloat("_b4", v4);
        compute_shader.SetFloat("_b5", v5);
        compute_shader.SetFloat("_b6", v6);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, A.width / 8, A.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        material.SetTexture("_texf", B);
        //gameObject.GetComponent<blurEffet>().SetTexture("tex", B);
        
    }
}