using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform knight;
    //private float cur;
    private float rotateX = 0f;
    // private float rotateY = 0f;

    float sensitivity = 1.7f;

    public float cameraDistance = 10.0f;
    
    

    void Start()
    {
        //cur = 0.0f;
               
    }

    void LateUpdate()
    {
        transform.position = knight.transform.position - knight.transform.forward * cameraDistance;
        transform.LookAt(knight.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    void Update()
    {
            rotateX = Input.GetAxis ("Mouse X")*sensitivity;
        
        
            knight.Rotate(Vector3.up * rotateX);
       
        
    }

  

}
