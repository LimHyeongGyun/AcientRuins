using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldElf : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    private DialogueManager dialogueManager;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void Update()
    {
        AnimationController();
    }

    private void AnimationController()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Talk"))
        {
            if (!dialogueManager.activeUI && dialogueManager.lastContext && player.interactive)
            {
                player.interactive = false;
                TalkEnd();
            }
        }
    }
    public void EnterdTalk()
    {
        animator.SetBool("Talk", true);
        dialogueManager.FindContext("¥ƒ¿∫ ø§«¡øÕ ¥Î»≠ Ω√¿€");
    }
    private void TalkEnd()
    {
        player.interactive = false;
        animator.SetBool("Talk", false);
    }
}
