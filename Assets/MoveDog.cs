using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDog : MonoBehaviour
{
    private float move, moveside, speed; //move will move the dog
    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis("Vertical")*speed*Time.deltaTime;
        //moveside = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
    }

    void LateUpdate()
    {
        transform.Translate(0f,0f, move);
        //transform.Translate(moveside,0f, 0f);
    }
}
