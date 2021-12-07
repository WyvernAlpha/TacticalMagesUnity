using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager_Base : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        GameManager.instance.LoadScene(scene);
    }

    public void PlaySceneTransitionAudio(AudioClip clip)
    {
        AudioManager.instance.PlaySceneTransitionAudio(clip);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
