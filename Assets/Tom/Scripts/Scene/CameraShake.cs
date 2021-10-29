using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;


public class CameraShake : MonoBehaviour
{
    #region Variables
    //Camera shake
    [SerializeField] private float shakeDur = 1;

    [SerializeField] private float shakeStrenght = 0.1f;
    [SerializeField] private Vector3 shakeStrenghtV3;
    [SerializeField] private bool shakeStrenghtWithVector3 = false;

    [SerializeField] private int shakeVibrato = 5;

    [Range(0, 90)]
    [SerializeField] private int shakeRandomness = 10;

    [SerializeField] private bool shakeFadeOut = true;

    //Game Manager
    private GameManager gm;
    #endregion

    private void Start()
    {
        gm = GameManager.GetInstance();
        gm.onCameraShake += Shake;
    }

    public void Shake()
    {
        Debug.Log("shake !");

        Vector3 basePos = transform.position;

        if (shakeStrenghtWithVector3)
        {
            transform.DOShakePosition(shakeDur, shakeStrenghtV3, shakeVibrato, shakeRandomness, shakeFadeOut);
        }
        else
        {
            transform.DOShakePosition(shakeDur, shakeStrenght, shakeVibrato, shakeRandomness, shakeFadeOut);
        }

        StartCoroutine(Reset(basePos));
    }

    private IEnumerator Reset(Vector3 resetPos)
    {
        Debug.Log("start reset");
        yield return new WaitForSeconds(shakeDur);
        Debug.Log("end reset");
        transform.position = resetPos;
        StopCoroutine(Reset(resetPos));
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(CameraShake))]
    public class CameraShakeEditor : Editor
    {
        #region Variables
        SerializedProperty shakeDurProp;

        SerializedProperty shakeStrenghtProp;
        SerializedProperty shakeStrenghtWithVector3Prop;
        SerializedProperty shakeStrenghtV3;

        SerializedProperty shakeVibratoProp;

        SerializedProperty shakeRandomnessProp;

        SerializedProperty shakeFadeOutProp;

        CameraShake camShake;

        GameManager gm;

        private void OnEnable()
        {
            shakeDurProp = serializedObject.FindProperty("shakeDur");

            shakeStrenghtProp = serializedObject.FindProperty("shakeStrenght");
            shakeStrenghtWithVector3Prop = serializedObject.FindProperty("shakeStrenghtWithVector3");
            shakeStrenghtV3 = serializedObject.FindProperty("shakeStrenghtV3");

            shakeVibratoProp = serializedObject.FindProperty("shakeVibrato");

            shakeRandomnessProp = serializedObject.FindProperty("shakeRandomness");

            shakeFadeOutProp = serializedObject.FindProperty("shakeFadeOut");

            camShake = (CameraShake)target;

            gm = GameManager.GetInstance(); 
        }

        #endregion
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //Duration
            EditorGUILayout.PropertyField(shakeDurProp);
            EditorGUILayout.HelpBox("The duration in seconds of the camera shake.", MessageType.None);
            EditorGUILayout.Space(15);

            //Strenght
            EditorGUILayout.PropertyField(shakeStrenghtWithVector3Prop);
            if (shakeStrenghtWithVector3Prop.boolValue)
            {
                EditorGUILayout.PropertyField(shakeStrenghtV3);
            }
            else
            {
                EditorGUILayout.PropertyField(shakeStrenghtProp);
            }
            EditorGUILayout.HelpBox("The shake strength. Using a Vector3 instead of a float lets you choose the strength for each axis.", MessageType.None);
            EditorGUILayout.Space(15);

            //Vibrato
            EditorGUILayout.PropertyField(shakeVibratoProp);
            EditorGUILayout.HelpBox("How much will the shake vibrate.", MessageType.None);
            EditorGUILayout.Space(15);

            //Randomness
            EditorGUILayout.PropertyField(shakeRandomnessProp);
            EditorGUILayout.HelpBox("How much the shake will be random. Setting it to 0 will shake along a single direction.", MessageType.None);
            EditorGUILayout.Space(15);

            //FadeOut
            EditorGUILayout.PropertyField(shakeFadeOutProp);
            EditorGUILayout.HelpBox("If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not.", MessageType.None);
            EditorGUILayout.Space(15);

            serializedObject.ApplyModifiedProperties();

            //Button
            if (GUILayout.Button("SHAKE !"))
            {
                camShake.Shake();
                //gm.Shake();
            }
        }
    }
    #endif
}
