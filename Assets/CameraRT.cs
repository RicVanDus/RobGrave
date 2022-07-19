using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRT : MonoBehaviour
{
    // Start is called before the first frame update
    public bool RenderOnlyOneFrame;
    Camera cam;


    void Start()
    {
        cam = gameObject.GetComponent<Camera>();

        cam.enabled = true;
        
    }

    private void OnRenderObject()
    {
        if (RenderOnlyOneFrame)
        {
            cam.enabled = false;
        }
    }

    private void OnPostRender()
    {
        if (RenderOnlyOneFrame)
        {
            cam.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
