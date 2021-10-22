using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestDist : MonoBehaviour
{
    public Transform S, Mid, E, player;
    public float range;
    bool isAtPos = false;
    // Start is called before the first frame update
    void Start()
    {
        SetRandPos();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isAtPos) SetRandPos();
        isAtPos = false;
        StopAllCoroutines();
        Mid.position = S.position + new Vector3(range, range, 0);
        float rangeBtwES = Vector2.Distance(S.position, E.position);
        float angle = (float)System.Math.Atan2(E.position.y - S.position.y, E.position.x - S.position.x);
        Debug.Log(angle * Mathf.Rad2Deg);
        Vector2 vec = new Vector2(rangeBtwES * Mathf.Cos(angle), rangeBtwES * Mathf.Sin(angle));
        Mid.position = vec.normalized + (Vector2)S.position/* + new Vector2(range, range)*/;
        //StartCoroutine(MovePerso());
        Vector2 fVec = (Mid.position + S.position) - S.position;
        Mid.position = fVec;
    }

    void SetRandPos()
    {
        isAtPos = false;
        StopAllCoroutines();
        Mid.position = S.position + new Vector3(range, range, 0);
        float rangeBtwES = Vector2.Distance(S.position, E.position);
        float angle = (float)System.Math.Atan2(E.position.y - S.position.y, E.position.x - S.position.x);
        Debug.Log(angle * Mathf.Rad2Deg);
        Vector2 vec = new Vector2(rangeBtwES * Mathf.Cos(angle), rangeBtwES * Mathf.Sin(angle));
        Mid.position = vec.normalized + (Vector2)S.position;
        //StartCoroutine(MovePerso());
    }

    IEnumerator MovePerso()
    {
        float index = 0;
        Vector2 startPos = S.position;
        Vector2 endPos = (Vector2)Mid.position + Random.insideUnitCircle * range;
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

        Gizmos.color = Color.red;
        Gizmos.DrawLine(S.position, new Vector2(Mid.position.x + range, Mid.position.y + range));
    }

}
