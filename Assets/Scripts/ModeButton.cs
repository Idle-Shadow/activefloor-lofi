using TMPro;
using UnityEngine;

public class ModeButton : MonoBehaviour
{
    public TextMeshProUGUI TextObject;
    public EquationGenerator EquationGenerator;

    void OnEnable() {
        EquationGenerator.GlobalModeChanged += ChangeText;
    }

    void OnDisable() {
        EquationGenerator.GlobalModeChanged -= ChangeText;
    }

    void ChangeText()
    {
        string newText = "Mode: ";
        switch (EquationGenerator.GlobalMode)
        {
            case OperatorMode.add:
                newText += "Add";
                break;
            case OperatorMode.multiply:
                newText += "Multiply";
                break;
            case OperatorMode.mixed:
                newText += "Mixed";
                break;
        }
        TextObject.text = newText;
    }
}
