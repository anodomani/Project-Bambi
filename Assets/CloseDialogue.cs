using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogue : MonoBehaviour
{
    public GameObject linkedObject;
    public void Update(){
        if(Input.GetMouseButtonDown(0)){
            linkedObject.SetActive(false);
        }
    }
}
