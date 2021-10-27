using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;

public class GameObjectSpawnerWindow : EditorWindow
{
    GameObject[] gameObjects;
    GameObject currentObjet;

    Vector3 position = Vector3.zero;
    Vector3 rotation = Vector3.zero;

    [MenuItem("Loïck Tool/GameObject Spawner")]
    static void InitWindow()
    {
        GameObjectSpawnerWindow window = GetWindow<GameObjectSpawnerWindow>();
        window.titleContent = new GUIContent("GameObject Spawner");
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Update Entity Folder"))
        {
            LocateAssetGameObjects();
        }
        EditorGUILayout.Space();
        position = EditorGUILayout.Vector3Field("GameObject Position", position);
        rotation = EditorGUILayout.Vector3Field("GameObject Rotation", rotation);
        currentObjet = EditorGUILayout.ObjectField("Object Selectionné", currentObjet, typeof(GameObject), false) as GameObject;
        DisplayButtonGameObjects();
        if (GUILayout.Button("Spawn GameObject") && currentObjet != null)
        {
            SpawnGameObject();
        }
    }

    void LocateAssetGameObjects()
    {
        string[] guidAssets = AssetDatabase.FindAssets("", new string[] { "Assets/Entity" });
        gameObjects = new GameObject[guidAssets.Length];
        for (int i = 0; i < (gameObjects.Length); i++)
        {
            string currentGuid = guidAssets[i];
            string currentAssetPath = "";
            currentAssetPath = AssetDatabase.GUIDToAssetPath(currentGuid);
            gameObjects[i] = AssetDatabase.LoadAssetAtPath<GameObject>(currentAssetPath);
        }
        Debug.Log(gameObjects.Length + " Objet dans le dossier Entity");
    }

    void DisplayButtonGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            if (GUILayout.Button(obj.name))
            {
                currentObjet = obj;
            }
        }
    }
    void SpawnGameObject()
    {
        GameObject obj = new GameObject();
        obj = currentObjet.gameObject;
        obj.transform.position = position;
        Vector3 localrotation = obj.transform.eulerAngles;
        localrotation = rotation;
    }
}