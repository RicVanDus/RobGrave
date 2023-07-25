using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]

public class HideColliders : MonoBehaviour
{
    private bool _visible = false;
    
    public void ToggleColliders()
    {
        if (_visible)
        {
            Debug.Log("[colliders not visible]");
            _visible = false;
            ToggleAllMeshRenderers(false);
        }
        else
        {
            _visible = true;
            Debug.Log("[colliders visible]");
            ToggleAllMeshRenderers(true);
        }
    }

    private void ToggleAllMeshRenderers(bool toggle)
    {
        var children =  transform.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer child in children)
        {
            child.enabled = toggle;
        }
    }
}

[ExecuteAlways]

[CustomEditor(typeof(HideColliders))]
public class ColliderCreatorEditor : Editor {
   
    override public void  OnInspectorGUI ()
    {
        HideColliders hideColliders = (HideColliders)target;
        if(GUILayout.Button("Toggle Colliders"))
        {
            hideColliders.ToggleColliders();
        }
        DrawDefaultInspector();
    }
}
