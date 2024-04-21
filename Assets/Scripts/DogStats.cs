using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DogStats : MonoBehaviour
{
    public static DogStats instance; //it is static - check if any future error
    
    public event Action onEventDamage;
    public event Action onEventUpgraded;
    public int maxHearts;
    int hearts;
    public int Hearts { get { return hearts; } }

    
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
            if(onEventDamage != null)
            {
                onEventDamage();
            }
        }
        
    }

    public void RestoreHeart()
    {
        if(hearts>=maxHearts)
        {
            return;
        }
        else
        {
            hearts+=1;
            UnityEngine.Debug.Log("Adding " + hearts);
            if(onEventDamage != null)
            {
                onEventDamage();
            }
        }
        
    }

    public void UpgradeHealth()
    {
        maxHearts++;
        hearts = maxHearts;
        if (onEventUpgraded != null)
        {
            onEventUpgraded();
        }
    }
}
