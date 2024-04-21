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
        for(int i = 0; i< dogStats.maxHearts; i++)
        {
            GameObject instantiateHeart = Instantiate(fillHeart, this.transform);
            fillHearts.Add(instantiateHeart.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
