using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AnswerButton[] Buttons;
    public bool HasAnswered = false;
    public AnswerButton ChosenAnswer;

    public delegate void PlayerEvents();
    public static event PlayerEvents PlayerHasAnswered;

    public void ButtonPressed(int buttonIndex)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (buttonIndex == i)
            {
                Buttons[i].Press();
                ChosenAnswer = Buttons[i];
            }
            else Buttons[i].PressReset();
        }
        HasAnswered = true;
        if (PlayerHasAnswered != null) PlayerHasAnswered.Invoke();
    }

    public void ResetPlayer()
    {
        foreach (AnswerButton button in Buttons) button.IsPressed = false;
        HasAnswered = false;
    }
}
