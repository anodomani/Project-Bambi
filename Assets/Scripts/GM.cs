using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GM : MonoBehaviour
{
    public static GM inst;
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin vCamNoise;

    void Awake(){
        if (inst != null && inst != this)
        {
            Destroy(this.gameObject);
        } else {
            inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start(){
        vCam = FindObjectOfType<CinemachineVirtualCamera>();
        vCamNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator ScreenShake(float amplitude, int duration){
        while(duration > 0){
            //Camera.main.transform.position = new Vector2(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude));
            vCamNoise.m_AmplitudeGain = amplitude;
            duration--;
            yield return new WaitForFixedUpdate();
        }
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

    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
