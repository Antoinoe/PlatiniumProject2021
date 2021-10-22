using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [HideInInspector] public Attack playerScript;

    private void OnTriggerEnter(Collider collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject && (collidingObject.tag == "Player" && collidingObject.GetComponent<Attack>().teamNb != playerScript.teamNb) | collidingObject.tag == "NPC")
        {
            playerScript.targets.Add(collidingObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject.tag == "Player" | collidingObject.tag == "NPC")
        {
            playerScript.targets.Remove(collidingObject);
        }
    }
}
