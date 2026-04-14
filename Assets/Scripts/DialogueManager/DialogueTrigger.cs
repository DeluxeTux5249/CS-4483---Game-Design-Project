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
    public PlayerInput playerInput;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.Log("Couldn't find a player");
        }
    }

    public void TriggerDialog(InputAction.CallbackContext context)
    {
        dialogue_pop_up.SetActive(false);
        FindAnyObjectByType<DialogueManager>().StartDialogue(tree.nodes[0]);       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(playerInput.actions.FindActionMap("Player").FindAction("Interact"));
            playerInput.actions.FindActionMap("Player").FindAction("Interact").performed += TriggerDialog;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInput.actions.FindActionMap("Player").FindAction("Interact").performed -= TriggerDialog;
        }
    }

    private void OnDestroy()
    {
        // not needed; player is destroyed tween loads
        //playerInput.actions.FindActionMap("Player").FindAction("Interact").performed -= TriggerDialog;
    }

}
