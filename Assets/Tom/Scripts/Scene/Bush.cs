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
    private void OnTriggerEnter2D(Collider2D collision)
     {
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
                iAIdentity.isInvisible += 1;
                if(iAIdentity.isInvisible == 1)
                {
                    SpriteRenderer spriteRend = iAIdentity.spriteRend;
                    spriteRend.sprite = gm.invisibleSprite;
                    //debug
                    Color spriteColor = spriteRend.color;
                    spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0.25f);
                    spriteRend.color = spriteColor;
                    //end debug
                }
            }
        }
     }
    #endregion

    #region Leave Bush
    private void OnTriggerExit2D(Collider2D collision)
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
                iAIdentity.isInvisible += -1;
                if(iAIdentity.isInvisible == 0)
                {
                    SpriteRenderer spriteRend = iAIdentity.spriteRend;
                    spriteRend.sprite = gm.players[iAIdentity.teamNb].playerSprite;
                    //debug
                    Color spriteColor = spriteRend.color;
                    spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
                    spriteRend.color = spriteColor;
                    //end debug
                }
            }
        }
     }
    #endregion
}
