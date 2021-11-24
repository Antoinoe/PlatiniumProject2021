using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public List<AIController.AreaCollider> areaColliders;
    private int areaIndex;

    public void ChangeCurrentAreaCollider(AIController iAController)
    {
        if (areaColliders[areaIndex].CheckPointIsInZonescollider(iAController.transform.position))
        {
            
        }
    }

    public int GetNewArea(AIController iAController)
    {

    }
}
