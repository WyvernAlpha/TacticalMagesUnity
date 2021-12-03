using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSourceMain;
    [SerializeField] private AudioSource audioSourceTransition;
    [SerializeField] private AudioSet gameSceneMusic;

    private void Awake()
    {
        //Singleton for Audio Manager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Additional Audio Manager was found and destroyed.");
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayGameSceneMusic()
    {
        audioSourceMain.clip = gameSceneMusic.clip;
        audioSourceMain.volume = gameSceneMusic.volume;
        audioSourceMain.pitch = gameSceneMusic.pitch;
        audioSourceMain.Play();
    }

    public void PlaySceneTransitionAudio(AudioClip clip)
    {
        audioSourceTransition.PlayOneShot(clip, audioSourceTransition.volume);
    }

    public void PlayOneShot(AudioClip clip, float volume = 1.0f)
    {
        audioSourceMain.PlayOneShot(clip, volume);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {        
        if (scene.name == GameManager.instance.GetGameSceneName())
        {
            PlayGameSceneMusic();
        }
    }

}

[System.Serializable]
class AudioSet
{
    public AudioClip clip;
    public float volume = 1.0f;
    public float pitch = 1.0f;
}