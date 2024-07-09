using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFinalBoss : MonoBehaviour
{
    public GameObject relic;
    public GameObject effect;

    private void Start()
    {
        relic.SetActive(false);
        effect.SetActive(false);
    }
    public void ActiveRelic()
    {
        relic.SetActive(true);
        effect.SetActive(true);
    }
}
