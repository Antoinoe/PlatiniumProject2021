using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RollCredits : MonoBehaviour
{
    
    [Header("Credits Settings")]
    [Tooltip("Temps du déroulement des crédits en secondes.")]
    [SerializeField]private float progressSpeed;
    [Tooltip("GameObject comprennant les crédits.")]
    [SerializeField] private Rigidbody2D credits;
    private bool hasCreditsAnimationEnded = false;
    private void Start()
    {
        StartCredits();
    }

    private void Update()
    {
        if (hasCreditsAnimationEnded)
            SceneManager.LoadScene("Main");
    }

    void StartCredits()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        print("showing credits...");
        yield return new WaitForSeconds(3f);
        hasCreditsAnimationEnded = true;

    }

}
