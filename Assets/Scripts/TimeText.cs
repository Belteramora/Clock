using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField hoursInputField;

    [SerializeField]
    private TMP_Text hoursText;

    [SerializeField]
    private Image hoursImage;

    [SerializeField]
    private TMP_InputField minutesInputField;

    [SerializeField]
    private TMP_Text minutesText;

    [SerializeField]
    private Image minutesImage;

    [SerializeField]
    private TMP_Text secondsText;

    


    public void SetText(string hours, string minutes, string seconds)
    {
        hoursInputField.text = hours;
        minutesInputField.text = minutes;
        secondsText.text = seconds;
    }

    public void SetText(DateTime dateTime)
    {
        hoursInputField.text = dateTime.ToString("hh");
        minutesInputField.text = dateTime.ToString("mm");
        secondsText.text = dateTime.ToString("ss");
    }

    public void EnableEdit()
    {
        hoursInputField.interactable = true;
        hoursText.color = Color.black;
        hoursImage.enabled = true;

        minutesInputField.interactable = true;
        minutesText.color = Color.black;
        minutesImage.enabled = true;
    }

    public void DisableEdit()
    {
        hoursInputField.interactable = false;
        hoursText.color = Color.white;
        hoursImage.enabled = false;

        minutesInputField.interactable = false;
        minutesText.color = Color.white;
        minutesImage.enabled = false;
    }
}
