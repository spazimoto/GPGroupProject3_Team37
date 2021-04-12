using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))    
        {
            SceneManager.LoadScene("Start");
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("Start");
    }
}
