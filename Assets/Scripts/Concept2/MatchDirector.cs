using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MatchDirector : MonoBehaviour
{
    public int EquationAmount = 10;
    public int HighestNumberAddition = 100;
    public int HighestNumberMultiplication = 20;
    public float NewEquationInterval = 3;
    public float SecondsAddOnSucces = 10;
    public float SecondsSubtractOnFail = 10;

    public Player Player1;
    public Player Player2;
    public Timer GameTimer;

    public TextMeshProUGUI EquationText;
    public Image DisplayImage;
    public TextMeshProUGUI DebugText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;

    public AudioSource UIAudioSource;
    public AudioClip ClipCorrect;
    public AudioClip ClipWrong;

    List<(int, int, int)> _equations;
    (int, int, int) _currentEquation;
    int _currentEquationIndex = 0;
    OperatorType _operatorType;
    bool _player2HasResult;
    int _highestNumberForRound;
    int _score = 0;

    void Start()
    {
        EquationGenerator generator = new();
        _operatorType = (OperatorType)UnityEngine.Random.Range(0, 2);
        _highestNumberForRound = _operatorType == OperatorType.add ? HighestNumberAddition : HighestNumberMultiplication;
        _equations = generator.GenerateEquations(EquationAmount, _highestNumberForRound, _operatorType);
        NextEquation();
    }

    void OnEnable() {
        Player.PlayerHasAnswered += CheckAnswers;
        Timer.TimerExpired += EndGame;
    }

    void OnDisable() {
        Player.PlayerHasAnswered -= CheckAnswers;
        Timer.TimerExpired -= EndGame;
    }

    void Update()
    {
        TimerText.text = Mathf.FloorToInt(GameTimer.timeRemaining).ToString();
    }

    void NextEquation()
    {
        if (_currentEquationIndex >= EquationAmount)
        {
            EndGame();
            return;
        }

        _currentEquation = _equations[_currentEquationIndex];
        InitialisePlayerAnswers(Player1, _currentEquation.Item1);

        _player2HasResult = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        if (_player2HasResult) InitialisePlayerAnswers(Player2, _currentEquation.Item3);
        else InitialisePlayerAnswers(Player2, _currentEquation.Item2);

        string operatorAsString = _operatorType == OperatorType.add ? "+" : "x";
        EquationText.text = String.Format("<b><color=blue>P1</color></b> {0} {1} = {2}",
            operatorAsString,
            _player2HasResult ? _currentEquation.Item2 : "<b><color=red>P2</color></b>",
            _player2HasResult ? "<b><color=red>P2</color></b>" : _currentEquation.Item3);
        DebugText.text = $"{_currentEquation.Item1} {operatorAsString} {_currentEquation.Item2} = {_currentEquation.Item3}";
        DisplayImage.color = Color.white;

        Player1.ResetPlayer();
        Player2.ResetPlayer();

        _currentEquationIndex++;
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
                    int randomNumber = UnityEngine.Random.Range(1, _highestNumberForRound);
                    bool numberAlreadyPresent = false;
                    foreach (AnswerButton button in player.Buttons)
                    {
                        if (randomNumber == button.Number) numberAlreadyPresent = true;
                    }
                    if (!numberAlreadyPresent)
                    {
                        player.Buttons[i].ChangeNumber(randomNumber);
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
        string operatorAsString = _operatorType == OperatorType.add ? "+" : "x";
        EquationText.text = $"{_currentEquation.Item1} {operatorAsString} {_currentEquation.Item2} = {_currentEquation.Item3}";

        if ((!_player2HasResult && Player1.ChosenAnswer.Number == _currentEquation.Item1 &&
            Player2.ChosenAnswer.Number == _currentEquation.Item2) || 
            (_player2HasResult && Player1.ChosenAnswer.Number == _currentEquation.Item1 &&
            Player2.ChosenAnswer.Number == _currentEquation.Item3))
        {
            AnswerCorrect();
        }
        else
        {
            AnswerWrong();
        }

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
        _score++;
        ScoreText.text = _score.ToString();
    }

    void AnswerWrong()
    {
        DisplayImage.color = Color.red;
        UIAudioSource.PlayOneShot(ClipWrong);
        GameTimer.SubtractTime(SecondsSubtractOnFail);
    }
}
