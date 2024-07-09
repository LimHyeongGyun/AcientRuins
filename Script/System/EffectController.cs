using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    void Update()
    {
        Destroy(this.gameObject, 0.5f);
    }
}
