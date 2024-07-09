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
    //���� ���� ������ ����ȭ
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
        //������ ���⸦ ����ϱ� ���� �� ������ �����Ѵٸ�
        if (player.data.strength >= _weapon.wStats.needStr)
        {
            fillStrength = true;
            player.inventory.inventoryUI.fillStr = "����";
            player.data.power += _weapon.wStats.wPower;
        }
        //������ ���⸦ ����ϱ� ���� �� ������ �����ϴٸ�
        else if (player.data.strength < _weapon.wStats.needStr)
        {
            fillStrength = false;
            player.inventory.inventoryUI.fillStr = "������";
            player.data.power = 1 + (player.data.powerLv - 1) * 2;
        }
        player.inventory.inventoryUI.PlayerInformation();
    }
    //���� �ɷ�ġ ����
    private void DisarmWeaponInfo()
    {
        //������ ���⸦ ����ϱ� ���� �� ������ ������ ���¿��ٸ�
        if (player.data.strength >= handweaponInfo.wStats.needStr)
        {
            player.data.power -= handweaponInfo.wStats.wPower; //���� �Ŀ� ����
        }
        //������ ���⸦ ����ϱ� ���� �� ������ ������ ���¿��ٸ�
        else if (player.data.strength < handweaponInfo.wStats.needStr)
        {
            //�÷��̾� �⺻ ���ݷ� ����
            player.data.power = 1 + (player.data.powerLv - 1) * 2;
        }
    }
}
