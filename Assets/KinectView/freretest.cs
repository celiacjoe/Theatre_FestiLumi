
using UnityEngine;

public class freretest : MonoBehaviour {
    public ComputeShader compute_shader;
    public Texture2D inputTexture;
    public Vector2 pos; 
    int handle_main;
    void Start () {
        handle_main = compute_shader.FindKernel("CSMain");
    }
	void Update () {
        compute_shader.SetTexture(handle_main, "reader", inputTexture);
        compute_shader.SetTexture(handle_main, "result", inputTexture);
        compute_shader.Dispatch(handle_main, 1, 1, 1);
        //compute_shader.GetData(Result);
        pos.Set(0.5f,0.3f);    
       
    }
}
