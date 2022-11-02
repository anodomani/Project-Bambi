using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableOnTrigger : MonoBehaviour
{
    public GameObject activatedObject;
    public int amountOfPlayersIn;

    void Update(){
        if(amountOfPlayersIn > 1){
            activatedObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.TryGetComponent(out Player player)){
            amountOfPlayersIn++;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.TryGetComponent(out Player player)){
            amountOfPlayersIn--;
        }
    }
}
