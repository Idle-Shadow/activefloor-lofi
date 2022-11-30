using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MatchDirector : MonoBehaviour
{
    public float NewEquationInterval = 3;
    public float SecondsAddOnSucces = 10;
    public float SecondsSubtractOnFail = 10;
    public int Score { get; private set; } = 0;

    public Player Player1;
    public Player Player2;
    public Timer GameTimer;
    public EquationGenerator EquationGenerator;

    public TextMeshProUGUI EquationText;
    public Image DisplayImage;
    public TextMeshProUGUI DebugText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;

    public AudioSource UIAudioSource;
    public AudioClip ClipCorrect;
    public AudioClip ClipWrong;

    public delegate void MatchEvents();
    public static event MatchEvents PointScored;

    (int, int, int) _currentEquation;
    bool _player2HasResult;

    void Start()
    {
        NextEquation();
    }

    void OnEnable()
    {
        Player.PlayerHasAnswered += CheckAnswers;
        Timer.TimerExpired += EndGame;
    }

    void OnDisable()
    {
        Player.PlayerHasAnswered -= CheckAnswers;
        Timer.TimerExpired -= EndGame;
    }

    void Update()
    {
        TimerText.text = Mathf.FloorToInt(GameTimer.TimeRemaining).ToString();
    }

    void NextEquation()
    {
        _currentEquation = EquationGenerator.GenerateEquation();
        InitialisePlayerAnswers(Player1, _currentEquation.Item1);

        _player2HasResult = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        if (_player2HasResult) InitialisePlayerAnswers(Player2, _currentEquation.Item3);
        else InitialisePlayerAnswers(Player2, _currentEquation.Item2);

        EquationText.text = String.Format("<b><color=blue>P1</color></b> {0} {1} = {2}",
            EquationGenerator.CurrentMode.StringRepresentation,
            _player2HasResult ? _currentEquation.Item2 : "<b><color=red>P2</color></b>",
            _player2HasResult ? "<b><color=red>P2</color></b>" : _currentEquation.Item3);
        DebugText.text = $"{_currentEquation.Item1} {EquationGenerator.CurrentMode.StringRepresentation} {_currentEquation.Item2} = {_currentEquation.Item3}";
        DisplayImage.color = Color.white;

        Player1.ResetPlayer();
        Player2.ResetPlayer();
    }

    void InitialisePlayerAnswers(Player player, int correctAnswer)
    {
        int amountButtons = player.Buttons.Length;
        int correctAnswerOn = UnityEngine.Random.Range(0, amountButtons);

        for (int i = 0; i < amountButtons; i++)
        {
            if (i == correctAnswerOn) player.Buttons[i].ChangeNumber(correctAnswer);
            else
            {
                bool positionFilled = false;
                while (!positionFilled)
                {
                    int floor;
                    int ceil;
                    if (EquationGenerator.CurrentMode.Mode == OperatorMode.subtract && _player2HasResult)
                    {
                        floor = correctAnswer - 5 - Mathf.FloorToInt(correctAnswer * .2f);
                        ceil = correctAnswer + 5 + Mathf.FloorToInt(correctAnswer * .2f);
                    }
                    else
                    {
                        floor = Mathf.Max(correctAnswer - Mathf.FloorToInt(correctAnswer * .2f), 1);
                        ceil = Mathf.Max(correctAnswer + Mathf.FloorToInt(correctAnswer * .2f), 20);
                    }
                    int candidate = UnityEngine.Random.Range(floor, ceil);

                    bool numberAlreadyPresent = false;
                    foreach (AnswerButton button in player.Buttons)
                    {
                        if (candidate == button.Number) numberAlreadyPresent = true;
                    }

                    if (!numberAlreadyPresent && candidate != correctAnswer)
                    {
                        player.Buttons[i].ChangeNumber(candidate);
                        positionFilled = true;
                    }
                }
            }
        }
    }

    void CheckAnswers()
    {
        if (!Player1.HasAnswered || !Player2.HasAnswered) return;

        GameTimer.Pause();
        EquationText.text = $"{_currentEquation.Item1} {EquationGenerator.CurrentMode.StringRepresentation} {_currentEquation.Item2} = {_currentEquation.Item3}";
        int[] equationElements = new int[3] {
            Player1.ChosenAnswer.Number,
            _player2HasResult ? _currentEquation.Item2 : Player2.ChosenAnswer.Number,
            _player2HasResult ? Player2.ChosenAnswer.Number : _currentEquation.Item3
        };

        bool check = false;
        switch (EquationGenerator.CurrentMode.Mode)
        {
            case OperatorMode.add:
                check = equationElements[0] + equationElements[1] == equationElements[2];
                break;
            case OperatorMode.multiply:
                check = equationElements[0] * equationElements[1] == equationElements[2];
                break;
            case OperatorMode.subtract:
                check = equationElements[0] - equationElements[1] == equationElements[2];
                break;
            case OperatorMode.divide:
                check = equationElements[0] / equationElements[1] == equationElements[2];
                break;
        }

        if (check) AnswerCorrect();
        else AnswerWrong();

        StartCoroutine(ResetInterval());
    }

    IEnumerator ResetInterval()
    {
        yield return new WaitForSeconds(NewEquationInterval);
        NextEquation();
        GameTimer.Resume();
    }

    void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    void AnswerCorrect()
    {
        DisplayImage.color = Color.green;
        UIAudioSource.PlayOneShot(ClipCorrect);
        GameTimer.AddTime(SecondsAddOnSucces);
        Score++;
        ScoreText.text = Score.ToString();
        if (PointScored != null) PointScored.Invoke();
    }

    void AnswerWrong()
    {
        DisplayImage.color = Color.red;
        UIAudioSource.PlayOneShot(ClipWrong);
        GameTimer.SubtractTime(SecondsSubtractOnFail);
    }
}
