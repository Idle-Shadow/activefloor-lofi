using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LavaHurtbox : MonoBehaviour
{
    public float FadeOutDuration = 1f;

    Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void Flash()
    {
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color color = _image.color;
        for (float alpha = 1f; alpha >= 0; alpha -= .05f)
        {
            color.a = alpha;
            _image.color = color;
            yield return new WaitForSeconds(.05f * FadeOutDuration);
        }
    }
}
