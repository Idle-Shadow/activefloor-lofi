using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AnswerButton : MonoBehaviour
{
    public bool IsPressed = false;
    public int Number { get; private set; } = 0;
    public TextMeshProUGUI Text;
    public Image Image;
    public Color PressedColor;
    public int ButtonNumber {get; set;} = 0;
    public Button button;

    public void ChangeNumber(int number)
    {
        Number = number;
        Text.text = number.ToString();
        PressReset();
    }

    public void SetActive(bool active)
    {
        button.enabled = active;
    }

    public void Press()
    {
        IsPressed = true;
        ChangeColor(PressedColor);
    }

    public void ResetButton()
    {
        Text.text = "";
        Image.sprite = null;
    }

    public void ChangeColor(Color color)
    {
        Image.color = color;
    }

    public void PressReset()
    {
        IsPressed = false;
        ChangeColor(Color.white);
    }

    public void SetSpriteImage(Sprite sprite)
    {
        Image.sprite = sprite;
    }

    public void SetCapitalName(string capital)
    {
        Text.text = capital;
    }

    internal void SetCountryName(string name)
    {
        Text.text = name;
    }

    public void SetCountryFlag(string svg)
    {
        Debug.Log(svg);
    }
}
