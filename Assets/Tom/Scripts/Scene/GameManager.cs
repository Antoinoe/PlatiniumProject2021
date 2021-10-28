using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Variables
    static GameManager _instance;

    public Rewired.InputManager inputManager;

    public AudioManager audioManager;

    public Player[] players;
    public int playerNbrs = 1;

    [SerializeField] private int iAPerPlayer;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iAPrefab;

    private List<int> teams;

    //Camera shake
    [SerializeField] private float shakeDur;
    [SerializeField] private float shakeStrenght;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private int shakeRandomness;

    public event Action onCameraShake;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        _instance = this;
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        teams = new List<int>();

        Instantiate(inputManager);
        foreach (Player player in players)
        {
            //instancie joueur
            Vector2 newLocation = RandomNavmeshLocation(10, transform.position);
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(newLocation.x, newLocation.y, 0), playerPrefab.transform.rotation);

            //team
            newPlayer.GetComponent<PlayerController>().teamNb = player.playerNb;
            teams.Add(1);
            teams[player.playerNb] = 1;

            //skin
            newPlayer.GetComponentInChildren<SpriteRenderer>().sprite = player.playerSprite;

            //effects
            if (player.smokeSystem)
            {
                GameObject.Instantiate(player.smokeSystem, newPlayer.transform);
            }

            for (int i = 0; i < iAPerPlayer; i++)
            {
                Vector2 initPos2 = RandomNavmeshLocation(10, transform.position);
                GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), iAPrefab.transform.rotation);
                newIA.GetComponent<IAIdentity>().teamNb = player.playerNb;
                newIA.GetComponentInChildren<SpriteRenderer>().sprite = player.playerSprite;
            }
        }

        /*for (int i = 0; i < playerNbrs; i++)
        {
            Vector2 initPos = RandomNavmeshLocation(10, transform.position);
            GameObject newPlayer = GameObject.Instantiate(playerPrefab, new Vector3(initPos.x, initPos.y, 0), playerPrefab.transform.rotation);
            newPlayer.GetComponent<PlayerController>().teamNb = i;

            for (int i2 = 0; i2 < iAPerPlayer; i2++)
            {
                Vector2 initPos2 = RandomNavmeshLocation(10, transform.position);
                GameObject newIA = GameObject.Instantiate(iAPrefab, new Vector3(initPos2.x, initPos2.y, 0), iAPrefab.transform.rotation);
                newIA.GetComponent<IAIdentity>().teamNb = i;
            }
        }*/
    }

    public void WinCheck(int curTeam, int targetTeam)
    {
        teams[curTeam] += -1;
        teams[targetTeam] += 1;

        if (targetTeam == players.Length)
        {
            Win(targetTeam);
        }
    }

    public void Win(int teamNb)
    {
        Debug.Log("Team " + teamNb + " win !");
    }

    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin, AIController.CircleOrientation.Orientation navmeshOrientation)
    {
        List<int> allOrientations = new List<int>() { 0, 1, 2, 3 };
        List<int> tempList = allOrientations;
        for (int i = 0; i < allOrientations.Count; i++)
        {
            if (tempList[i] == (int)navmeshOrientation)
            {
                tempList.Remove(tempList[i]);
                break;
            }
        }
        int randomIndex = Random.Range(0, allOrientations.Count);
        navmeshOrientation = (AIController.CircleOrientation.Orientation)tempList[randomIndex];
        AIController.CircleOrientation iAOrientation = new AIController.CircleOrientation(navmeshOrientation);
        float angle = UnityEngine.Random.Range(iAOrientation.angleMin, iAOrientation.angleMax);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += origin;
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    public static Vector2 RandomNavmeshLocation(float radius, Vector2 origin)
    {
        float angle = UnityEngine.Random.Range(-180, Mathf.PI);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        randomPosition += origin;
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void Shake()
    {
        Camera.main.DOShakePosition(shakeDur, shakeStrenght, shakeVibrato, shakeRandomness);
    }

    public void Shake(float duration, float strength, int vibrato, int randomness)
    {
        Camera.main.DOShakePosition(duration, strength, vibrato, randomness);
    }
}
