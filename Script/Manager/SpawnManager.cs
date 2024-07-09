using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public enum SpawnType { Catfish, Dullahan, Scavanger, Demon, Minotaur}
    public SpawnType monType;

    public Transform spawnPoints;
    public GameObject[] enemy;

    private int spawnNum;

    // Update is called once per frame
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (monType == SpawnType.Catfish)
        {
            spawnNum = 0;
        }
        else if (monType == SpawnType.Dullahan)
        {
            spawnNum = 1;
        }
        else if (monType == SpawnType.Scavanger)
        {
            spawnNum = 2;
        }
        else if (monType == SpawnType.Demon)
        {
            spawnNum = 3;
        }
        else if (monType == SpawnType.Minotaur)
        {
            spawnNum = 4;
        }

        Instantiate(enemy[spawnNum], spawnPoints);
    }
}
