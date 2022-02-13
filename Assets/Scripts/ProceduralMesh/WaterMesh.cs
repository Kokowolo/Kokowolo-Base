using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMesh : ProceduralMesh
{
    [SerializeField, Min(0)] float lengthX;
    [SerializeField, Min(0)] float lengthZ;
    [SerializeField, Min(1)] int numberOfQuadsX;
    [SerializeField, Min(1)] int numberOfQuadsZ;
    
    private void Start()
    {
        this.TriangulateQuad(lengthX, lengthZ, numberOfQuadsX, numberOfQuadsZ);
    }
}
