using System;
using System.Collections;
using UnityEngine;

public class SoundManager_Junkrat : MonoBehaviour
{
    [SerializeField] private AudioClip _beepClip;
    [SerializeField] private float _beepTime;

    private static SoundManager_Junkrat _instance;

    public static SoundManager_Junkrat Instance
    {
        get { return _instance; }
    }

    public AudioSource beepSource;

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
        beepSource.Stop();

        beepSource.clip = _beepClip;
        beepSource.Play();
    }

    public void PlayBeepTwice()
    {
        StartCoroutine(BeepTwiceCoroutine());
    }
}