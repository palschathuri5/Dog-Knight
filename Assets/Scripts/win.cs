using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{

    void Update()
    {
        // Find all objects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If there are no enemies left, load Scene 3
        if (enemies.Length == 0)
        {
            SceneManager.LoadScene(3);
        }
    }
}