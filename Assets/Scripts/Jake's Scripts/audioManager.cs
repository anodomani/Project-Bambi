using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public string copyPaste = "FindObjectOfType<audioManager>().Play('sound name');"; // line to paste in other scripts to call a sound

    public sound[] sounds; // sound class containing name, volume, pitch, loop bool, music bool, and the actual audio clip

    public static audioManager Instance;

    private Scene currentScene;
    public bool musicMuted = false;
    public bool sfxMuted = false;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {

            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // adds an audio source in the inspector for every audio source in the array, allowing you to manipulate each sound from the manager
        foreach (sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if(currentScene != SceneManager.GetActiveScene())
        {
            if(SceneManager.GetActiveScene().name == "Level1")
            {
                foreach (sound s in sounds)
                {
                    if (s.music == true)
                    {
                        s.source.Pause();
                    }
                }
                Play("winter_music");
            }
            else
            {
                foreach (sound s in sounds)
                {
                    if (s.music == true)
                    {
                        s.source.Pause();
                    }
                }
            }

            currentScene = SceneManager.GetActiveScene();
        }
    }

    // method to call for playing a certain sound
    public void Play(string name)
    {
        sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    // method to call for pausing a certain sound
    public void Pause(string name)
    {
        sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void muteMusic(bool mute)
    {
        foreach (sound s in sounds)
        {
            if (s.music == true)
            {
                s.source.mute = mute;
            }
        }

        musicMuted = mute;
    }

    public void muteSFX(bool mute)
    {
        foreach (sound s in sounds)
        {
            if (s.sfx == true)
            {
                s.source.mute = mute;
            }
        }

        sfxMuted = mute;
    }
}
