using UnityEngine;
using UnityEngine.UI;

public class LavaBar : MonoBehaviour
{
    public GameObject Fill;
    public Timer GameTimer;

    RectTransform _rectTransform;
    Image _image;
    Color _defaultColor;
    float _maxTime;

    void Start()
    {
        _maxTime = GameTimer.MaxTime;
        _rectTransform = Fill.GetComponent<RectTransform>();
        _image = Fill.GetComponent<Image>();
        _defaultColor = _image.color;
    }

    void Update()
    {
        _rectTransform.anchorMax = new Vector2(GameTimer.TimeRemaining / _maxTime, 1);
        if (GameTimer.TimeRemaining < 10f) _image.color = Color.red;
        else _image.color = _defaultColor;
    }
}
