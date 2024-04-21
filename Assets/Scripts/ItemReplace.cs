using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReplace : MonoBehaviour
{
    public GameObject boxItem;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplaceBox(){
        Instantiate(boxItem, transform.position, transform.rotation); //replacing box crate with item, in same position/config
        
        Destroy(gameObject);
    }

    
   
}
