using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    ParticleSystem pS;

    // Start is called before the first frame update
    void Start()
    {
        pS = GetComponent<ParticleSystem>();
        Invoke("Kill", pS.main.duration*2);
    }

    void Kill(){
        Destroy(this.gameObject);
    }
}
