using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DogStats : MonoBehaviour
{
    public static DogStats instance; //it is static - check if any future error
    public Animator animator;

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
        animator = GetComponent<Animator>();
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
        if (animator.GetBool("Death") == true)
        {
            return;
        }

        hearts-=1;
        UnityEngine.Debug.Log("Player Damage " + hearts);
        if(onEventDamage != null)
        {
            onEventDamage();
        }
        if (hearts <= 0)
        {
            UnityEngine.Debug.Log("Dead");
            animator.SetBool("Death", true);
            return;
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
