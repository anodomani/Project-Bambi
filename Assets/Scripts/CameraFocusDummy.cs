using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusDummy : MonoBehaviour
{
    public GameObject[] camTargets;

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = new Vector2(0,0);
        foreach(GameObject i in camTargets){
            newPos += (Vector2)i.transform.position;
        }
        newPos /= camTargets.Length;
        transform.position = newPos;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
