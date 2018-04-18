using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PromptMsg
{
    public string txt;
    public Color color;

    public PromptMsg()
    {

    }
    public PromptMsg(string text, Color color)
    {
        this.txt = text;
        this.color = color;
    }

    public void ChangeText(string text, Color color)
    {
        this.txt = text;
        this.color = color;
    }
}

