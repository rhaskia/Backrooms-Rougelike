using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource footsteps, pickupItem, hit, newLevel;

    public static SoundManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayFootsteps()
    {
        if (!footsteps.isPlaying) footsteps.Play();
    }

    public void PlayPickup()
    {
        if (!pickupItem.isPlaying) pickupItem.Play();
    }

    public void PlayHit()
    {
        if (!hit.isPlaying) hit.Play();
    }

    public void PlayNewLevel()
    {
        if (!newLevel.isPlaying) newLevel.Play();
    }

    public void PlayClip(AudioSource audio)
    {
        if (!audio.isPlaying) audio.Play();
    }
}
