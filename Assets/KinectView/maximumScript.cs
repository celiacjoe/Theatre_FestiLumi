using UnityEngine;

public class maximumScript : MonoBehaviour
{
    public ComputeShader shader;
    public Texture2D inputTexture;

    public uint[] groupMaxData;
    public int groupMax;

    private ComputeBuffer groupMaxBuffer;

    private int handleMaximumMain;

    void Start()
    {
        if (null == shader || null == inputTexture)
        {
            Debug.Log("Shader or input texture missing.");
            return;
        }

        handleMaximumMain = shader.FindKernel("MaximumMain");
        groupMaxBuffer = new ComputeBuffer((inputTexture.height + 63) / 64, sizeof(uint) * 3);
        groupMaxData = new uint[((inputTexture.height + 63) / 64) * 3];

        if (handleMaximumMain < 0 || null == groupMaxBuffer || null == groupMaxData)
        {
            Debug.Log("Initialization failed.");
            return;
        }

        shader.SetTexture(handleMaximumMain, "InputTexture", inputTexture);
        shader.SetInt("InputTextureWidth", inputTexture.width);
        shader.SetBuffer(handleMaximumMain, "GroupMaxBuffer", groupMaxBuffer);
    }

    void OnDestroy()
    {
        if (null != groupMaxBuffer)
        {
            groupMaxBuffer.Release();
        }
    }

    void Update()
    {
        shader.Dispatch(handleMaximumMain, (inputTexture.height + 63) / 64, 1, 1);
        // divided by 64 in x because of [numthreads(64,1,1)] in the compute shader code
        // added 63 to make sure that there is a group for all rows

        // get maxima of groups
        groupMaxBuffer.GetData(groupMaxData);

        // find maximum of all groups
        groupMax = 0;
        for (int group = 1; group < (inputTexture.height + 63) / 64; group++)
        {
            if (groupMaxData[3 * group + 2] > groupMaxData[3 * groupMax + 2])
            {
                groupMax = group;
            }
        }
    }
}