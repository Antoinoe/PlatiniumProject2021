using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    Vector3 vect3;
    float size;
    bool intro;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        transform.position = new Vector3(-7, 8, -1);
        size = 1;
        vect3 = transform.position;
        intro = true;
        StartCoroutine(CameraIntro());
    }

    private IEnumerator CameraIntro()
    {
        cam.orthographicSize = 1;
        transform.position = new Vector3(-7, 8, -1);

        yield return new WaitForSeconds(.5f);
        for (float i = 0; i < .35f; i += 0.01f)
        {
            vect3 = Vector3.Lerp(transform.position, new Vector3(7, 8, -1), i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < .35f; i += 0.01f)
        {
            vect3 = Vector3.Lerp(transform.position, new Vector3(-7, 0, -1), i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < .35f; i += 0.01f)
        {
            vect3 = Vector3.Lerp(transform.position, new Vector3(7, 0, -1), i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < .35f; i += 0.01f)
        {
            vect3 = Vector3.Lerp(transform.position, new Vector3(0, 4.25f, -1), i);
            yield return new WaitForSeconds(Time.deltaTime);
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
