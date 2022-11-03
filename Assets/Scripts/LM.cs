using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LM : MonoBehaviour
{
    public static LM inst;
    void Awake(){
        if (inst != null && inst != this)
        {
            Destroy(this.gameObject);
        } else {
            inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Restart"))
        {
            restartScene();
        }
    }

    public void restartScene()
    {
        print("reloading scene");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
