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
    //방패 장착
    public void EquipShield(ItemInfo _shield, GameObject _shiledobj)
    {
        if (equipShield != null) //이미 장착중인 방패가 있다면
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
        equipShield.SetActive(false); //장착중인 방패 해제
        eqShieldSlot.SetActive(false);
        eqShieldSlot.GetComponent<Image>().sprite = null;
    }
    //실드 내구도 관리
    public void ShieldDurabilityManageMent(int _damage)
    {
        //실드가 파괴되지 않았다면
        if (!destroy)
        {
            shieldInfo.sStats.durability -= 2; //공격 방어시 내구도 2씩 감소
            inven.UpdateItemData(shieldInfo);
            if (shieldInfo.sStats.durability <= 0) //내구도가 0이하가 되면
            {
                //장착 해제시키기
                equip = false;
                ClearShield();
            }
        }
        else if (destroy) //방패가 파괴되었다면 //보스몬스터 공격에 의해서만 파괴됨
        {
            equip = false;
            shieldInfo.sStats.destory = destroy;
            inven.UpdateItemData(shieldInfo);
            if (shieldInfo.sStats.takeDamage) //방패가 부숴질 때 대미지를 입는 방패일 때
            {
                player._DecreaseHp(_damage);
            }
            ClearShield();
        }
    }
}