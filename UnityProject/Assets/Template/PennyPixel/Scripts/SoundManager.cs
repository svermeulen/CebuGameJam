using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public AudioClip DieSound;
    public AudioClip ExitSound;
    public AudioSource Source;

    public static SoundManager Instance
    {
        get; private set;
    }

    public void Awake()
    {
        Instance = this;
    }

    public void PlayDied()
    {
        Source.PlayOneShot(DieSound);
    }

    public void PlayExitted()
    {
        Source.PlayOneShot(ExitSound);
    }
}

