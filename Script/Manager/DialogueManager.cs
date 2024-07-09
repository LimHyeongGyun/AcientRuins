using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private ReadSpreadSheet spreadsheet;
    [SerializeField]
    private GameManager gameManager;

    public bool activeUI;
    public GameObject DialogueUI;
    public DialogueEvent dialogue;

    public Text nameText;
    public Text contextText;
    private int contextNum;
    public bool lastContext;
    [SerializeField]
    private Dialogue useDialogue;

    bool space;
    int count;
    string curId;

    private WaitForSeconds timelag1 = new WaitForSeconds(0.001f);
    private void Awake()
    {
        space = true;
    }
    public void SetDialogue()
    {
        DialogueUI = GameObject.FindGameObjectWithTag("Dialogue");
        contextText = DialogueUI.transform.GetChild(0).GetComponent<Text>();
        nameText = DialogueUI.transform.GetChild(1).GetComponentInChildren<Text>();
        if (!gameManager.saveGame)
        {
            gameManager.saveGame = true;
            DialogueUI = GameObject.FindGameObjectWithTag("Dialogue");
            contextText = DialogueUI.transform.GetChild(0).GetComponent<Text>();
            nameText = DialogueUI.transform.GetChild(1).GetComponentInChildren<Text>();
            FindContext("�÷��̾� ���� �� ���� ���۽�");
        }
        else if (gameManager.saveGame)
        {
            activeUI = true;
        }
        ActiveUI();
    }
    public void ActiveUI()
    {
        activeUI = !activeUI;
        DialogueUI.SetActive(activeUI);
    }
    void Update()
    {
        if (activeUI && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (!lastContext && space)
            {
                PrintContext();
            }
            else if (lastContext)
            {
                ActiveUI();
                useDialogue = null;
                lastContext = false;
            }
            
        }
    }
    public void InputDialogueContexts(string tsv)
    {
        //�� ������ �ɰ���
        string[] row = tsv.Split("\n");

        foreach (string context in row)
        {
            string[] content = context.Split("\t"); //�� ������ �ɰ��� ���
            //���ο� ��Ȳ ã���� ��
            if (content[0] != "")
            {
                //�迭�� ���� �߰��ϱ�
                Dialogue _dialogue = new Dialogue();
                dialogue.dialogues.Add(_dialogue);
                dialogue.dialogues[^1].situationEvent = content[0]; //��Ȳ ����ֱ�
            }
            //���� ��Ȳ�� ��
            else if (content[0] == "")
            {
                //���ο� ID �� ��
                if (content[1] != "")
                {
                    ID contextID = new ID();
                    dialogue.dialogues[^1].contextId.Add(contextID);
                    dialogue.dialogues[^1].contextId[^1].id = content[1]; //ID �Է�
                    dialogue.dialogues[^1].contextId[^1].name = content[2]; //ĳ���� �̸� �Է�
                    dialogue.dialogues[^1].contextId[^1].contexts.Add(content[3]); //������ ù ��� �Է�
                }
                //���� ID�� ��
                else if (content[1] == "")
                {
                    //���� ��� ����ֱ�
                    dialogue.dialogues[^1].contextId[^1].contexts.Add(content[3]);
                }
            }
        }
    }
    //����� ��ȭ ���
    public void FindContext(string _event)
    {
        useDialogue = InputContext(_event);
        PrintContext();
    }
    public Dialogue InputContext(string _event)
    {
        Dialogue useContext = new Dialogue();
        //�ش��ϴ� �̺�Ʈ ã��
        foreach (Dialogue _dialogue in dialogue.dialogues)
        {
            //�ش� �̺�Ʈ�� ã�Ҵٸ�
            if (_event == _dialogue.situationEvent)
            {
                useContext = _dialogue;
                break;
            }
        }
        return useContext;
    }
    /// <summary>
    /// ��ȭ ������ �� ȣ�� �� ���� �Ѿ �� ȣ��
    /// </summary>
    public void PrintContext()
    {
        //ID�� ����Ǿ��ٸ�
        if (useDialogue.contextId[contextNum].id != curId)
        {
            //�ٸ� ���̵��� ���� ���ؽ�Ʈ ���̵� ����
            curId = useDialogue.contextId[contextNum].id;
            count = 0;
            //���� ���̵��� �̸����� ĳ���� �̸� �ٲٱ�
            nameText.text = useDialogue.contextId[contextNum].name;
            contextText.text = ""; //���� �ʱ�ȭ
        }
        StartCoroutine(PrintTextTimeLag(useDialogue.contextId[contextNum].contexts[count]));
    }
    IEnumerator PrintTextTimeLag(string context)
    {
        space = false;
        string text = context.ToString();
        for (int i = 0; i < text.Length - 1; i++)
        {
            contextText.text += text[i].ToString();
            yield return timelag1;
        }
        contextText.text += "\n";
        //������ ������ �ƴ� �� ���� ���� ȣ�����ֱ�
        if (useDialogue.contextId[contextNum].contexts[count] != useDialogue.contextId[contextNum].contexts[^1])
        {
            count++;
            PrintContext();
        }
        //������ ������ ��
        else if (useDialogue.contextId[contextNum].contexts[count] == useDialogue.contextId[contextNum].contexts[^1])
        {
            space = true;
            //��ȭ�� ������ ������ ��
            if (useDialogue.contextId[contextNum] == useDialogue.contextId[^1])
            {
                lastContext = true;
                contextNum = -1;
                curId = "";
            }
            contextNum++;
        }
    }
}