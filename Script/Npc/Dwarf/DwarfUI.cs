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
        dialogueManager.FindContext("���� ������ �ð��� ��");
        dialogueManager.ActiveUI();
        fix = true;
    }
    public void Fix()
    {
        //������ ���а� ���� ��
        if (player.equipData.shieldData.shieldInfo == null)
        {
            dialogueManager.FindContext("������ ���а� ���� ��");
            dialogueManager.ActiveUI();
        }
        else if (player.equipData.shieldData.shieldInfo != null)
        {
            int fixdurability = player.equipData.shieldData.shieldInfo.sStats.maxdurability - player.equipData.shieldData.shieldInfo.sStats.durability;
            //������ �ʿ��� ������ �� 
            if (player.equipData.shieldData.shieldInfo.sStats.maxdurability > player.equipData.shieldData.shieldInfo.sStats.durability)
            {
                //��ȭ�� ����� ���� ��
                if (player.data.silver >= fixdurability * 4)
                {
                    for (int i = 0; i < fixdurability; i++)
                    {
                        PaySilver(i, fixdurability);
                        player.equipData.shieldData.shieldInfo.sStats.durability++;
                        if (i == fixdurability - 1)
                        {
                            dialogueManager.FindContext("������ ������ ��");
                            dialogueManager.ActiveUI();
                        }
                    }
                }
                //��ȭ�� ������� ���� ��
                else if (player.data.silver < fixdurability * 4)
                {
                    dialogueManager.FindContext("��ȭ�� ������� ���� ��");
                    dialogueManager.ActiveUI();
                }
            }
            //������ �ʿ����� ���� �����϶�
            else if (player.equipData.shieldData.shieldInfo.sStats.maxdurability <= player.equipData.shieldData.shieldInfo.sStats.durability)
            {
                dialogueManager.FindContext("�������� ���� ���� ������ ��");
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
        dialogueManager.FindContext("������� ���� ����");
        dialogueManager.ActiveUI();
        dwarf.TalkAnimation();
        dwarf.talk = false;
        player.interactive = false;
        uiManager.ActiveCursor();
    }
}