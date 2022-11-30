using UnityEngine;
using UnityEngine.UI;

public class ModeToggle : MonoBehaviour
{
    public OperatorModeItem Mode;
    Toggle _toggle;
    EquationGenerator _generator;
    EquationGenerator.ModeWithToggle _modeToggle;

    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _generator = FindObjectOfType<EquationGenerator>();
        _modeToggle = _generator.TargetModes.Find(x => x.OperatorMode == Mode);
        _toggle.isOn = _modeToggle.ModeEnabled;
        _toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(_toggle);
        });
    }

    void ToggleValueChanged(Toggle change)
    {
        _generator.ToggleTargetMode(Mode, _toggle.isOn);
    }
}
