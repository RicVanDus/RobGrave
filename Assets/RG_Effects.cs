using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.VFX;

public class RG_Effects : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private VisualEffect _dirtFromShovel;
    [SerializeField] private GameObject _dirtOnShovel;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        HideDirtOnShovel();
    }

    [UsedImplicitly]
    private void ShowDirtFromShovel()
    {
        StartCoroutine(ToggleDirtFromShovel());
    }

    [UsedImplicitly]
    private void ShowDirtOnShovel()
    {
        _dirtOnShovel.SetActive(true);
    }
    
    [UsedImplicitly]
    private void HideDirtOnShovel()
    {
        _dirtOnShovel.SetActive(false);
    }
    
    private IEnumerator ToggleDirtFromShovel()
    {
        _dirtFromShovel.Play();

        yield return new WaitForSeconds(0.3f);
        
        _dirtFromShovel.Stop();
    }
}
