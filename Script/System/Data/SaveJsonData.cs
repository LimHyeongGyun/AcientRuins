using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    //�÷��̾� ���� �޾ƿ���
    public int maxHp;
    public int strength;
    public int power;

    public int silver;
    public int acientMemorie;

    public int healthLv;
    public int strengthLv;
    public int powerLv;

    //����� ���� ��ġ �޾ƿ���
    public string sceneName;
    public Transform saveBaseTransform;
    public float posX;
    public float posY;
    public float posZ;

    //���� ������ ������
    public int equipWeapon;
    public int equipShield;
    public int equipItem;
    public bool equipW;
    public bool equipS;
    public bool equipI;

    //�κ��丮 ��Ȳ
    public List<ItemInfo> playerInventory = new List<ItemInfo>();
}
public class SaveJsonData : MonoBehaviour
{
    public PlayerData playerData;

    public void SavePlayerDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        //�����͸� ������ ��� ����
        string path = Path.Combine(Application.dataPath, "playerData.json");
        //������ ����� ������ �ִٸ� ����
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        //���� ���� �� ����
        File.WriteAllText(path, jsonData);
    }

    public void LoadPlayerDataFromJson()
    {
        //�����͸� �ҷ��� ��� ����
        string path = Path.Combine(Application.dataPath, "playerData.json");
        //������ �ؽ�Ʈ�� string���� ����
        string jsonData = File.ReadAllText(path);
        //�� Json�����͸� �� ����ȭ�Ͽ� plyaerData�� �־���
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
}
