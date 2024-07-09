using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commodities : MonoBehaviour
{
    public BasicMonster monster;

    public int silver;
    public int acientMemorie;
    public float destroyTime;

    private void Start()
    {
        destroyTime = 60f;
    }
    private void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }
    public void CommoditiesAmount()
    {
        var monsterType = monster.monType;
        if (monsterType == BasicMonster.MonsterType.NormalMonster)
        {
            silver = Random.Range(3, 11);
            acientMemorie = Random.Range(10, 31);
        }
        else if (monsterType == BasicMonster.MonsterType.EliteMonster)
        {
            silver = Random.Range(10, 21);
            acientMemorie = Random.Range(30, 101);
        }
        else if (monsterType == BasicMonster.MonsterType.BossMonster)
        {
            silver = Random.Range(100, 151);
            acientMemorie = Random.Range(200, 301);
        }
    }
}
