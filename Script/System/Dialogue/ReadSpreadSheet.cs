using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ReadSpreadSheet : MonoBehaviour
{
    private const string spreadSheetURL = "https://docs.google.com/spreadsheets/d/1qUaobVFt9pj3BaDbAQAHjnOD9yyasFep-4rz-QBoq8E/export?format=tsv&range=A2:D";
    public string sheetData;
    [SerializeField]
    private DialogueManager dialogueManager;

    //���߿� ���� ��
    private void Start()
    {
        //StartCoroutine(LoadSpreadSheet());
    }
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "1 LoadScene")
        {
            StartCoroutine(LoadSpreadSheet());
        }
    }
    private IEnumerator LoadSpreadSheet()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(spreadSheetURL))
        {
            yield return www.SendWebRequest();
            if (www.isDone)
            {
                sheetData = www.downloadHandler.text;
                //���̾�α� �����Ͱ� ���ٸ�
                if (dialogueManager.dialogue.dialogues.Count == 0)
                {
                    dialogueManager.InputDialogueContexts(sheetData);
                }
            }
        }
    }
}
