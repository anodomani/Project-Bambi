using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public Vector2 paralaxSpeed;
    public Vector2 paralaxFlatOffset;
    private float width, offsetX;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        offsetX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (offsetX * Vector2.right) + Camera.main.transform.position * paralaxSpeed + paralaxFlatOffset;
        if(Camera.main.transform.position.x * (1 - paralaxSpeed.x) > offsetX + width){
            offsetX += width;
        }
    }
}
