using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : MonoBehaviour
{
    public Player player;
    private DialogueManager dialogueManager;
    private UIManager uiManager;
    public DwarfUI dwarfUI;

    private Animator animator;
    public bool talk;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorControl();
    }
    private void AnimatorControl()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShakeHand"))
        {
            if (!dialogueManager.activeUI && !dialogueManager.lastContext && !talk && player.interactive)
            {
                talk = true;
                dwarfUI = FindObjectOfType<DwarfUI>();
                dwarfUI.ActiveDwarfUI();
                uiManager.ActiveCursor();
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.64f)
            {
                animator.SetTrigger("Start");
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("StartCrossArm"))
        {
            if (!dialogueManager.activeUI && !dialogueManager.lastContext && !talk && player.interactive)
            {
                talk = true;
                dwarfUI = FindObjectOfType<DwarfUI>();
                dwarfUI.ActiveDwarfUI();
                uiManager.ActiveCursor();
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && dwarfUI.activeUI)
            {
                animator.ResetTrigger("Start");
                TalkAnimation();
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("CrossArm"))
        {
            //방패 수리를 클릭 했을 때
            if (!dialogueManager.activeUI && !dialogueManager.lastContext && dwarfUI.fix)
            {
                dwarfUI.fix = false;
                dwarfUI.Fix();
            }
        }
    }
    public void EnterdTalk()
    {
        animator.SetTrigger("Meet");
        dialogueManager.FindContext("드워프와 대화 시작시");
    }
    public void TalkAnimation()
    {
        animator.SetBool("Talk", dwarfUI.activeUI);
    }
}
