using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XNode;

public class DialogueTrigger : MonoBehaviour
{
    // could've probably done better by adding a supermanager, but this is already a stitching job, and it doesn't matter for now
    public DialogueTree tree;
    public Button interactButton;
    public GameObject dialogue_pop_up;

    public void TriggerDialog()
    {
        dialogue_pop_up.SetActive(false);
        FindAnyObjectByType<DialogueManager>().StartDialogue(tree.nodes[0]);       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactButton.gameObject.SetActive(true);
            interactButton.onClick.AddListener(this.TriggerDialog);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactButton.onClick.RemoveListener(this.TriggerDialog);
            interactButton.gameObject.SetActive(false);
        }
    }
}
