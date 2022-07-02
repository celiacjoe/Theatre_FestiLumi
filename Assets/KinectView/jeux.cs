using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jeux : MonoBehaviour {
    private Vector3[] Vertices;
    public float po;
    public float pox;
    public float poy;

    void Start () {
        Vertices = new Vector3[4];
        Vertices[0] = new Vector3(0.6f, 0.2f, 0.9f);
        Vertices[1] = new Vector3(0.8f, 0.1f, 0.4f);
        Vertices[2] = new Vector3(0.6f, 0.8f, 0.6f);
        Vertices[3] = new Vector3(0.1f, 0.5f, 0.1f);
    }
	
	void Update () {
        po =Vertices[0].z;

        for (int i = 1; i < Vertices.Length; i++)
        {

            if (Vertices[i].x > 0.5f)
            {
                if (Vertices[i].y > 0.5f)
                {
                    if (Vertices[i].z < po)
                    {
                        po = Vertices[i].z;
                        pox = Vertices[i].x;
                        poy = Vertices[i].y;
                    }
                }
            }
        } 
    }
}

