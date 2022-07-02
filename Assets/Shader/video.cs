using UnityEngine;

public class video : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    RenderTexture C;
    RenderTexture D;
    public Material material;
    int handle_main;
    public Texture vid;
    public Texture vidSecour;
    public float _c1;
    public float _c2;
    public bool Normal = true;
    public bool Secour = false;
    public GameObject GOSecour;


    void Start()
    {
       
        A = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        B.enableRandomWrite = true;
        B.Create();
        C = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        C.enableRandomWrite = true;
        C.Create();
        D = new RenderTexture(Display.main.systemWidth, Display.main.systemHeight, 0);
        D.enableRandomWrite = true;
        D.Create();
        handle_main = compute_shader.FindKernel("CSMain");
    }

    void Update()
    {
        
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader3", C);
        // compute_shader.SetTexture(handle_main, "reader2", vid);
        if (Input.GetKeyDown("r"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown("space"))
            {
           // GOSecour.active =  !GOSecour.active ;
            Normal = false;
            Secour = true;
            }

        if (Input.GetKeyDown("return"))
        {
            Secour = false;
            Normal = true;
        }

        if (Secour == true)
        {
            GOSecour.active = true;
            compute_shader.SetTexture(handle_main, "reader2", vidSecour);
        }

        if (Normal == true)
        { 
            GOSecour.active = false;
            compute_shader.SetTexture(handle_main, "reader2", vid);
        }

        compute_shader.SetFloat("_time",Time.time);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetFloat("_c2", _c2);
        compute_shader.SetFloat("_r1", Display.main.systemWidth);
        compute_shader.SetFloat("_r2", Display.main.systemHeight);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.SetTexture(handle_main, "writer3", D);
        compute_shader.Dispatch(handle_main, A.width / 8, A.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.SetTexture(handle_main, "reader3", D);
        compute_shader.SetTexture(handle_main, "writer3", C);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        material.SetTexture("_texf", B);
    }
}