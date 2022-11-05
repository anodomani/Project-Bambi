using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogue : MonoBehaviour
{
    //public GameObject linkedObject;
    public void Update(){
        Time.timeScale = 0;
        if(Input.GetMouseButtonDown(0)){
            Time.timeScale = 1;
            this.gameObject.SetActive(false);
        }
    }
}
