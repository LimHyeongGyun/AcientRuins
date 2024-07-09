using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : SingleTon<DataManager>
{
    private PlayerSetData setData;
    [SerializeField]
    private SaveJsonData jsonData;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private DialogueManager dialogueManager;
    //[HideInInspector]
    public Player player;

    public int maxHp;
    public int curHp;
    public int strength;
    public int power;

    public int silver;
    public int acientMemorie;

    public int healthLv;
    public int strengthLv;
    public int powerLv;

    public float posx;
    public float posy;
    public float posz;

    public string saveSceneName;
    public Transform baseTransform;
    public bool save;

    public void SetPlayerData()
    {
        if (!gameManager.load)
        {
            player = FindObjectOfType<Player>();
            setData = FindObjectOfType<PlayerSetData>();
            SetData();
        }
        dialogueManager.SetDialogue();
    }

    //�÷��̾� ������ ����
    public void SaveData()
    {
        save = true;
        //�÷��̾� ��������
        jsonData.playerData.maxHp = maxHp;
        jsonData.playerData.strength = strength;
        jsonData.playerData.power = power;

        jsonData.playerData.healthLv = healthLv;
        jsonData.playerData.strengthLv = strengthLv;
        jsonData.playerData.powerLv = powerLv;
        //�÷��̾� �Ҹ� ��ȭ ����
        jsonData.playerData.silver = silver;
        jsonData.playerData.acientMemorie = acientMemorie;
        //��ġ�� �� ����
        jsonData.playerData.sceneName = saveSceneName;
        jsonData.playerData.saveBaseTransform = baseTransform;
        if (baseTransform != null)
        {
            jsonData.playerData.posX = baseTransform.position.x;
            jsonData.playerData.posY = baseTransform.position.y;
            jsonData.playerData.posZ = baseTransform.position.z;
        }

        jsonData.playerData.playerInventory.Clear();
        //�κ��丮 ����
        foreach (ItemInfo weapon in player.inventory.weaponStorage)
        {
            jsonData.playerData.playerInventory.Add(weapon);
        }
        foreach (ItemInfo shield in player.inventory.shieldStorage)
        {
            jsonData.playerData.playerInventory.Add(shield);
        }
        foreach (ItemInfo item in player.inventory.consumableItemStorage)
        {
            jsonData.playerData.playerInventory.Add(item);
        }
        //������ ������ ����
        if (player.equipData.weaponData.equip)
        {
            jsonData.playerData.equipWeapon = player.equipData.weaponData.handweaponInfo.wStats.weaponNum;
            jsonData.playerData.equipW = player.equipData.weaponData.equip;
        }
        if (player.equipData.shieldData.equip)
        {
            jsonData.playerData.equipShield = player.equipData.shieldData.shieldInfo.sStats.shieldNum;
            jsonData.playerData.equipS = player.equipData.shieldData.equip;
        }
        if (player.equipData.consumeItemData.equip)
        {
            jsonData.playerData.equipItem = player.equipData.consumeItemData.consumeItemInfo.iInfo.ItemNum;
            jsonData.playerData.equipI = player.equipData.consumeItemData.equip;
        }

        jsonData.SavePlayerDataToJson();

        PlayerPrefs.SetInt("SAVE", System.Convert.ToInt16(save));
        PlayerPrefs.Save();
    }
    //����� ������ ȣ��
    public void LoadData()
    {
        jsonData.LoadPlayerDataFromJson();

        maxHp = jsonData.playerData.maxHp;
        curHp = maxHp;
        strength = jsonData.playerData.strength;
        power = jsonData.playerData.power;
        silver = jsonData.playerData.silver;
        acientMemorie = jsonData.playerData.acientMemorie;
        
        healthLv = jsonData.playerData.healthLv;
        strengthLv = jsonData.playerData. strengthLv;
        powerLv = jsonData.playerData. powerLv;

        saveSceneName = jsonData.playerData.sceneName;
        posx = jsonData.playerData.posX;
        posy = jsonData.playerData.posY;
        posz = jsonData.playerData.posZ;
    }
    public void LoadItemInfo()
    {
        int index = 0;
        //�κ��丮 ���� �ҷ�����
        foreach (ItemInfo item in jsonData.playerData.playerInventory)
        {
            //�κ��丮�� ������ �߰�
            player.inventory.AddItem(item);

            //���������� �������� �� �߰� �ߴٸ�
            if (index == jsonData.playerData.playerInventory.Count - 1)
            {
                //������ ����
                if (jsonData.playerData.equipW)
                {
                    foreach (ItemSlot weaponSlot in player.inventory.inventoryUI.weaponSlots)
                    {
                        if (weaponSlot.itemInfo.wStats.weaponNum == jsonData.playerData.equipWeapon)
                        {
                            weaponSlot.SelectSlot();
                        }
                    }
                }
                if (jsonData.playerData.equipS)
                {
                    foreach (ItemSlot shieldSlot in player.inventory.inventoryUI.shieldSlots)
                    {
                        if (shieldSlot.itemInfo.sStats.shieldNum == jsonData.playerData.equipShield)
                        {
                            shieldSlot.SelectSlot();
                        }
                    }
                }
                if (jsonData.playerData.equipI)
                {
                    foreach (ItemSlot itemSlot in player.inventory.inventoryUI.consumableSlots)
                    {
                        if (itemSlot.itemInfo.iInfo.ItemNum == jsonData.playerData.equipItem)
                        {
                            itemSlot.SelectSlot();
                        }
                    }
                }
            }
            index++;
        }
    }
    //���ӵ����� �ʱ�ȭ �� ó�� ���� �� �� ȣ��
    public void SetData()
    {
        maxHp = setData.stats.maxHp;
        curHp = maxHp;
        player.stamina = setData.stats.maxStamina;
        strength = setData.stats.strength;
        power = setData.stats.power;
        silver = setData.stats.silver;
        acientMemorie = setData.stats.acientMemorie;

        healthLv = setData.stats.healthLv;
        strengthLv = setData.stats.strengthLv;
        powerLv = setData.stats.powerLv;

        saveSceneName = SceneManager.GetActiveScene().name;
        Transform startPos = GameObject.Find("PortalPoint").transform;
        posx = jsonData.playerData.posX = startPos.position.x;
        posy = jsonData.playerData.posY = startPos.position.y;
        posz = jsonData.playerData.posZ = startPos.position.z;
        
        //������ �⺻ ����
        SaveData();
    }
    public void ChangeWeapon()
    {
        //���̽�ķ�� ������ �� ���� ����� ����
        foreach (ItemInfo weapon in player.inventory.weaponStorage)
        {
            jsonData.playerData.playerInventory.Add(weapon);
        }
        foreach (ItemInfo shield in player.inventory.shieldStorage)
        {
            jsonData.playerData.playerInventory.Add(shield);
        }
        foreach (ItemInfo item in player.inventory.consumableItemStorage)
        {
            jsonData.playerData.playerInventory.Add(item);
        }
        jsonData.playerData.power = power;
        jsonData.SavePlayerDataToJson();
    }
}
