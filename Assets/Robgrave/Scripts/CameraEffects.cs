using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class CameraEffects : RenderObjects
{
    public Material _mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
