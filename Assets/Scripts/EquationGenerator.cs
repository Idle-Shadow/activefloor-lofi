using System.Collections.Generic;
using System;
using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    [Serializable]
    public class ModeWithToggle
    {
        public OperatorModeItem OperatorMode;
        public bool ModeEnabled = false;
    }
    public List<ModeWithToggle> TargetModes;
    public OperatorModeItem CurrentMode { get; private set; }
    public bool IncreaseDifficultyWhenScored = false;
    public float DifficultyIncrease = .1f;

    public delegate void EquationGeneratorEvents();
    public static event EquationGeneratorEvents TargetModesChanged;

    int _difficulty = 0;

    void OnEnable()
    {
        MatchDirector.PointScored += IncreaseDifficulty;
    }

    void OnDisable()
    {
        MatchDirector.PointScored -= IncreaseDifficulty;
    }

    public (int, int, int) GenerateEquation()
    {
        (int, int, int) equation = (0, 0, 0);

        bool ModeSelected = false;
        while (!ModeSelected)
        {
            ModeWithToggle mode = TargetModes[UnityEngine.Random.Range(0, TargetModes.Count)];
            if (mode.ModeEnabled)
            {
                ModeSelected = true;
                CurrentMode = mode.OperatorMode;
            }
        }

        (int, int) randomNumbers = GenerateRandomNumbers();
        equation.Item1 = randomNumbers.Item1;
        equation.Item2 = randomNumbers.Item2;

        switch (CurrentMode.Mode)
        {
            case OperatorMode.add:
                equation.Item3 = equation.Item1 + equation.Item2;
                break;
            case OperatorMode.multiply:
                equation.Item3 = equation.Item1 * equation.Item2;
                break;
            case OperatorMode.subtract:
                equation.Item3 = equation.Item1 - equation.Item2;
                break;
            case OperatorMode.divide:
                equation.Item3 = equation.Item1 / equation.Item2;
                break;
        }

        return equation;
    }

    public void ToggleTargetMode(OperatorModeItem mode, bool modeEnabled)
    {
        TargetModes.Find(x => x.OperatorMode == mode).ModeEnabled = modeEnabled;
        if (TargetModesChanged != null) TargetModesChanged.Invoke();
    }

    void IncreaseDifficulty()
    {
        if (IncreaseDifficultyWhenScored)
        {
            _difficulty++;
        }
    }

    (int, int) GenerateRandomNumbers()
    {
        int difficultyAddition = Mathf.FloorToInt(_difficulty * DifficultyIncrease * CurrentMode.HighestNumber);
        int floor = CurrentMode.LowestNumber + difficultyAddition;
        int ceil = CurrentMode.HighestNumber + difficultyAddition;
        int number1;
        int number2;

        do {
            number1 = UnityEngine.Random.Range(floor, ceil);
            number2 = UnityEngine.Random.Range(floor, ceil);
        } while (number1 == number2 || (CurrentMode.Mode == OperatorMode.divide && number1 % number2 != 0));

        switch (CurrentMode.Mode)
        {
            case OperatorMode.subtract:
                if (number1 > number2) return (number1, number2);
                else return (number2, number1);
            default:
                return (number1, number2);
        }
    }
}
