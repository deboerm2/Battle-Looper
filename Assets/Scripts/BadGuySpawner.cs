using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuySpawner : MonoBehaviour
{
    public GameObject meleeBadGuyPrefab;
    public GameObject rangedBadGuyPrefab;
    public bool needNewRound = true;

    private Collider2D col2D;
    private int melee;
    private int ranged;
    private bool doSpawn;

    // Start is called before the first frame update
    void Start()
    {
        col2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        while (doSpawn)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (melee <= 0)
                    continue;
                melee--;
                doSpawn = false;
                StartCoroutine(SpawnEnemy(meleeBadGuyPrefab));

            }
            else
            {
                if (ranged <= 0)
                    continue;
                ranged--;
                doSpawn = false;
                StartCoroutine(SpawnEnemy(rangedBadGuyPrefab));

            }

        }

        if(melee <= 0 && ranged <= 0 && transform.childCount == 0)
        {
            print("needNewRound");
            needNewRound = true;
        }

    }

    public void NewRound(GameManager.Round newRound)
    {
        melee = newRound.numMeleeBadGuys;
        ranged = newRound.numRangedBadGuys;
        needNewRound = false;
        doSpawn = true;

    }

    IEnumerator SpawnEnemy(GameObject badGuy)
    {
        Vector2 spawnpoint = new Vector3(Random.Range(-col2D.bounds.extents.x, col2D.bounds.extents.x),
                                        Random.Range(-col2D.bounds.extents.y, col2D.bounds.extents.y));
        spawnpoint.x += gameObject.transform.position.x;
        spawnpoint.y += gameObject.transform.position.y;
        Instantiate(badGuy, spawnpoint, Quaternion.identity, gameObject.transform);

        yield return new WaitForSeconds(0.5f);
        if (melee != 0 || ranged != 0)
            doSpawn = true;
        else
            doSpawn = false;
    }

}
