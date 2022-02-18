using System;
using System.Collections;
using UnityEngine;

public class SoundManager_Junkrat : MonoBehaviour
{
    [SerializeField] private AudioClip _beepClip;
    [SerializeField] private float _beepTime;
    [SerializeField] private AudioClip _grenadeShootClip;
    [SerializeField] private AudioClip _grenadeExplodeClip;
    [SerializeField] private AudioClip _mineExplodeClip;

    private static SoundManager_Junkrat _instance;

    public static SoundManager_Junkrat Instance
    {
        get { return _instance; }
    }

    [SerializeField] AudioSource _beepSource;
    [SerializeField] AudioSource _grenadeShootSource;
    [SerializeField] AudioSource _grenadeExplodeSource;
    [SerializeField] AudioSource _mineExplodeSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Update()
    {

    }

    IEnumerator BeepTwiceCoroutine()
    {
        PlayBeepOnce();

        //float beepClipLength = _beepClip.length;

        yield return new WaitForSeconds(_beepTime);
        
        PlayBeepOnce();
    }

    public void PlayBeepOnce()
    {
        _beepSource.Stop();

        _beepSource.clip = _beepClip;
        _beepSource.Play();
    }

    public void PlayBeepTwice()
    {
        StartCoroutine(BeepTwiceCoroutine());
    }
    
    public void PlayGrenadeShoot()
    {
        _grenadeShootSource.clip = _grenadeShootClip;
        _grenadeShootSource.Play();
    }
    
    public void PlayGrenadeExplode()
    {
        _grenadeExplodeSource.clip = _grenadeExplodeClip;
        _grenadeExplodeSource.Play();
    }

    public void PlayMineExplode()
    {
        _mineExplodeSource.clip = _mineExplodeClip;
        _mineExplodeSource.Play();
    }
}