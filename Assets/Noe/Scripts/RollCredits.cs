using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class RollCredits : MonoBehaviour
{
    [SerializeField]
    private GameObject slider;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float distance;
    private void Start()
    {
        StartCredits();
        StartCoroutine(BackToTheMenu());
    }

    private void Update()
    {
        // forcer le retour menu
    }

    void StartCredits()
    {
        Vector3 startPos = slider.transform.position;
        slider.GetComponent<RectTransform>().DOLocalMoveX(-distance, speed); 
    }

    IEnumerator BackToTheMenu()
    {
        yield return new WaitForSeconds(speed + 3.0f);
        SceneManager.LoadScene("Main");
    }


}
