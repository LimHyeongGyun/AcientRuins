using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldData : MonoBehaviour
{
    [SerializeField]
    private Inventory inven;
    public Player player;
    public ItemInfo shieldInfo;

    public int durability;
    public bool takeDamage;
    public bool destroy;
    public bool equip;
    public GameObject equipShield;

    [SerializeField]
    private GameObject eqShieldSlot;

    private void Awake()
    {
        eqShieldSlot = GameObject.Find("ShieldSlot").transform.GetChild(1).gameObject;
    }
    //���� ����
    public void EquipShield(ItemInfo _shield, GameObject _shiledobj)
    {
        if (equipShield != null) //�̹� �������� ���а� �ִٸ�
        {
            ClearShield();
        }
        shieldInfo = _shield;
        equipShield = _shiledobj;
        _shield.InitSetData();

        eqShieldSlot.GetComponent<Image>().sprite = _shield.itemSprite;
        eqShieldSlot.SetActive(true);
        equipShield.SetActive(true);
    }
    public void ClearShield()
    {
        shieldInfo = null;
        equipShield.SetActive(false); //�������� ���� ����
        eqShieldSlot.SetActive(false);
        eqShieldSlot.GetComponent<Image>().sprite = null;
    }
    //�ǵ� ������ ����
    public void ShieldDurabilityManageMent(int _damage)
    {
        //�ǵ尡 �ı����� �ʾҴٸ�
        if (!destroy)
        {
            shieldInfo.sStats.durability -= 2; //���� ���� ������ 2�� ����
            inven.UpdateItemData(shieldInfo);
            if (shieldInfo.sStats.durability <= 0) //�������� 0���ϰ� �Ǹ�
            {
                //���� ������Ű��
                equip = false;
                ClearShield();
            }
        }
        else if (destroy) //���а� �ı��Ǿ��ٸ� //�������� ���ݿ� ���ؼ��� �ı���
        {
            equip = false;
            shieldInfo.sStats.destory = destroy;
            inven.UpdateItemData(shieldInfo);
            if (shieldInfo.sStats.takeDamage) //���а� �ν��� �� ������� �Դ� ������ ��
            {
                player._DecreaseHp(_damage);
            }
            ClearShield();
        }
    }
}