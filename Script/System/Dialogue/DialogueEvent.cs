using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("�̺�Ʈ")]
    public string situationEvent;

    [Tooltip("ID")]
    public List<ID> contextId = new List<ID>();
}
[System.Serializable]
public class ID
{
    [Tooltip("���� ID")]
    public string id;
    [Tooltip("ĳ���� �̸�")]
    public string name;
    [Tooltip("���")]
    public List<string> contexts = new List<string>();
}
[CreateAssetMenu(fileName = "DialogueEvent", menuName = "SO/DialogueSO")]
public class DialogueEvent : ScriptableObject
{
    public List<Dialogue> dialogues = new List<Dialogue>();
}