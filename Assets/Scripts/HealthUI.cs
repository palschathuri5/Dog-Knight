using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject fillHeart;
    public List<Image> fillHearts;
    DogStats dogStats;
    // Start is called before the first frame update
    void Start()
    {
        dogStats = DogStats.instance;
        dogStats.onEventDamage += UpdateHearts;
        dogStats.onEventUpgraded += AddHearts;
        
        for(int i = 0; i< dogStats.maxHearts; i++)
        {
            GameObject instantiateHeart = Instantiate(fillHeart, this.transform);
            fillHearts.Add(instantiateHeart.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void UpdateHearts()
    {
        int emptyHeart = dogStats.Hearts;
        foreach(Image i in fillHearts)
        {
            i.fillAmount = emptyHeart;
            emptyHeart -= 1;
        }
    }

    void AddHearts()
    {
        foreach(Image i in fillHearts)
        {
            Destroy(i.gameObject);
        }
        fillHearts.Clear();
        for(int i = 0; i< dogStats.maxHearts; i++)
        {
            GameObject instantiateHeart = Instantiate(fillHeart, this.transform);
            fillHearts.Add(instantiateHeart.GetComponent<Image>());
        }
    }
}
