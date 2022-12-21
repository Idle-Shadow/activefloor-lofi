using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquationModeSetter : MonoBehaviour
{
    [SerializeField] EquationGeneratorPreset preset;

    public void Play()
    {
        transform.SetParent(null, false);
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Math Game");
        StartCoroutine(SetModeAfterSceneLoad());
    }

    IEnumerator SetModeAfterSceneLoad()
    {
        yield return new WaitWhile(() => FindObjectOfType<EquationGenerator>() == null);

        FindObjectOfType<EquationGenerator>().SetNewGeneratorPreset(preset);
        Destroy(gameObject);
    }
}
