using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{
    public GameObject knight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //"DogWalk" is a input is have added in input manager. Shiba moves 
        //forward when is press W 
        if(Input.GetButtonDown("DogWalk")){
            knight.GetComponent<Animator>().Play("WalkForwardBattle");
        }
        //When I no longer press W (DogWalk), then Shiba is set to idle animation
        if(Input.GetButtonUp("DogWalk")){
            knight.GetComponent<Animator>().Play("Idle_Battle");
        }

         
        
    }
}
