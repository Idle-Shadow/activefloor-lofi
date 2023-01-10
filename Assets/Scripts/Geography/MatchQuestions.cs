using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExtensionMethods;

public class MatchQuestions : MonoBehaviour
{
    [SerializeField] int difficulty = 20;
    [SerializeField] int difficultyIncrease = 2;

    [SerializeField] Timer gameTimer;
    [SerializeField] TMP_Text timerText;
    [SerializeField] float secondsAddOnSucces = 10;
    [SerializeField] float secondsSubtractOnFail = 10;
    [SerializeField] float newEquationInterval = 3;

    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Image questionImage;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] AudioClip clipCorrect;
    [SerializeField] AudioClip clipWrong;
    [SerializeField] Image statusImage;

    private int score = 0;

    [SerializeField] GeographyQuestionGenerator questionGenerator;
    Region region;

    private GeographyQuestion quiz;

    void Start()
    {
        region = RegionPicker.chosenRegion;
        LoadNewQuestion();
        LoadAnswers();
    }

    void LoadNewQuestion()
    {
        quiz = questionGenerator.GenerateQuestion(region, difficulty);

        switch (quiz.questionType)
        {
            case GeographyQuestion.QuestionType.name:
                questionText.gameObject.SetActive(true);
                questionImage.gameObject.SetActive(false);
                questionText.text = quiz.countryQuestion.name;
                break;
            case GeographyQuestion.QuestionType.capital:
                questionText.gameObject.SetActive(true);
                questionImage.gameObject.SetActive(false);
                questionText.text = quiz.countryQuestion.capital;
                break;
            case GeographyQuestion.QuestionType.flag:
                questionText.gameObject.SetActive(false);
                questionImage.gameObject.SetActive(true);
                questionImage.sprite = quiz.countryQuestion.GetFlag();
                break;
            case GeographyQuestion.QuestionType.map:
                questionText.gameObject.SetActive(false);
                questionImage.gameObject.SetActive(true);
                questionImage.sprite = quiz.countryQuestion.GetMap();
                break;
            default:
                Debug.LogError("Non-existing geography question type");
                break;
        }
    }

    void LoadAnswers()
    {
        //LOAD ANSWERS FOR PLAYER 1
        switch (quiz.answerTypeP1)
        {
            case GeographyQuestion.AnswerType.name:
                {
                    List<string> answers = new();
                    foreach (Country c in quiz.countryAnswersP1) answers.Add(c.name);
                    answers.Add(quiz.countryQuestion.name);
                    LoadAnswersText(answers, player1);
                }
                break;
            case GeographyQuestion.AnswerType.capital:
                {
                    List<string> answers = new();
                    foreach (Country c in quiz.countryAnswersP1) answers.Add(c.capital);
                    answers.Add(quiz.countryQuestion.capital);
                    LoadAnswersText(answers, player1);
                }
                break;
            case GeographyQuestion.AnswerType.flag:
                {
                    List<(Sprite, string)> answers = new();
                    foreach (Country c in quiz.countryAnswersP1)
                    {
                        answers.Add((c.GetFlag(), c.code));
                    }

                    answers.Add((quiz.countryQuestion.GetFlag(), quiz.countryQuestion.code));
                    LoadAnswersSprite(answers, player1);
                }
                break;
            default:
                Debug.LogError("Non-existing geography answer type");
                break;
        }

        //LOAD ANSWERS FOR PLAYER 2
        switch (quiz.answerTypeP2)
        {
            case GeographyQuestion.AnswerType.name:
                {
                    List<string> answers = new();
                    foreach (Country c in quiz.countryAnswersP2) answers.Add(c.name);
                    answers.Add(quiz.countryQuestion.name);
                    LoadAnswersText(answers, player2);
                }
                break;
            case GeographyQuestion.AnswerType.capital:
                {
                    List<string> answers = new();
                    foreach (Country c in quiz.countryAnswersP2) answers.Add(c.capital);
                    answers.Add(quiz.countryQuestion.capital);
                    LoadAnswersText(answers , player2);
                }
                break;
            case GeographyQuestion.AnswerType.flag:
                {
                    List<(Sprite, string)> answers = new();
                    foreach (Country c in quiz.countryAnswersP2) 
                    {
                        answers.Add((c.GetFlag(), c.code)); 
                    }

                    answers.Add((quiz.countryQuestion.GetFlag(), quiz.countryQuestion.code));
                    LoadAnswersSprite(answers, player2);
                }
                break;
            default:
                Debug.LogError("Non-existing geography answer type");
                break;
        }
    }

    void LoadAnswersText(List<string> answers, Player player)
    {
        answers = answers.Shuffle();

        for (int i = 0; i < answers.Count; i++)
        {
            player.Buttons[i].SetCountryName(answers[i]);
        }
    }

    private void LoadAnswersSprite(List<(Sprite img, string name)> answers, Player player)
    {
        answers = answers.Shuffle();

        for (int i = 0; i < answers.Count; i++)
        {
            player.Buttons[i].SetSpriteImage(answers[i].img, answers[i].name);
        }
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
        if (!player1.HasAnswered || !player2.HasAnswered)
        {
            return;
        }

        player1.EnableButtons(false);
        player2.EnableButtons(false);

        gameTimer.Pause();

        CheckAnswers();

        StartCoroutine(ResetInterval());
    }

    IEnumerator ResetInterval()
    {
        yield return new WaitForSeconds(newEquationInterval);
        NextQuestion();
        gameTimer.Resume();
    }

    private void CheckAnswers()
    {
        bool P1correct = false;
        bool P2correct = false;

        switch (quiz.answerTypeP1)
        {
            case GeographyQuestion.AnswerType.name:
                P1correct = player1.ChosenAnswer.Text.text == quiz.countryQuestion.name;
                break;
            case GeographyQuestion.AnswerType.capital:
                P1correct = player1.ChosenAnswer.Text.text == quiz.countryQuestion.capital;
                break;
            case GeographyQuestion.AnswerType.flag:
                P1correct = player1.ChosenAnswer.Image.gameObject.name == quiz.countryQuestion.code;
                break;
            default:
                break;
        }

        switch (quiz.answerTypeP2)
        {
            case GeographyQuestion.AnswerType.name:
                P2correct = player2.ChosenAnswer.Text.text == quiz.countryQuestion.name;
                break;
            case GeographyQuestion.AnswerType.capital:
                P2correct = player2.ChosenAnswer.Text.text == quiz.countryQuestion.capital;
                break;
            case GeographyQuestion.AnswerType.flag:
                P2correct = player2.ChosenAnswer.Image.gameObject.name == quiz.countryQuestion.code;
                break;
            default:
                break;
        }

        if (P1correct && P2correct)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    } 

    
    private void NextQuestion()
    {
        questionImage.sprite = null;
        questionText.text = "";
        foreach (AnswerButton b in player1.Buttons)
        {
            b.ResetButton();
            b.SetActive(true);
        }
        foreach (AnswerButton b in player2.Buttons)
        {
            b.ResetButton();
            b.SetActive(true);
        }

        player1.ChosenAnswer.PressReset();
        player2.ChosenAnswer.PressReset();
        player1.ChosenAnswer = null;
        player2.ChosenAnswer = null;
        player1.ResetPlayer();
        player2.ResetPlayer();
        statusImage.color = Color.white;

        difficulty += difficultyIncrease;

        LoadNewQuestion();
        LoadAnswers();
    }

    private void CorrectAnswer()
    {
        statusImage.color = Color.green;
        uiAudioSource.PlayOneShot(clipCorrect);
        gameTimer.AddTime(secondsAddOnSucces);
        score++;
        scoreText.text = score.ToString();
    }

    private void WrongAnswer()
    {
        statusImage.color = Color.red;
        uiAudioSource.PlayOneShot(clipWrong);
        gameTimer.SubtractTime(secondsSubtractOnFail);
    }

    private void EndGame()
    {
        SceneManager.LoadScene(1);
    }
}
