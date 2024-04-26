using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReplace : MonoBehaviour
{
    public GameObject boxItem;
    GameObject instantCherry;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplaceBox(){
        instantCherry = Instantiate(boxItem, transform.position, transform.rotation); //replacing box crate with item, in same position/config
        
        Destroy(gameObject);

        //Destroying the cherry object, to indicate that it is has been collected
        if (instantCherry != null)
        {
            Destroy(instantCherry,1.5f);
        }
    }

    
   
}
