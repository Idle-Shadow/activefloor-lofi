using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator
{
    public List<(int, int, int)> GenerateEquations(int amount, int highestNumber, OperatorType type)
    {
        List<(int, int, int)> list = new();

        for (int i = 0; i < amount; i++)
        {
            list.Add(GenerateEquation(highestNumber, type));
        }

        return list;
    }

    (int, int, int) GenerateEquation(int highestNumber, OperatorType type)
    {
        (int, int, int) equation = (0, 0, 0);
        equation.Item1 = Random.Range(1, highestNumber);
        equation.Item2 = Random.Range(1, highestNumber);

        switch (type)
        {
            case OperatorType.add:
                equation.Item3 = equation.Item1 + equation.Item2;
                break;
            case OperatorType.multiply:
                equation.Item3 = equation.Item1 * equation.Item2;
                break;
        }

        return equation;
    }
}
