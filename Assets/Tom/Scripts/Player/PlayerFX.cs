using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject attackFX;
    [HideInInspector] public Vector2 dir;

    void Start()
    {
        
    }


    public void spawnFX()
    {
        float angle = Vector2.SignedAngle(Vector2.right, dir - (Vector2)transform.position);
        GameObject fist = GameObject.Instantiate(attackFX, transform.position, Quaternion.Euler(0, angle, 0));

        if (dir.x < 0)
        {
            Vector3 scale = fist.transform.localScale;
            fist.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
}
