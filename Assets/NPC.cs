using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDiaLogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Return)) 
        {
            Debug.Log("Nhấn Enter khi đang gần NPC");
            Interact();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Đảm bảo Player có tag là "Player"
        {
            isPlayerNearby = true;
            Debug.Log("Player đã vào vùng tương tác.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player rời khỏi vùng tương tác.");
        }
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (!isDialogueActive)
        {
            StartDiaLogue();
        }
        else
        {
            NextLine();
        }
    }


    void StartDiaLogue()
    {
        if (dialogueData == null || dialogueData.dialogueLine == null || dialogueData.dialogueLine.Length == 0)
        {
            Debug.LogError("NPCDialogue chưa được gán hoặc không có hội thoại.");
            return;
        }

        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }


    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.SetText("");

        foreach (char letter in dialogueData.dialogueLine[dialogueIndex].ToCharArray())
        {
            dialogText.SetText(dialogText.text + letter);
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
        if (dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogText.SetText(dialogueData.dialogueLine[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLine.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogText.SetText("");
        dialoguePanel.SetActive(false);
    }
}
