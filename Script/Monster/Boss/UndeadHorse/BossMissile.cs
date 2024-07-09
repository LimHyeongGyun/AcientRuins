using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : MonsterAttack
{
    public GameObject target;
    NavMeshAgent nav;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        base.Update();
        nav.SetDestination(target.transform.position);
    }
}
