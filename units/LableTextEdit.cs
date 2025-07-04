using Godot;
using System;

public partial class LableTextEdit : MarginContainer
{
    [ExportGroup("Internal")]
    [Export]
    Label label_Item;
    [Export]
    TextEdit textEdit_Item;

    public void Init(string name, string text, string placeholderText = "")
    {
        label_Item.Text = name;
        textEdit_Item.Text = text;
        textEdit_Item.PlaceholderText = placeholderText;
    }

    public string GetText()
    {
        return textEdit_Item.Text;
    }



}
