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
            FindContext("플레이어 게임 새 게임 시작시");
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
        //행 단위로 쪼개기
        string[] row = tsv.Split("\n");

        foreach (string context in row)
        {
            string[] content = context.Split("\t"); //열 단위로 쪼개서 담기
            //새로운 상황 찾았을 때
            if (content[0] != "")
            {
                //배열의 길이 추가하기
                Dialogue _dialogue = new Dialogue();
                dialogue.dialogues.Add(_dialogue);
                dialogue.dialogues[^1].situationEvent = content[0]; //상황 집어넣기
            }
            //같은 상황일 때
            else if (content[0] == "")
            {
                //새로운 ID 일 때
                if (content[1] != "")
                {
                    ID contextID = new ID();
                    dialogue.dialogues[^1].contextId.Add(contextID);
                    dialogue.dialogues[^1].contextId[^1].id = content[1]; //ID 입력
                    dialogue.dialogues[^1].contextId[^1].name = content[2]; //캐릭터 이름 입력
                    dialogue.dialogues[^1].contextId[^1].contexts.Add(content[3]); //문단의 첫 대사 입력
                }
                //같은 ID일 때
                else if (content[1] == "")
                {
                    //다음 대사 집어넣기
                    dialogue.dialogues[^1].contextId[^1].contexts.Add(content[3]);
                }
            }
        }
    }
    //사용할 대화 담기
    public void FindContext(string _event)
    {
        useDialogue = InputContext(_event);
        PrintContext();
    }
    public Dialogue InputContext(string _event)
    {
        Dialogue useContext = new Dialogue();
        //해당하는 이벤트 찾기
        foreach (Dialogue _dialogue in dialogue.dialogues)
        {
            //해당 이벤트를 찾았다면
            if (_event == _dialogue.situationEvent)
            {
                useContext = _dialogue;
                break;
            }
        }
        return useContext;
    }
    /// <summary>
    /// 대화 시작할 때 호출 및 문단 넘어갈 때 호출
    /// </summary>
    public void PrintContext()
    {
        //ID가 변경되었다면
        if (useDialogue.contextId[contextNum].id != curId)
        {
            //다른 아이디라면 현재 컨텍스트 아이디 변경
            curId = useDialogue.contextId[contextNum].id;
            count = 0;
            //현재 아이디의 이름으로 캐릭터 이름 바꾸기
            nameText.text = useDialogue.contextId[contextNum].name;
            contextText.text = ""; //문장 초기화
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
        //마지막 문장이 아닐 때 다음 문장 호출해주기
        if (useDialogue.contextId[contextNum].contexts[count] != useDialogue.contextId[contextNum].contexts[^1])
        {
            count++;
            PrintContext();
        }
        //마지막 문장일 때
        else if (useDialogue.contextId[contextNum].contexts[count] == useDialogue.contextId[contextNum].contexts[^1])
        {
            space = true;
            //대화의 마지막 문단일 때
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