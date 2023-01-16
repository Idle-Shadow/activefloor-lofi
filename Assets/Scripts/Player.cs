using UnityEngine;

public class Player : MonoBehaviour
{
    public AnswerButton[] Buttons;
    public int ActualAnswer = 0;
    public bool HasAnswered = false;
    public AnswerButton ChosenAnswer { get; private set; }

    public void ButtonPressed(AnswerButton answer)
    {
        if (ChosenAnswer != null && Application.platform != RuntimePlatform.Android)
        {
            ChosenAnswer.PressReset();
        }

        ChosenAnswer = answer;
    }

    public void ButtonReleased()
    {
        ChosenAnswer = null;
    }

    public void ResetPlayer()
    {
        foreach (AnswerButton button in Buttons)
        {
            button.PressResetAndRelease();
            button.SetActive(true);
        }
    }

    public void EnableButtons (bool active)
    {
        foreach (AnswerButton button in Buttons) button.SetActive(active);
    }
}
