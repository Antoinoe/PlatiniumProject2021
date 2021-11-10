using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    #region Variables
    GameManager gm;
    #endregion

    private void Start()
    {
        gm = GameManager.GetInstance();
    }

    #region Enter Bush
    private void OnTriggerEnter(Collider collision)
     {
        Debug.Log("entered bush");
         GameObject collidingObject = collision.gameObject;
         if (collidingObject && (collidingObject.CompareTag("Player") | collidingObject.CompareTag("NPC")))
         {
            if (collidingObject.CompareTag("Player"))
            {
                PlayerController playerController = collidingObject.GetComponent<PlayerController>();
                playerController.SetInvisibility(true);
            }
            else if (collidingObject.CompareTag("NPC"))
            {
                IAIdentity iAIdentity = collidingObject.GetComponent<IAIdentity>();
                SpriteRenderer spriteRend = iAIdentity.spriteRend;
                spriteRend.sprite = gm.invisibleSprite;
                //debug
                Color spriteColor = spriteRend.color;
                spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0.25f);
                spriteRend.color = spriteColor;
                //end debug
                iAIdentity.isInvisible = true;
            }
        }
     }
    #endregion

    #region Leave Bush
    private void OnTriggerExit(Collider collision)
     {
         GameObject collidingObject = collision.gameObject;
         if (collidingObject && (collidingObject.CompareTag("Player") | collidingObject.CompareTag("NPC")))
         {
            if (collidingObject.CompareTag("Player"))
            {
                PlayerController playerController = collidingObject.GetComponent<PlayerController>();
                playerController.SetInvisibility(false);
            }
            else if (collidingObject.CompareTag("NPC"))
            {
                IAIdentity iAIdentity = collidingObject.GetComponent<IAIdentity>();
                SpriteRenderer spriteRend = iAIdentity.spriteRend;
                spriteRend.sprite = gm.players[iAIdentity.teamNb].playerSprite;
                //debug
                Color spriteColor = spriteRend.color;
                spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
                spriteRend.color = spriteColor;
                //end debug
                iAIdentity.isInvisible = false;
            }
        }
     }
    #endregion
}
