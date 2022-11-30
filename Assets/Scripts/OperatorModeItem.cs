using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OperatorModeItem", order = 1)]
public class OperatorModeItem : ScriptableObject
{
    public OperatorMode Mode;
    public string StringRepresentation;
    public int LowestNumber;
    public int HighestNumber;
}
