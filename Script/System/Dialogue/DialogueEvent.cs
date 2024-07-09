using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("이벤트")]
    public string situationEvent;

    [Tooltip("ID")]
    public List<ID> contextId = new List<ID>();
}
[System.Serializable]
public class ID
{
    [Tooltip("문단 ID")]
    public string id;
    [Tooltip("캐릭터 이름")]
    public string name;
    [Tooltip("대사")]
    public List<string> contexts = new List<string>();
}
[CreateAssetMenu(fileName = "DialogueEvent", menuName = "SO/DialogueSO")]
public class DialogueEvent : ScriptableObject
{
    public List<Dialogue> dialogues = new List<Dialogue>();
}