using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchQuestions : MonoBehaviour
{
    [SerializeField] Timer gameTimer;
    [SerializeField] TMP_Text timerText;
    [SerializeField] float secondsAddOnSucces = 10;
    [SerializeField] TMP_Text geographyQuestionDisplay;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] Image displayImage;
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] AudioClip clipCorrect;

    private List<string> questions;
    private int score = 0;

    //public delegate void MatchEvents();
    //public static event MatchEvents pointScored;

    void Start()
    {
        questions = new List<string>();
        questions.Add("Which flags and capital is United Kingdom?");
        //questions.Add("Which flags and capital is Italy?");
        questions.Add("Which flags and capital is Sweden?");
        questions.Add("Which flags and capital is Netherland?");
        questions.Add("Which flags and capital is Spain?");
        int i = Random.Range(0, questions.Count);
        geographyQuestionDisplay.text = questions[i];
    }

    void Update()
    {
        timerText.text = Mathf.FloorToInt(gameTimer.TimeRemaining).ToString();
    }

    void OnEnable()
    {
        Player.PlayerHasAnswered += MatchAnswer;
        Timer.TimerExpired += EndGame;
    }

    void OnDisable()
    {
        Player.PlayerHasAnswered += MatchAnswer;
        Timer.TimerExpired += EndGame;
    }

    private void MatchAnswer()
    {
        if (player1.ChosenAnswer && player2.ChosenAnswer == null)
        {
            return;
        }

        if (questions[0] == geographyQuestionDisplay.text)
        {
            player1.ChosenAnswer.ChangeNumber(1);
            player2.ChosenAnswer.ChangeNumber(0);
            Debug.Log(player1.ChosenAnswer);
            Debug.Log(player2.ChosenAnswer);

            if (player1.ChosenAnswer.Number == 1 && player2.ChosenAnswer.Number == 0)
            {
                Debug.Log("UK");
                CorrectAnswer();
            }
        }
        else if (questions[1] == geographyQuestionDisplay.text)
        {
            player1.ChosenAnswer.ChangeNumber(2);
            player2.ChosenAnswer.ChangeNumber(1);
            Debug.Log(player1.ChosenAnswer);
            Debug.Log(player2.ChosenAnswer);

            if (player1.ChosenAnswer.Number == 2 && player2.ChosenAnswer.Number == 1)
            {
                Debug.Log("Sweden");
                CorrectAnswer();
            }
        }
        else if (questions[2] == geographyQuestionDisplay.text)
        {
            player1.ChosenAnswer.ChangeNumber(0);
            player2.ChosenAnswer.ChangeNumber(2);
            Debug.Log(player1.ChosenAnswer);
            Debug.Log(player2.ChosenAnswer);

            if (player1.ChosenAnswer.Number == 0 && player2.ChosenAnswer.Number == 2)
            {
                Debug.Log("NL");
                CorrectAnswer();
            }
        }
        else if (questions[3] == geographyQuestionDisplay.text)
        {
            player1.ChosenAnswer.ChangeNumber(3);
            player2.ChosenAnswer.ChangeNumber(3);
            Debug.Log(player1.ChosenAnswer);
            Debug.Log(player2.ChosenAnswer);

            if (player1.ChosenAnswer.Number == 3 && player2.ChosenAnswer.Number == 3)
            {
                Debug.Log("Spain");
                CorrectAnswer();
            }
        }
    }

    private void NextAnswer()
    {
        int i = Random.Range(0, questions.Count);
        geographyQuestionDisplay.text = questions[i];
    }

    private void CorrectAnswer()
    {
        displayImage.color = Color.green;
        uiAudioSource.PlayOneShot(clipCorrect);
        gameTimer.AddTime(secondsAddOnSucces);
        score++;
        player1.ChosenAnswer.PressReset();
        player2.ChosenAnswer.PressReset();
        scoreText.text = score.ToString();
    }

    private void EndGame()
    {
        SceneManager.LoadScene(1);
    }
}
