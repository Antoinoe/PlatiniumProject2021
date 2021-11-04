using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestDist : MonoBehaviour
{
    // public Transform S, Mid, E, player;
    // public float range;

    //bool isAtPos = false;

    public static Vector2 SetRandPos(Transform iAPos, Vector3 mainPoint, float range)
    {
        Vector2 mid = iAPos.position + new Vector3(range, range, 0);
        float rangeBtwES = Vector2.Distance(iAPos.position, mainPoint);
        float angle = (float)System.Math.Atan2(mainPoint.y - iAPos.position.y, mainPoint.x - iAPos.position.x);
        Vector2 vec = new Vector2(rangeBtwES * Mathf.Cos(angle), rangeBtwES * Mathf.Sin(angle));
        mid = vec.normalized + (Vector2)iAPos.position;
        Vector2 vecF = (Vector2)iAPos.position - mid;
        Vector2 vecFF = ((mid - vecF * range) - mid) + (Vector2)iAPos.position;
        mid = vecFF;

        return mid;
    }

    /*IEnumerator MovePerso()
    {
        float index = 0;
        Vector2 startPos = S.position;
        while (index < 1)
        {
            S.position = Vector2.Lerp(startPos, endPos, index);
            index += Time.deltaTime;

            yield return null;
        }

        isAtPos = true;
        yield return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Mid.position, range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector2.zero, S.position);
        Gizmos.DrawLine(Vector2.zero, E.position);

        /*Gizmos.color = Color.red;
        Vector2 vec = S.position - Mid.position;
        Gizmos.DrawLine(Mid.position, (Vector2)Mid.position - vec * range);*/

    /*Gizmos.color = Color.green;
    Vector2 vecF = S.position - Mid.position;
    Vector2 vecFF = ((Vector2)Mid.position - vecF * range) - (Vector2)Mid.position;
    Gizmos.DrawLine(Mid.position, vecFF);
}*/

}
