using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed { get; private set; } = false;
    public int Number { get; private set; } = 0;
    public TextMeshProUGUI Text;
    public Image Image;
    public Sprite DefaultImage;
    public Color PressedColor;
    public Button button;
    public delegate void AnswerButtonEvents();
    public static event AnswerButtonEvents ButtonPressed;

    Player _player;

    void OnEnable()
    {
        _player = GetComponentInParent<Player>();
    }

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
        _player.ButtonPressed(this);
        if (ButtonPressed != null) ButtonPressed.Invoke();
    }

    public void ResetButton()
    {
        Text.text = "";
        Image.sprite = DefaultImage;
        Image.color = Color.white;
    }

    public void ChangeColor(Color color)
    {
        Image.color = color;
    }

    public void PressReset()
    {
        IsPressed = false;
        ChangeColor(Color.white);
        _player.ButtonReleased();
    }

    public void SetSpriteImage(Sprite sprite, string name)
    {
        Image.sprite = sprite;
        Image.name = name;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.enabled) Press();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.enabled && Application.platform == RuntimePlatform.Android)
            PressReset();
    }
}
