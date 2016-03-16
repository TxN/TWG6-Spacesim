using UnityEngine;
using System.Collections;

public class ChatMessage 
{
    public string text;
    public Color color;

    public ChatMessage(string t, Color col)
    {
        text = t;
        color = col;
    }

}
