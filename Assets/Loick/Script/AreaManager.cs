using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public List<AIController.AreaCollider> areaColliders;

    public void ChangeCurrentAreaCollider(AIController iAController)
    {
        Vector2 iAPosition = iAController.transform.position;
        if (areaColliders[iAController.GetAreaIndex()].CheckPointIsInZonescollider(iAPosition))
        {
            for (int i = 0; i < areaColliders.Count; i++)
            {
                if (areaColliders[i].CheckPointIsInZonescollider(iAPosition))
                {
                    iAController.SetAreaIndex(i);
                }
            }

        }
    }
}
