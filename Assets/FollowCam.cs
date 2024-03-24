using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform knight;
    public Vector3 camOffset ;
    //private float cur;
    private float rotateX = 0f;
    private float rotateY = 0f;

    float sensitivity = 5f;
    
    void Start()
    {
        //cur = 0.0f;
               
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = knight.transform.position + camOffset;
        
    }

    void Update()
    {
        
        
        rotateX = Input.GetAxis ("Mouse X")*sensitivity;
        

        knight.Rotate(Vector3.up * rotateX);
    }
}
