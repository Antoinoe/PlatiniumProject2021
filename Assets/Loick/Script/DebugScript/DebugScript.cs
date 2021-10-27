using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class DebugScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
