using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraformableMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Deform(RaycastHit hit , float radius = 1f)
    {
        Vector2 coord = hit.textureCoord;

        //hit.
        //RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
    }
}
