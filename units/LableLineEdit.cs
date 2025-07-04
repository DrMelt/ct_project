using Godot;
using System;

public partial class LableLineEdit : MarginContainer
{
    [ExportGroup("Internal")]
    [Export]
    Label label_Item;
    [Export]
    LineEdit lineEdit_Item;

    public void Init(string name, string text, string placeholderText = "")
    {
        label_Item.Text = name;
        lineEdit_Item.Text = text;
        lineEdit_Item.PlaceholderText = placeholderText;
    }

    public string GetText()
    {
        return lineEdit_Item.Text;
    }

}
