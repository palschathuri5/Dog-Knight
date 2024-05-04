using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchWin : MonoBehaviour
{
    public void SwitchToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
