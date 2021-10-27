using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

[ExecuteAlways]
public class CameraShake : MonoBehaviour
{
    #region Variables
    //Camera shake
    [SerializeField] private float shakeDur;

    [SerializeField] private float shakeStrenght;
    [SerializeField] private Vector3 shakeStrenghtV3;
    [SerializeField] private bool shakeStrenghtWithVector3 = false;

    [SerializeField] private int shakeVibrato;

    [SerializeField] private int shakeRandomness;

    [SerializeField] private bool shakeFadeOut;
    #endregion

    public void Shake()
    {
        Debug.Log("shake !");

        if (shakeStrenghtWithVector3)
        {
            Camera.main.DOShakePosition(shakeDur, shakeStrenghtV3, shakeVibrato, shakeRandomness);
        }
        else
        {
            Camera.main.DOShakePosition(shakeDur, shakeStrenght, shakeVibrato, shakeRandomness);
        }
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
            }
        }
    }
    #endif
}
