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
    [SerializeField] float secondsSubtractOnFail = 10;
    [SerializeField] float newEquationInterval = 3;
    [SerializeField] TMP_Text geographyQuestionDisplay;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] Image displayImage;
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] AudioClip clipCorrect;
    [SerializeField] AudioClip clipWrong;

    private List<string> questions;
    private List<string> compareQuestions;
    private int score = 0;
    private int questionNumber = 0;

    //public delegate void MatchEvents();
    //public static event MatchEvents pointScored;

    void Start()
    {
        questions = new List<string>();
        compareQuestions = new List<string>();

        questions.Add("Which flags and capital is United Kingdom?");
        //questions.Add("Which flags and capital is Italy?");
        questions.Add("Which flags and capital is Sweden?");
        questions.Add("Which flags and capital is Netherland?");
        questions.Add("Which flags and capital is Spain?");

        compareQuestions = questions;

        foreach(string c in compareQuestions)
        {
            Debug.Log(c.ToString());
        }

        for (int i = 0; i < questions.Count; i++) 
        {
            string temp = questions[i];
            int randomIndex = Random.Range(i, questions.Count);
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
        geographyQuestionDisplay.text = questions[0];
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
        Player.PlayerHasAnswered -= MatchAnswer;
        Timer.TimerExpired -= EndGame;
    }

    private void MatchAnswer()
    {
        if (player1.ChosenAnswer && player2.ChosenAnswer == null)
        {
            return;
        }

        gameTimer.Pause();

        //if (questions[0] == geographyQuestionDisplay.text)
        /* for(int a = 0; a < questions.Count; a++)
        {
            if(compareQuestions[a] == questions[a])
            {
                
            }
        } */

        if("Which flags and capital is United Kingdom?" == geographyQuestionDisplay.text)
        {

            if (player1.ButtonNumber == 1 && player2.ButtonNumber == 0)
            //if (player1.ChosenAnswer.ToString() == "Button1 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button0 (AnswerButton)")
            {
                Debug.Log("UK");
                CorrectAnswer();
            }
            else
            {
                Debug.Log("Wrong");
                WrongAnswer();
            }
        }
        //else if (questions[1] == geographyQuestionDisplay.text)
        else if("Which flags and capital is Sweden?" == geographyQuestionDisplay.text)
        {

            if (player1.ButtonNumber == 2 && player2.ButtonNumber == 1)
            //if (player1.ChosenAnswer.ToString() == "Button2 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button1 (AnswerButton)")
            {
                Debug.Log("Sweden");
                CorrectAnswer();
            }
            else
            {
                Debug.Log("Wrong");
                WrongAnswer();
            }
        }
        //else if (questions[2] == geographyQuestionDisplay.text)
        else if("Which flags and capital is Netherland?" == geographyQuestionDisplay.text)
        {

            if (player1.ButtonNumber == 0 && player2.ButtonNumber == 2)
            //if (player1.ChosenAnswer.ToString() == "Button0 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button2 (AnswerButton)")
            {
                Debug.Log("NL");
                CorrectAnswer();
            }
            else
            {
                Debug.Log("Wrong");
                WrongAnswer();
            }
        }
        //else if (questions[3] == geographyQuestionDisplay.text)
        else if("Which flags and capital is Spain?" == geographyQuestionDisplay.text)
        {

            if (player1.ButtonNumber == 3 && player2.ButtonNumber == 3)
            //if (player1.ChosenAnswer.ToString() == "Button3 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button3 (AnswerButton)")
            {
                Debug.Log("Spain");
                CorrectAnswer();
            }
            else
            {
                Debug.Log("Wrong");
                WrongAnswer();
            }
        }

        StartCoroutine(ResetInterval());
    }

    IEnumerator ResetInterval()
    {
        yield return new WaitForSeconds(newEquationInterval);
        NextQuestion();
        gameTimer.Resume();
    }

    private void NextQuestion()
    {
        questionNumber++;
        player1.ChosenAnswer.PressReset();
        player2.ChosenAnswer.PressReset();
        player1.ChosenAnswer = null;
        player2.ChosenAnswer = null;
        player1.ResetPlayer();
        player2.ResetPlayer();
        displayImage.color = Color.white;
        geographyQuestionDisplay.text = questions[questionNumber];
    }

    private void CorrectAnswer()
    {
        displayImage.color = Color.green;
        uiAudioSource.PlayOneShot(clipCorrect);
        gameTimer.AddTime(secondsAddOnSucces);
        score++;
        scoreText.text = score.ToString();
    }

    private void WrongAnswer()
    {
        displayImage.color = Color.red;
        uiAudioSource.PlayOneShot(clipWrong);
        gameTimer.SubtractTime(secondsSubtractOnFail);
    }

    private void EndGame()
    {
        SceneManager.LoadScene(1);
    }
}
