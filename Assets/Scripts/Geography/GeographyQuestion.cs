using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class GeographyQuestion
{
    public Country countryQuestion;
    public QuestionType questionType;
    public List<Country> countryAnswersP1;
    public AnswerType answerTypeP1;
    public List<Country> countryAnswersP2;
    public AnswerType answerTypeP2;

    //The amount of answers including the correct one
    private static readonly int answerAmount = 4;

    public GeographyQuestion(List<Country> countries)
    {
        countryQuestion = countries.GetRandomElement();
        questionType = questionType.GetRandomEnum();
        countryAnswersP1 = countries.GetRandomElementsExcluding(new List<Country>() { countryQuestion }, answerAmount - 1);
        answerTypeP1 = answerTypeP1.GetRandomEnumExcluding(new List<string>() { questionType.ToString() });
        countryAnswersP2 = countries.GetRandomElementsExcluding(new List<Country>() { countryQuestion }, answerAmount - 1);
        answerTypeP2 = answerTypeP2.GetRandomEnumExcluding(new List<string>() { questionType.ToString(), answerTypeP1.ToString() });
    }

    public enum QuestionType
    {
        name,
        capital,
        flag,
        map
    }

    public enum AnswerType
    {
        name,
        capital,
        flag
    }
}


