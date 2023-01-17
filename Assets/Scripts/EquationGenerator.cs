using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    public EquationGeneratorPreset GeneratorPreset;
    public EquationGeneratorPreset.ModeSettings CurrentModeSettings { get; private set; }

    int _difficulty = 0;

    void OnEnable()
    {
        MatchDirector.PointScored += IncreaseDifficulty;
    }

    void OnDisable()
    {
        MatchDirector.PointScored -= IncreaseDifficulty;
    }

    public void SetNewGeneratorPreset (EquationGeneratorPreset preset)
    {
        GeneratorPreset = preset;
    }

    public (int, int, int) GenerateEquation()
    {
        (int, int, int) equation = (0, 0, 0);

        CurrentModeSettings = GeneratorPreset.TargetModes[Random.Range(0, GeneratorPreset.TargetModes.Count)];

        (int, int) randomNumbers = GenerateRandomNumbers();
        equation.Item1 = randomNumbers.Item1;
        equation.Item2 = randomNumbers.Item2;

        switch (CurrentModeSettings.OperatorMode.Mode)
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

    void IncreaseDifficulty()
    {
        if (GeneratorPreset.IncreaseDifficultyWhenScored)
        {
            _difficulty++;
        }
    }

    (int, int) GenerateRandomNumbers()
    {
        int difficultyAddition = Mathf.FloorToInt(_difficulty * GeneratorPreset.DifficultyIncrease * CurrentModeSettings.HighestNumber);
        int floor = CurrentModeSettings.LowestNumber + difficultyAddition;
        int ceil = CurrentModeSettings.HighestNumber + difficultyAddition;
        int number1;
        int number2;

        do {
            number1 = Random.Range(floor, ceil);
            number2 = Random.Range(floor, ceil);
        } while (number1 == number2 || (CurrentModeSettings.OperatorMode.Mode == OperatorMode.divide && number1 % number2 != 0));

        switch (CurrentModeSettings.OperatorMode.Mode)
        {
            case OperatorMode.subtract:
                if (number1 > number2) return (number1, number2);
                else return (number2, number1);
            default:
                return (number1, number2);
        }
    }
}
