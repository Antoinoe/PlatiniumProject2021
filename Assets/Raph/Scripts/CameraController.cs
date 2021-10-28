using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variable
    private Camera cam;
    private Vector3 vect3;
    private float size;
    private bool intro;

    [Header("Camera Intro")]
    [SerializeField] private bool skipIntro;
    [SerializeField] private List<Vector3> Positions = new List<Vector3>();
    [Range(1, 10)]
    [SerializeField] private int speed = 2;
    [Range(0f, 1f)]
    [SerializeField] private float delay = .35f;

    [Space]
    [Header("Shake Camera")]
    [Range(0f, 5f)]
    [SerializeField] private float shakeDur;
    [Range(0f, 1f)]
    [SerializeField] private float shakeStrenght;
    [Range(0, 10)]
    [SerializeField] private int shakeVibration;
    [Range(0, 20)]
    [SerializeField] private int shakeRandomness;

#endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        intro = false;

        if (Positions.Count > 0 && !skipIntro)
        {
            transform.position = Positions[0];
            cam.orthographicSize = size;
            vect3 = transform.position;
            size = transform.position.z * -1;
            intro = true;
            StartCoroutine(CameraIntro());
        }
    }

    private IEnumerator CameraIntro()
    {

        yield return new WaitForSeconds(.5f);

        foreach (Vector3 Pos in Positions)
        {
            for (float i = 0; i < delay; i += speed * 0.005f)
            {
                vect3 = Vector3.Lerp(transform.position, Pos, i);
                if (vect3.z * -1 != cam.orthographicSize)
                {
                    size = vect3.z * -1;
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
            vect3 = Pos;
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

    public void Shake()
    {
        Camera.main.DOShakePosition(shakeDur, shakeStrenght, shakeVibration, shakeRandomness);
    }

    // Update is called once per frame
    void Update()
    {
        if (intro)
        {
            transform.position = vect3;
            cam.orthographicSize = size;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shake();
            }
        }
    }
}
