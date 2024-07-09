using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public bool activeCursor;

    private void Start()
    {
        ActiveCursor();
    }
    public void ActiveCursor()
    {
        activeCursor = !activeCursor;
        if (!activeCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (activeCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }   
    }
}
