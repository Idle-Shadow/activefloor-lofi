using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionPicker : MonoBehaviour
{
    public static Region chosenRegion;

    public Region region;
    public void OnPickRegion()
    {
        chosenRegion = region;
        SceneManager.LoadScene("GeographyConcept");
    }
}
