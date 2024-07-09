using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParry : MonoBehaviour
{
    private HashSet<PlayerAttack> enteredAtk = new HashSet<PlayerAttack>();

    void Update()
    {
        DestroyObj();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            var playerAtk = other.gameObject.GetComponent<PlayerAttack>();

            if (!enteredAtk.Contains(playerAtk))
            {
                Player player = other.transform.parent.GetComponent<Player>();
                enteredAtk.Add(playerAtk);
                player.Groggy();
            }
            Destroy(gameObject);
        }
    }

    private void DestroyObj()
    {
        Destroy(gameObject, 0.7f);
    }
}