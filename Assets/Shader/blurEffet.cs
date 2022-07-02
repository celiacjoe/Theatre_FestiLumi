using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blurEffet : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture B;
    public Material material;
    int handle_main;
    public float v1;
    public float v2;
    public float v3;
    public Texture tex ;
    void Start()
    {
       
        B = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
    }

    void Update()
    {
        //compute_shader.SetTexture(handle_main, "reader", tex,0);
        //compute_shader.Dispatch(handle_main, tex.width / 8, tex.height / 8, 1);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        material.SetTexture("_texf", B);
        //gameObject.GetComponent<blurEffet>().SetTexture("_texf", B);
    }
}
