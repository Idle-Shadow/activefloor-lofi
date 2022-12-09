using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtensionMethods;

public class GeographyQuestionGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    private List<Country> countries;

    private void Start()
    {
        LoadJson();

        for (int i = 0; i < 100; i++)
        {
            GeographyQuestion q = GenerateQuestion(Region.Any, i);
        }
    }

    private void LoadJson()
    {
        countries = JsonConvert.DeserializeObject<List<Country>>(jsonFile.text);
    }

    public GeographyQuestion GenerateQuestion(Region region, int maxDifficulty = 100)
    {
        if (region == Region.Any)
        {
            return new GeographyQuestion(countries, maxDifficulty);
        }
        else
        {
            return new GeographyQuestion(countries.Where(x => x.region == region.ToString()).ToList(), maxDifficulty);
        }
    }
}

public enum Region
{
    Europe,
    Africa,
    Asia,
    Oceania,
    Americas,
    Any
}
