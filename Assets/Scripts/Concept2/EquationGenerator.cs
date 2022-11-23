using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    public int HighestNumberAddition = 100;
    public int HighestNumberMultiplication = 20;
    public OperatorMode GlobalMode { get; private set; } = OperatorMode.add;
    public OperatorMode CurrentMode { get; private set; }
    public int CurrentHighestNumber { get; private set; }

    public delegate void EquationGeneratorEvents();
    public static event EquationGeneratorEvents GlobalModeChanged;

    public (int, int, int, string) GenerateEquation()
    {
        (int, int, int, string) equation = (0, 0, 0, "");

        switch (GlobalMode)
        {
            case OperatorMode.add:
                CurrentMode = OperatorMode.add;
                CurrentHighestNumber = HighestNumberAddition;
                break;
            case OperatorMode.multiply:
                CurrentMode = OperatorMode.multiply;
                CurrentHighestNumber = HighestNumberMultiplication;
                break;
            case OperatorMode.mixed:
                CurrentMode = (OperatorMode)Random.Range(0, 2);
                CurrentHighestNumber = CurrentMode == OperatorMode.add ? HighestNumberAddition : HighestNumberMultiplication;
                break;
        }

        equation.Item1 = Random.Range(1, CurrentHighestNumber);
        equation.Item2 = Random.Range(1, CurrentHighestNumber);

        switch (CurrentMode)
        {
            case OperatorMode.add:
                equation.Item3 = equation.Item1 + equation.Item2;
                equation.Item4 = "+";
                break;
            case OperatorMode.multiply:
                equation.Item3 = equation.Item1 * equation.Item2;
                equation.Item4 = "x";
                break;
        }

        return equation;
    }

    public void NextGlobalMode()
    {
        int newModeIndex = (int)GlobalMode + 1;
        if (newModeIndex > 2) newModeIndex = 0;
        GlobalMode = (OperatorMode)newModeIndex;
        GlobalModeChanged.Invoke();
    }

    public void SetGlobalMode(int modeIndex)
    {
        GlobalMode = (OperatorMode)modeIndex;
        GlobalModeChanged.Invoke();
    }
}
