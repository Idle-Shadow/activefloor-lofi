using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeographyQuestions : MonoBehaviour
{
    [SerializeField] TMP_Text geographyQuestion;
    private List<string> questions;
    // Start is called before the first frame update
    void Start()
    {
        questions = new List<string>();
        questions.Add("Which flags and capital is United Kingdom?");
        questions.Add("Which flags and capital is Italy?");
        questions.Add("Which flags and capital is Sweden?");
        questions.Add("Which flags and capital is Netherland?");
        questions.Add("Which flags and capital is Spain?");
        int i = Random.Range(0,questions.Count);
        geographyQuestion.text = questions[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
