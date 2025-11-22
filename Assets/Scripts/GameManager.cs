using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Round
    {
        public int numMeleeBadGuys;
        public int numRangedBadGuys;
    }

    public static GameManager instance;

    public BadGuySpawner badGuySpawner;
    public GameObject GoodGuyParent;
    [HideInInspector]
    public EntityStats[] allEntities;
    
    public Round[] rounds;
    public int currentRound = 0;
    
    public GameObject[] brushPickupPrefabs;
    public Transform[] pickupSpawnAreas;
    [Tooltip("number of deaths needed for a pickup to spawn")]public int pickupSpawnCount;

    public GameObject loseScreen;
    public GameObject winScreen;

    private GameObject[] spawnedBrushes = new GameObject[8];
    //total deaths between each pickup spawn
    private int pickupSpawnCountTotal;
    private bool isWinner = false;
    

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        pickupSpawnCountTotal = pickupSpawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        allEntities = FindObjectsByType<EntityStats>(FindObjectsSortMode.None);
        if(GoodGuyParent.transform.childCount == 0)
        {
            GameOver();
        }

        if (badGuySpawner.needNewRound && !isWinner)
            NextRound();

        //gets subtracted when an entity dies, mostly bad guys but also good guys
        if(pickupSpawnCount <= 0)
        {
            SpawnBrushPickup();
            pickupSpawnCount = pickupSpawnCountTotal;
        }
    }

    void NextRound()
    {
        currentRound++;
        if(currentRound > rounds.Length)
        {
            isWinner = true;
            GameOver();
        }
        else
            badGuySpawner.NewRound(rounds[currentRound - 1]);

    }

    void SpawnBrushPickup()
    {
        //pick brush to spawn
        int selectedBrush = Random.Range(0, brushPickupPrefabs.Length);

        //instatiate brush pickup in first available open slot
        for (int i = 0; i < spawnedBrushes.Length; i++)
        {
            if(spawnedBrushes[i] == null)
            {
                spawnedBrushes[i] = Instantiate(brushPickupPrefabs[selectedBrush], pickupSpawnAreas[i].position, Quaternion.identity);
                return;
            }
        }
    }
    public void RemovePickup(GameObject pickup)
    {
        for (int i = 0; i < spawnedBrushes.Length; i++)
        {
            if(spawnedBrushes[i] == pickup)
            {
                spawnedBrushes[i] = null;
            }
        }
    }

    void GameOver()
    {
        if(isWinner)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
