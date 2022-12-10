using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField]
    private NPCConversation conversation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ConversationManager.Instance.EndConversation();
        }
    }
}
