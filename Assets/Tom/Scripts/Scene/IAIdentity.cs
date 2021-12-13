using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAIdentity : MonoBehaviour
{
    #region Variables
    [HideInInspector] public int teamNb;
    public SpriteRenderer spriteRend;
    public AIController controllerIdentity;
    [HideInInspector] public int isInvisible = 0;
    #endregion
}
