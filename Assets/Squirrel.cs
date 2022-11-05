using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Squirrel : MonoBehaviour
{
    public int nuts;
    public TMP_Text nutsUI;
    public GameObject Dialogue;

    // Update is called once per frame
    void Update()
    {
        nutsUI.text = nuts.ToString();
        if(nuts > 4){
            Dialogue.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.TryGetComponent(out Acorn acorn) && !acorn.collected){
            nuts++;
            acorn.collected = true;
        }
    }
}
