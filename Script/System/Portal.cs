using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameManager gameManager;
    private BasicMonster boss;
    [SerializeField]
    string sceneName;
    [SerializeField]
    GameObject bossStagePortal;
    public bool clear;

    private void Awake()
    {
        if (bossStagePortal != null)
        {
            boss = FindObjectOfType<BasicMonster>();
            boss.portal = this;
            bossStagePortal.GetComponent<BoxCollider>().enabled = clear;
            bossStagePortal.transform.GetChild(0).gameObject.SetActive(clear);
        }
    }
    public void SceneChange()
    {
        gameManager = FindObjectOfType<GameManager>();

        gameManager.loadSceneName = sceneName;
        gameManager.crossScene = true;
        gameManager.ChangeLoadingScene();
    }
    public void ActiveClearPortal()
    {
        clear = !clear;
        bossStagePortal.GetComponent<BoxCollider>().enabled = clear;
        bossStagePortal.transform.GetChild(0).gameObject.SetActive(clear);
    }
}
