using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource footsteps, pickupItem, hit, newLevel;
    public Slider globalVolume, sfxVolume, musicVolume, ambientVolume;
    public AudioSource[] sfx, music, ambience;

    public GameObject[] levelSounds;

    public static SoundManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        globalVolume.value = PlayerPrefs.GetFloat("globalVolume", globalVolume.value);
        sfxVolume.value = PlayerPrefs.GetFloat("sfxVolume", sfxVolume.value);
        musicVolume.value = PlayerPrefs.GetFloat("musicVolume", musicVolume.value);
        ambientVolume.value = PlayerPrefs.GetFloat("ambientVolume", ambientVolume.value);
    }

    // Update is called once per frame
    void Update()
    {
        if (MapGenerator.Instance != null)
        {
            for (int i = 0; i < levelSounds.Length; i++)
            {
                levelSounds[i].SetActive(i == MapGenerator.Instance.floor);
            }
        }
    }

    public void ManageAudio()
    {
        AudioListener.volume = globalVolume.value;

        foreach (AudioSource source in sfx) { source.volume = sfxVolume.value; }
        foreach (AudioSource source in music) { source.volume = musicVolume.value; }
        foreach (AudioSource source in ambience) { source.volume = ambientVolume.value; }

        PlayerPrefs.SetFloat("globalVolume", globalVolume.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume.value);
        PlayerPrefs.SetFloat("musicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("ambientVolume", ambientVolume.value);
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
