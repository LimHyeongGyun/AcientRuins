using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPSlider : MonoBehaviour
{
    private DataManager dataManager;

    public float maxHp;
    public float curHp;
    public float preHp;
    [SerializeField]
    private Slider hpValue;
    [SerializeField]
    private Slider afterImage;

    private void Start()
    {
        SetHP();
    }
    private void Update()
    {
        ChangeHPAfterUI();
    }
    public void HpValue()
    {
        hpValue.value = curHp / maxHp;
    }
    //���� ���� �Ǵ� ���� ������ HP ��
    public void SetHP()
    {
        maxHp = FindObjectOfType<DataManager>().maxHp;
        curHp = maxHp;
        preHp = curHp;
        HpValue();
    }
    //HP����
    public void ChangeHP(float changeHp)
    {
        preHp = curHp;
        curHp = changeHp;
        HpValue();
    }
    //HP���� �ܻ�ǥ��
    private void ChangeHPAfterUI()
    {
        preHp = Mathf.Lerp(preHp, curHp, Time.deltaTime * 8f);
        afterImage.value = preHp / maxHp;
    }
}