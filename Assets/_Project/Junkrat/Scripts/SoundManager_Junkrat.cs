using System;
using System.Collections;
using UnityEngine;

public class SoundManager_Junkrat : MonoBehaviour
{
    [SerializeField] private AudioClip _beepClip;
    [SerializeField] private float _beepTime;
    [SerializeField] private AudioClip _grenadeShootClip;

    private static SoundManager_Junkrat _instance;

    public static SoundManager_Junkrat Instance
    {
        get { return _instance; }
    }

    [SerializeField] AudioSource _beepSource;
    [SerializeField] private AudioSource _grenadeShootSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void PlayGrenadeShoot()
    {
        _grenadeShootSource.clip = _grenadeShootClip;
        _grenadeShootSource.Play();
    }
}