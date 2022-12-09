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
    }

    private void LoadJson()
    {
        countries = JsonConvert.DeserializeObject<List<Country>>(jsonFile.text);
    }

    public GeographyQuestion GenerateQuestion(Region region)
    {
        if (region == Region.Any)
        {
            return new GeographyQuestion(countries);
        }
        else
        {
            return new GeographyQuestion(countries.Where(x => x.region == region.ToString()).ToList());
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
