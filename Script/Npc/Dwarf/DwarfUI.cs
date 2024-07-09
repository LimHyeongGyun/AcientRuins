using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfUI : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private UIManager uiManager;
    public Player player;
    public Dwarf dwarf;
    public bool activeUI;
    public bool fix;

    [SerializeField]
    private GameObject dwarfUI;
    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
    }
    private void Start()
    {
        activeUI = true;
        ActiveDwarfUI();
    }
    public void ActiveDwarfUI()
    {
        activeUI = !activeUI;
        dwarfUI.SetActive(activeUI);
    }
    public void FixAShield()
    {
        dialogueManager.FindContext("방패 수리를 맡겼을 때");
        dialogueManager.ActiveUI();
        fix = true;
    }
    public void Fix()
    {
        //장착한 방패가 없을 때
        if (player.equipData.shieldData.shieldInfo == null)
        {
            dialogueManager.FindContext("장착한 방패가 없을 때");
            dialogueManager.ActiveUI();
        }
        else if (player.equipData.shieldData.shieldInfo != null)
        {
            int fixdurability = player.equipData.shieldData.shieldInfo.sStats.maxdurability - player.equipData.shieldData.shieldInfo.sStats.durability;
            //수리가 필요한 방패일 때 
            if (player.equipData.shieldData.shieldInfo.sStats.maxdurability > player.equipData.shieldData.shieldInfo.sStats.durability)
            {
                //재화가 충분히 있을 때
                if (player.data.silver >= fixdurability * 4)
                {
                    for (int i = 0; i < fixdurability; i++)
                    {
                        PaySilver(i, fixdurability);
                        player.equipData.shieldData.shieldInfo.sStats.durability++;
                        if (i == fixdurability - 1)
                        {
                            dialogueManager.FindContext("수리가 끝났을 때");
                            dialogueManager.ActiveUI();
                        }
                    }
                }
                //재화가 충분하지 않을 때
                else if (player.data.silver < fixdurability * 4)
                {
                    dialogueManager.FindContext("재화가 충분하지 않을 때");
                    dialogueManager.ActiveUI();
                }
            }
            //수리가 필요하지 않은 방패일때
            else if (player.equipData.shieldData.shieldInfo.sStats.maxdurability <= player.equipData.shieldData.shieldInfo.sStats.durability)
            {
                dialogueManager.FindContext("내구도가 닳지 않은 방패일 때");
                dialogueManager.ActiveUI();
            }
        }
    }
    private void PaySilver(int count, int fixdurability)
    {
        player.data.silver -= count * 4;
    }
    public void ReturnGame()
    {
        ActiveDwarfUI();
        dialogueManager.FindContext("드워프와 업무 종료");
        dialogueManager.ActiveUI();
        dwarf.TalkAnimation();
        dwarf.talk = false;
        player.interactive = false;
        uiManager.ActiveCursor();
    }
}