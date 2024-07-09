using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    //플레이어 스탯 받아오기
    public int maxHp;
    public int strength;
    public int power;

    public int silver;
    public int acientMemorie;

    public int healthLv;
    public int strengthLv;
    public int powerLv;

    //저장된 씬과 위치 받아오기
    public string sceneName;
    public Transform saveBaseTransform;
    public float posX;
    public float posY;
    public float posZ;

    //현재 장착한 아이템
    public int equipWeapon;
    public int equipShield;
    public int equipItem;
    public bool equipW;
    public bool equipS;
    public bool equipI;

    //인벤토리 현황
    public List<ItemInfo> playerInventory = new List<ItemInfo>();
}
public class SaveJsonData : MonoBehaviour
{
    public PlayerData playerData;

    public void SavePlayerDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        //데이터를 저장할 경로 지정
        string path = Path.Combine(Application.dataPath, "playerData.json");
        //기존에 저장된 파일이 있다면 삭제
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        //파일 생성 및 저장
        File.WriteAllText(path, jsonData);
    }

    public void LoadPlayerDataFromJson()
    {
        //데이터를 불러올 경로 지정
        string path = Path.Combine(Application.dataPath, "playerData.json");
        //파일의 텍스트를 string으로 저장
        string jsonData = File.ReadAllText(path);
        //이 Json데이터를 역 직렬화하여 plyaerData에 넣어줌
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
}
