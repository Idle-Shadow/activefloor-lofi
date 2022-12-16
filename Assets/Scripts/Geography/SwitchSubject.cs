using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchSubject : MonoBehaviour
{
    [SerializeField] GameObject math;
    [SerializeField] GameObject geography;
    [SerializeField] TMP_Text btnText;

    public void SwitchSubjectType()
    {
        if(math.activeSelf)
        {
            geography.SetActive(true);
            math.SetActive(false);
            btnText.text = "Geography";
        }
        else
        {
            geography.SetActive(false);
            math.SetActive(true);
            btnText.text = "Math";
        }
    }
}
