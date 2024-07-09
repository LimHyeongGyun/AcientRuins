using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponData : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private DataManager dataManager;

    public ItemInfo backweaponInfo;
    public ItemInfo handweaponInfo;
    public GameObject backWeapon;
    public GameObject handWeapon;
    public bool visualize;
    [SerializeField]
    private GameObject eqWeaponSlot;
    public bool equip;
    public bool fillStrength;
    //무기 장착 해제시 가시화
    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
        eqWeaponSlot = GameObject.Find("WeaponSlot").transform.GetChild(1).gameObject;
    }
    public void WeaponVisulalize()
    {
        backWeapon.SetActive(!visualize);
        handWeapon.SetActive(visualize);
    }
    public void EquipWeapon(ItemInfo _backWeapon, ItemInfo _handWeapon)
    {
        if (handWeapon != null)
        {
            ClearWeapon();
        }

        backweaponInfo = _backWeapon;
        handweaponInfo = _handWeapon;
        backWeapon = _backWeapon.gameObject;
        handWeapon = _handWeapon.gameObject;

        ChangeWeaponInfo(_handWeapon);

        eqWeaponSlot.GetComponent<Image>().sprite = _backWeapon.itemSprite;
        eqWeaponSlot.SetActive(true);

        WeaponVisulalize();
    }
    public void ClearWeapon()
    {
        DisarmWeaponInfo();

        backweaponInfo = null;
        handweaponInfo = null;

        backWeapon.SetActive(false);
        handWeapon.SetActive(false);

        eqWeaponSlot.SetActive(false);
        eqWeaponSlot.GetComponent<Image>().sprite = null;
    }
    public void ChangeWeaponInfo(ItemInfo _weapon)
    {
        _weapon.InitSetData();
        //장착한 무기를 사용하기 위한 힘 스탯을 만족한다면
        if (player.data.strength >= _weapon.wStats.needStr)
        {
            fillStrength = true;
            player.inventory.inventoryUI.fillStr = "충족";
            player.data.power += _weapon.wStats.wPower;
        }
        //장착한 무기를 사용하기 위한 힘 스탯이 부족하다면
        else if (player.data.strength < _weapon.wStats.needStr)
        {
            fillStrength = false;
            player.inventory.inventoryUI.fillStr = "미충족";
            player.data.power = 1 + (player.data.powerLv - 1) * 2;
        }
        player.inventory.inventoryUI.PlayerInformation();
    }
    //무기 능력치 해제
    private void DisarmWeaponInfo()
    {
        //장착한 무기를 사용하기 위한 힘 스탯을 만족한 상태였다면
        if (player.data.strength >= handweaponInfo.wStats.needStr)
        {
            player.data.power -= handweaponInfo.wStats.wPower; //무기 파워 빼기
        }
        //장착한 무기를 사용하기 위한 힘 스탯이 부족한 상태였다면
        else if (player.data.strength < handweaponInfo.wStats.needStr)
        {
            //플레이어 기본 공격력 유지
            player.data.power = 1 + (player.data.powerLv - 1) * 2;
        }
    }
}
