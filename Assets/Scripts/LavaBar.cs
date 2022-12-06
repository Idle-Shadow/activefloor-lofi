using UnityEngine;
using UnityEngine.UI;

public class LavaBar : MonoBehaviour
{
    public RectTransform Fill;
    public Timer GameTimer;

    float _maxTime;

    void Start()
    {
        _maxTime = GameTimer.MaxTime;
    }

    void Update()
    {
        Fill.anchorMax = new Vector2(GameTimer.TimeRemaining / _maxTime, 1);
    }
}
