using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDestroyer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Destroy(this.gameObject);
    }
}
