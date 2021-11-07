using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessScript : MonoBehaviour
{
    public Material mat;
    [Range(0.01f, 1)]
    public float downsamplingFactor;
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        int width = Mathf.FloorToInt(src.width*downsamplingFactor);
        int height = Mathf.FloorToInt(src.height*downsamplingFactor);
        RenderTexture r = RenderTexture.GetTemporary(
			width, height, 0, src.format
		);
        r.filterMode = FilterMode.Point;

        Graphics.Blit(src, r);

        Graphics.Blit(r, dest, mat);
        RenderTexture.ReleaseTemporary(r);
    }
}
