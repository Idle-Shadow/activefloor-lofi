using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public bool IsPressed = false;
    public int Number { get; private set; } = 0;
    public TextMeshProUGUI Text;
    public Image Image;
    public Color PressedColor;
    public int ButtonNumber {get; set;} = 0;

    public void ChangeNumber(int number)
    {
        Number = number;
        Text.text = number.ToString();
        PressReset();
    }

    public void Press()
    {
        IsPressed = true;
        ChangeColor(PressedColor);
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
}
