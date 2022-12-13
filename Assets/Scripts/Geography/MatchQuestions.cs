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
    [SerializeField] List<Sprite> amountOfFlags;
    [SerializeField] List<string> capitalName;
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

    [SerializeField] List<string> questions;
    private List<string> duplicateQuestion;
    private int score = 0;
    private int questionNumber = 0;

    //[SerializeField] GeographyQuestionGenerator questionGenerator;

    //private GeographyQuestion quiz;

    //public delegate void MatchEvents();
    //public static event MatchEvents pointScored;

    void Start()
    {
        //questions = new List<string>();
        duplicateQuestion = new List<string>();

        duplicateQuestion.Add("Which flags and capital is United Kingdom?");
        //questions.Add("Which flags and capital is Italy?");
        duplicateQuestion.Add("Which flags and capital is Sweden?");
        duplicateQuestion.Add("Which flags and capital is Netherland?");
        duplicateQuestion.Add("Which flags and capital is Spain?");

        //DuplicateQuestion = questions;

        /* for (int i = 0; i < DuplicateQuestion.Count; i++) 
        {
            string temp = DuplicateQuestion[i];
            int randomIndex = Random.Range(i, DuplicateQuestion.Count);
            DuplicateQuestion[i] = DuplicateQuestion[randomIndex];
            DuplicateQuestion[randomIndex] = temp;
        } */

        RandomizeList(duplicateQuestion, null);
        RandomizeList(capitalName, null);
        //RandomizeList(null, amountOfFlags);

        foreach(string a in capitalName)
        {
            Debug.Log(a.ToString());
        }

        foreach(string q in duplicateQuestion)
        {
            Debug.Log(q.ToString());
        }

        for(int i = 0; i < 4; i++)
        {
            player1.Buttons[i].SetSpriteImage(amountOfFlags[i]);
            player2.Buttons[i].SetCapitalName(capitalName[i]);
        }

        geographyQuestionDisplay.text = duplicateQuestion[questionNumber];
        //quiz = questionGenerator.GenerateQuestion(Region.Europe);
        //geographyQuestionDisplay.text = quiz.countryQuestion.name;
        //Debug.Log()
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

        gameTimer.Pause();

        CheckAnswers();

        //if (questions[0] == geographyQuestionDisplay.text)

        /* for(int a = 0; a < duplicateQuestion.Count; a++)
        {
            for(int b = 0; b < questions.Count; b++)
            {
                if(duplicateQuestion[a] == questions[b])
                {
                    Debug.Log(questions[b] + " questions " + b + " & " + duplicateQuestion[a] + " dup quest " + a );

                    if(player1.ChosenAnswer.Image.sprite.name == player1.Buttons[a].Image.name && player2.ChosenAnswer.Text.text == player2.Buttons.ToString())
                    {
                        CorrectAnswer();
                    }

                }
            }
        } */

        /* for(int a = 0; a < duplicateQuestion.Count; a++)
        {
            if(duplicateQuestion[a] == geographyQuestionDisplay.text)
            {
                Debug.Log(duplicateQuestion[a] + " display " + a );
                    
            }
        } */

        /* if("Which flags and capital is United Kingdom?" == geographyQuestionDisplay.text)
        {
            if (player1.ChosenAnswer.Image.sprite.name == "GB-ENG" && player2.ChosenAnswer.Text.text == "London") //correct
            //if (player1.ButtonNumber == 0 && player2.ButtonNumber == 0) //correct
            //if (player1.ButtonNumber == 1 && player2.ButtonNumber == 0)
            //if (player1.ChosenAnswer.ToString() == "Button1 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button0 (AnswerButton)")
            {
                //Debug.Log(player1.ChosenAnswer.Image.sprite.name);
                //Debug.Log(player2.ChosenAnswer.Text.text);
                Debug.Log("UK");
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        }
        //else if (questions[1] == geographyQuestionDisplay.text)
        else if("Which flags and capital is Sweden?" == geographyQuestionDisplay.text)
        {
            if (player1.ChosenAnswer.Image.sprite.name == "SE" && player2.ChosenAnswer.Text.text == "Stockholm") //correct
            //if (player1.ButtonNumber == 1 && player2.ButtonNumber == 1) //correct
            //if (player1.ButtonNumber == 0 && player2.ButtonNumber == 1)
            //if (player1.ChosenAnswer.ToString() == "Button2 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button1 (AnswerButton)")
            {
                //Debug.Log(player1.ChosenAnswer.Image.sprite.name);
                //Debug.Log(player2.ChosenAnswer.Text.text);
                Debug.Log("Sweden");
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        }
        //else if (questions[2] == geographyQuestionDisplay.text)
        else if("Which flags and capital is Netherland?" == geographyQuestionDisplay.text)
        {
            if (player1.ChosenAnswer.Image.sprite.name == "NL" && player2.ChosenAnswer.Text.text == "Amsterdam") //correct
            //if (player1.ButtonNumber == 2 && player2.ButtonNumber == 2) //correct
            //if (player1.ButtonNumber == 3 && player2.ButtonNumber == 2)
            //if (player1.ChosenAnswer.ToString() == "Button0 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button2 (AnswerButton)")
            {
                //Debug.Log(player1.ChosenAnswer.Image.sprite.name);
                //Debug.Log(player2.ChosenAnswer.Text.text);
                Debug.Log("NL");
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        }
        //else if (questions[3] == geographyQuestionDisplay.text)
        else if("Which flags and capital is Spain?" == geographyQuestionDisplay.text)
        {
            if (player1.ChosenAnswer.Image.sprite.name == "ES" && player2.ChosenAnswer.Text.text == "Madrid") //correct
            //if (player1.ButtonNumber == 3 && player2.ButtonNumber == 3) //correct
            //if (player1.ButtonNumber == 2 && player2.ButtonNumber == 3)
            //if (player1.ChosenAnswer.ToString() == "Button3 (AnswerButton)"  && player2.ChosenAnswer.ToString() == "Button3 (AnswerButton)")
            {
                //Debug.Log(player1.ChosenAnswer.Image.sprite.name);
                //Debug.Log(player2.ChosenAnswer.Text.text);
                Debug.Log("Spain");
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        } */

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
        for(int i = 0; i < questions.Count; i++)
        {
            if(geographyQuestionDisplay.text == questions[i])
            {
                int a = duplicateQuestion.IndexOf(geographyQuestionDisplay.text);
                Debug.Log(a);
                //Debug.Log(player1.Buttons[i].Image.sprite.name + " player1 " + player1.ChosenAnswer.Image.sprite.name);
                //Debug.Log(player2.Buttons[i].Text.text + " player2 " + player2.ChosenAnswer.Text.text);
                Debug.Log(capitalName[a].ToString() + " player2 " + player2.ChosenAnswer.Text.text);

                /* Debug.Log(player1.Buttons[i].Image.sprite.name == player1.ChosenAnswer.Image.sprite.name);
                Debug.Log(player2.ChosenAnswer.Text.text == player2.Buttons[i].Text.text); */

                if(player1.Buttons[i].Image.sprite.name == player1.ChosenAnswer.Image.sprite.name && player2.ChosenAnswer.Text.text == player2.Buttons[i].Text.text)
                {
                    //Debug.Log("works");
                    CorrectAnswer();
                    return;
                }
            }
        }
        WrongAnswer();
    }

    /* private void CheckQuestion()
    {
        
    } */

    private void RandomizeList(List<string> countries, List<Sprite> flags)
    {
        if(countries != null)
        {
            for (int i = 0; i < countries.Count; i++) 
            {
                string temp = countries[i];
                int randomIndex = Random.Range(i, countries.Count);
                countries[i] = countries[randomIndex];
                countries[randomIndex] = temp;
            }
        }

        if(flags != null)
        {
            for (int i = 0; i < flags.Count; i++) 
            {
                Sprite temp = flags[i];
                int randomIndex = Random.Range(i, flags.Count);
                flags[i] = flags[randomIndex];
                flags[randomIndex] = temp;
            }
        }
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
        geographyQuestionDisplay.text = duplicateQuestion[questionNumber];
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
