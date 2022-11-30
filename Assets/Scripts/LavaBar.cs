using UnityEngine;
using UnityEngine.UI;

public class LavaBar : MonoBehaviour
{
    public Image LavaObject;
    public Timer GameTimer;

    float _maxTime;

    void Start()
    {
        _maxTime = GameTimer.MaxTime;
    }

    void Update()
    {
        float fill = 1 - GameTimer.TimeRemaining / _maxTime;
        LavaObject.fillAmount = fill;
    }
}
