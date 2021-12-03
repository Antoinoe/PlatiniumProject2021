using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLantern : MonoBehaviour
{
    HarborManager manager;
    public int nbr;
    float maxTime, currTime;

    void Start()
    {
        manager = FindObjectOfType<HarborManager>();
        maxTime = manager.timeToGetLantern;
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currTime += Time.deltaTime;
            if (currTime >= maxTime)
                manager.SetLanternColor(collision.GetComponent<PlayerController>().playerNb, gameObject, nbr);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currTime = 0;
    }
}
