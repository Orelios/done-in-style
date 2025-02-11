using System;
using UnityEngine;

public class OLD_DialogueTrigger : MonoBehaviour
{
    private Canvas _dialogueCanvas;

    private bool _showDialogue;
    
    private void Awake()
    {
        _dialogueCanvas = transform.parent.GetComponentInChildren<Canvas>();
        
        _dialogueCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HideDialogue();
    }

    private void ShowDialogue()
    {
        _showDialogue = true;
        _dialogueCanvas.gameObject.SetActive(true);
    }

    private void HideDialogue()
    {
        _showDialogue = false;
        _dialogueCanvas.gameObject.SetActive(false);
    }
}
