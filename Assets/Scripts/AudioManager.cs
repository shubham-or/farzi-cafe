using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Sources---")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sound;

    [Header("---Audio Clips---")]
    [SerializeField] private AudioClip winning;
    [SerializeField] private AudioClip ingredient;

    public static AudioManager Instance;
    private void Awake() => Instance = this;

    void Start()
    {

    }

    public void SetMusicVolume(float _value) => music.volume = _value;

    public void SetSoundVolume(float _value) => sound.volume = _value;

    public void PlayMusicSound()
    {
        if (!music.isPlaying) music.Play();
    }

    public void PlayWinningSound() => StartCoroutine(Co_PlayWinningSound());

    public void PlayIngredientSound() => StartCoroutine(Co_PlayIngredientSound());

    private IEnumerator Co_PlayWinningSound()
    {
        if (music.isPlaying)
        {
            music.Pause();

            sound.Stop();
            sound.clip = winning;
            sound.Play();

            while (sound.isPlaying)
                yield return null;

            music.Play();
        }
        else
        {
            sound.Stop();
            sound.clip = ingredient;
            sound.Play();
        }
    }

    private IEnumerator Co_PlayIngredientSound()
    {
        if (music.isPlaying)
        {
            music.Pause();

            sound.Stop();
            sound.clip = ingredient;
            sound.Play();

            while (sound.isPlaying)
                yield return null;

            music.Play();
        }
        else
        {
            sound.Stop();
            sound.clip = ingredient;
            sound.Play();
        }
    }

}
