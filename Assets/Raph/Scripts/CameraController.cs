using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private Vector3 vect3;
    private float size;
    private bool intro;

    public List<Vector3> Positions = new List<Vector3>();
    [Range(1, 10)]
    public int speed = 2;
    [Range(0f, 1f)]
    public float delay = .35f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        if (Positions.Count > 0)
        {
            transform.position = Positions[0];
        }
        vect3 = transform.position;
        size = 1;
        cam.orthographicSize = size;
        intro = true;
        StartCoroutine(CameraIntro());
    }

    private IEnumerator CameraIntro()
    {
        yield return new WaitForSeconds(.5f);

        
        foreach (Vector3 Pos in Positions)
        {
            for (float i = 0; i < delay; i += speed * 0.005f)
            {
                vect3 = Vector3.Lerp(transform.position, Pos, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        yield return new WaitForSeconds(1);

        for (float i = 0.1f; i < 1f; i += Time.deltaTime)
        {
            size = Mathf.Lerp(1, 5, (Mathf.Log(i) / 3) + 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        intro = false;
        yield break;
    }

    // Update is called once per frame
    void Update()
    {

        if (intro) { transform.position = vect3; }
        cam.orthographicSize = size;
    }
}
