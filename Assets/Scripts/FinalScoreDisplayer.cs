using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScoreDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextMesh;

    public void SetScore(int Score)
    {
        if (TextMesh == null)
            return;
        TextMesh.text = $"Final score: { Score }!";
    }
}
