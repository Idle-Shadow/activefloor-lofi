using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquationGeneratorPreset", order = 2)]
public class EquationGeneratorPreset : ScriptableObject
{
    [Serializable]
    public class ModeSettings
    {
        public OperatorModeItem OperatorMode;
        public int LowestNumber;
        public int HighestNumber;
    }

    public List<ModeSettings> TargetModes;
    public bool IncreaseDifficultyWhenScored = false;

    [Tooltip("Increase in % (0.1 is 10%) for every right answer.")]
    public float DifficultyIncrease = .1f;
}
