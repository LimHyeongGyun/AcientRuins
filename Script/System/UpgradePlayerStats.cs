using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePlayerStats : MonoBehaviour
{
    private DataManager dataManager;
    [SerializeField]
    private HPSlider hpSlider;
    public Player player;
    public Text upgradeHp;
    public Text upgradeStrength;
    public Text upgradePower;

    public Text playerHpInfo;
    public Text playerStrengthInfo;
    public Text playerPowerInfo;
    public Text silverInfo;
    public Text acientMemorieInfo;

    public GameObject equipWeaponImg;
    public GameObject equipShieldImg;
    public GameObject equipConsumeItemImg;

    public const int hpUpgradeValue = 5;
    public const int strengthUpgradeValue = 1;
    public const int powerUpgradeValue = 2;

    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    //Button
    public void UpgradeHealth()
    {
        if (dataManager.acientMemorie >= dataManager.healthLv * 15)
        {
            dataManager.acientMemorie -= dataManager.healthLv * 15;
            dataManager.maxHp += hpUpgradeValue;
            dataManager.curHp = dataManager.maxHp;
            hpSlider.SetHP();
            dataManager.healthLv++;
            UpdateInformation();
        }
    }
    public void UpgrageStrength()
    {
        if (dataManager.acientMemorie >= dataManager.strengthLv * 4)
        {
            dataManager.acientMemorie -= dataManager.strengthLv * 4;
            dataManager.strength += strengthUpgradeValue;
            dataManager.strengthLv++;
            UpdateInformation();
        }
    }
    
    public void UpgradePower()
    {
        if (dataManager.acientMemorie >= dataManager.powerLv * 52)
        {
            dataManager.acientMemorie -= dataManager.powerLv * 52;
            dataManager.power += powerUpgradeValue;
            dataManager.powerLv++;
            UpdateInformation();
        }
    }
    //���� ������Ʈ
    public void UpdateInformation()
    {
        upgradeHp.text = $"ü��: {dataManager.maxHp} => {dataManager.maxHp + hpUpgradeValue}" + 
            "\n" + $"�ʿ� ����� ���: {dataManager.healthLv * 15}";
        upgradeStrength.text = $"�ٷ�: {dataManager.strength} => {dataManager.strength + strengthUpgradeValue}" +
            "\n" + $"�ʿ� ����� ���: {dataManager.strengthLv * 4}";
        upgradePower.text = $"�⺻ ���ݷ�: {1 + (dataManager.powerLv - 1) * 2} => {1 + dataManager.powerLv * powerUpgradeValue}" +
            "\n" + $"�ʿ� ����� ���: {dataManager.powerLv * 52}";

        playerHpInfo.text = $"{dataManager.maxHp}";
        playerStrengthInfo.text = $"{dataManager.strength}";
        playerPowerInfo.text = $"{dataManager.power}" +
            "\n" + $"�⺻ ���ݷ�: {1 + (dataManager.powerLv - 1) * 2}" + $"���� ���ݷ�:{dataManager.player.equipData.weaponData.handweaponInfo.wStats.wPower}";
        silverInfo.text = $"{dataManager.silver}";
        acientMemorieInfo.text = $"{dataManager.acientMemorie}";

        if (player.equipData.weaponData.handweaponInfo != null)
        {
            equipWeaponImg.SetActive(true);
            equipWeaponImg.GetComponent<Image>().sprite = player.equipData.weaponData.handweaponInfo.itemSprite;
        }
        if (player.equipData.shieldData.shieldInfo != null)
        {
            equipShieldImg.SetActive(true);
            equipShieldImg.GetComponent<Image>().sprite = player.equipData.shieldData.shieldInfo.itemSprite;
        }
        if (player.equipData.consumeItemData.consumeItemInfo != null)
        {
            equipConsumeItemImg.SetActive(true);
            equipConsumeItemImg.GetComponent<Image>().sprite = player.equipData.consumeItemData.consumeItemInfo.itemSprite;
        }
    }
}