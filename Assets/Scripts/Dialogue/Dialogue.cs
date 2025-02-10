using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public List<string> _dialogues;
    public float textSpeed = 0.1f;
    //public PlayerInteract playerInteract;

    private int index;

    private Canvas _dialogueCanvas;

    void Start()
    {
        dialogueText.text = string.Empty;
        //gameObject.SetActive(false);

        _dialogueCanvas = transform.parent.GetComponentInChildren<Canvas>();
        _dialogueCanvas.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //StartDialogue();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if dialogue box has finished typing all characters
            if (dialogueText.text == _dialogues[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                //immediately show the whole string for the current index
                dialogueText.text = _dialogues[index];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dialogueCanvas.gameObject.SetActive(true);
            if (index == 0)
            {
                StartDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _dialogueCanvas.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        StopAllCoroutines();
        dialogueText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in _dialogues[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < _dialogues.Count - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            _dialogueCanvas.gameObject.SetActive(false);
        }
        /*
        index++;
        if (_dialogues[index] == "")
        {
            dialogueText.text = string.Empty;
            //playerInteract.MakeDialogueBoxInactive();
            //gameObject.SetActive(false);
            _dialogueCanvas.gameObject.SetActive(false);
        }
        else
        {
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        */
    }

    public void SkipDialogue()
    {
        dialogueText.text = string.Empty;
        //playerInteract.MakeDialogueBoxInactive();
        gameObject.SetActive(false);
    }
}
