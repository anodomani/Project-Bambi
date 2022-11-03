using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class button : MonoBehaviour
{
    public audioManager audioManager;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Toggle musicToggle;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<audioManager>();
        sfxToggle.isOn = !audioManager.sfxMuted;
        musicToggle.isOn = !audioManager.musicMuted;
    }

    private void Update()
    {
        if(audioManager == null)
        {
            audioManager = FindObjectOfType<audioManager>();
        }
    }

    public void sfx()
    {
        if (sfxToggle.isOn)
        {
            audioManager.muteSFX(false);
        }
        else
        {
            audioManager.muteSFX(true);
        }
        onClick();
    }

    public void music()
    {
        if (musicToggle.isOn)
        {
            audioManager.muteMusic(false);
        }
        else
        {
            audioManager.muteMusic(true);
        }
        onClick();
    }

    public void onClick()
    {
        audioManager.Play("ui");
    }

    public void selectLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void pauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void restartScene()
    {
        print("reloading scene");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
