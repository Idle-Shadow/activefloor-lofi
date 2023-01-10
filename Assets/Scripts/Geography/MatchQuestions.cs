using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExtensionMethods;
using System.Linq;

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
    [SerializeField] Color CorrectAnswerColor;
    [SerializeField] Color IncorrectAnswerColor;

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
        AnswerButton.ButtonPressed += MatchAnswer;
        Timer.TimerExpired += EndGame;
    }

    void OnDisable()
    {
        AnswerButton.ButtonPressed -= MatchAnswer;
        Timer.TimerExpired -= EndGame;
    }

    private void MatchAnswer()
    {
        if (player1.ChosenAnswer == null || player2.ChosenAnswer == null)
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

    public bool VerifyPlayer1Answer() => quiz.answerTypeP1 switch
    {
        GeographyQuestion.AnswerType.name => player1.ChosenAnswer.Text.text == quiz.countryQuestion.name,
        GeographyQuestion.AnswerType.capital => player1.ChosenAnswer.Text.text == quiz.countryQuestion.capital,
        GeographyQuestion.AnswerType.flag => player1.ChosenAnswer.Image.gameObject.name == quiz.countryQuestion.code,
        _ => false
    };

    public bool VerifyPlayer2Answer() => quiz.answerTypeP2 switch
    {
        GeographyQuestion.AnswerType.name => player2.ChosenAnswer.Text.text == quiz.countryQuestion.name,
        GeographyQuestion.AnswerType.capital => player2.ChosenAnswer.Text.text == quiz.countryQuestion.capital,
        GeographyQuestion.AnswerType.flag => player2.ChosenAnswer.Image.gameObject.name == quiz.countryQuestion.code,
        _ => false
    };

    private void CheckAnswers()
    {
        bool P1correct = VerifyPlayer1Answer();
        bool P2correct = VerifyPlayer2Answer();

        HighlightCorrectAnswers();
        if (P1correct && P2correct)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    public void HighlightCorrectAnswers()
    {
        if (!VerifyPlayer1Answer())
            player1.ChosenAnswer.button.image.color = IncorrectAnswerColor;
        Getplayer1AnswerButton().Image.color = CorrectAnswerColor;

        if (!VerifyPlayer2Answer())
            player2.ChosenAnswer.button.image.color = IncorrectAnswerColor;
        Getplayer2AnswerButton().Image.color = CorrectAnswerColor;
    }

    public AnswerButton Getplayer1AnswerButton() => quiz.answerTypeP1 switch
    {
        GeographyQuestion.AnswerType.name => player1.Buttons.First(x => x.Text.text == quiz.countryQuestion.name),
        GeographyQuestion.AnswerType.capital => player1.Buttons.First(x => x.Text.text == quiz.countryQuestion.capital),
        GeographyQuestion.AnswerType.flag => player1.Buttons.First(x => x.Image.gameObject.name == quiz.countryQuestion.code),
        _ => null
    };

    public AnswerButton Getplayer2AnswerButton() => quiz.answerTypeP2 switch
    {
        GeographyQuestion.AnswerType.name => player2.Buttons.First(x => x.Text.text == quiz.countryQuestion.name),
        GeographyQuestion.AnswerType.capital => player2.Buttons.First(x => x.Text.text == quiz.countryQuestion.capital),
        GeographyQuestion.AnswerType.flag => player2.Buttons.First(x => x.Image.gameObject.name == quiz.countryQuestion.code),
        _ => null
    };


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

        player1.ResetPlayer();
        player2.ResetPlayer();
        statusImage.color = Color.white;

        difficulty += difficultyIncrease;

        LoadNewQuestion();
        LoadAnswers();
    }

    private void CorrectAnswer()
    {
        //statusImage.color = Color.green;
        uiAudioSource.PlayOneShot(clipCorrect);
        gameTimer.AddTime(secondsAddOnSucces);
        score++;
        scoreText.text = score.ToString();
    }

    private void WrongAnswer()
    {
        //statusImage.color = Color.red;
        uiAudioSource.PlayOneShot(clipWrong);
        gameTimer.SubtractTime(secondsSubtractOnFail);
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
