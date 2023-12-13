using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _soundEffects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void PlaySound(AudioClip sound)
    {
        _soundEffects.PlayOneShot(sound);
    }
}
