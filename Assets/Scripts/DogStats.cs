using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogStats : MonoBehaviour
{
    public static DogStats instance; //it is static - check if any future error
    public int maxHearts;
    int hearts;
    
    // Start is called before the first frame update
    private void Start()
    {
        hearts = maxHearts;
        UnityEngine.Debug.Log("Hearts at beggining " + hearts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    public void TakeDamage()
    {
        if(hearts<=0)
        {
            return;
        }
        else
        {
            hearts-=1;
            UnityEngine.Debug.Log("Damage " + hearts);
        }
        
    }
}
