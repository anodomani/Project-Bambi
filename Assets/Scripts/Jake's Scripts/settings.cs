using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class settings : MonoBehaviour
{
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Toggle musicToggle;

    private audioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<audioManager>();
        sfxToggle.isOn = !audioManager.sfxMuted;
        musicToggle.isOn = !audioManager.musicMuted;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
