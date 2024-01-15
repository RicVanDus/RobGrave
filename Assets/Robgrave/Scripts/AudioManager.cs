using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _soundEffects;
    [SerializeField] private GameObject _spatialSndEffect;

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

    public void PlayMusic(AudioClip music)
    {
        _music.loop = true;
        _music.PlayOneShot(music);
    }

    public void PlaySoundEffect(AudioClip sound)
    {
        _soundEffects.PlayOneShot(sound);
    }

    public void PlayRandomSoundEffect(AudioClip[] sound)
    {
        int rndIndex = Random.Range(0, sound.Length-1);
        
        PlaySoundEffect(sound[rndIndex]);
    }

    public void PlaySpatialSoundEffect(AudioClip sound, Vector3 position, bool randomPitch, float maxDistance)
    {
        var obj = Instantiate(_spatialSndEffect, position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();
        float rndPitch = Random.Range(-1.5f, 1.5f);
        
        if (randomPitch) source.pitch = rndPitch;
        source.clip = sound;
        source.maxDistance = maxDistance;
        
        source.Play();
        Destroy(obj, source.clip.length);
    }

    public void PlayRandomSpatialSoundEffect(AudioClip[] sound, Vector3 position, bool randomPitch, float maxDistance)
    {
        int rndIndex = Random.Range(0, sound.Length);
        
        PlaySpatialSoundEffect(sound[rndIndex], position, randomPitch, maxDistance);
    }
}
