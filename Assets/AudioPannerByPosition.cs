using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPannerByPosition : MonoBehaviour
{

    private AudioSource _thisSource;
    private WaitForSeconds _wait01 = new(0.1f);
    private bool _alive = true;

    private void Awake()
    {
        _thisSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(UpdatePanningPosition());
    }

    // checks player world position on X axis against this soundclip. And takes max distance into account. Pans accordingly.
    // UPDATED: also added volume, because this thing can't do it or somethin
    private IEnumerator UpdatePanningPosition()
    {
        float maxDis = _thisSource.maxDistance;
        Vector3 pos = transform.position;
        float xPosDifference;
        float distance;
        float volume;
        
        do
        {
            xPosDifference = PlayerController.Instance.transform.position.x - pos.x;
            distance = Vector3.Distance(PlayerController.Instance.transform.position, pos);

            _thisSource.panStereo = xPosDifference / maxDis * -1 * 2;

            volume = 1 - (distance / maxDis);
            _thisSource.volume = Mathf.Clamp(volume, 0f, 1f);

            yield return _wait01;
        } while (_alive);
    }
}