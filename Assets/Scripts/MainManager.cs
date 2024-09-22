using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private string timeServiceUrl = "https://yandex.com/time/sync.json";

    private bool inEdit;
    private bool edited;

    private float hourTimer;
    private float secondsTimer;

    private DateTime currentTime;

    [SerializeField]
    private Clock clock;

    [SerializeField]
    private TimeText timeText;

    [SerializeField]
    private Button editButton;

    [SerializeField]
    private Button endEditButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckTimerRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (inEdit) return; 
        

        if (!edited)
        {
            hourTimer += Time.deltaTime;

            if (hourTimer >= 3600)
            {
                hourTimer = 0;
                StartCoroutine(CheckTimerRoutine());
            }
        }

        secondsTimer += Time.deltaTime;

        if (secondsTimer >= 1)
        {
            var seconds = (int)secondsTimer;
            secondsTimer -= seconds;
            currentTime = currentTime.AddSeconds(seconds);
        }

        timeText.SetText(currentTime);
    }

    public IEnumerator CheckTimerRoutine()
    {

        var request = UnityWebRequest.Get(timeServiceUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            
            TimeJson timeJson = JsonUtility.FromJson<TimeJson>(request.downloadHandler.text);
            CorrectTime(DateTimeOffset.FromUnixTimeMilliseconds(timeJson.time).DateTime);
        }
        else 
        {
            Debug.LogError("Cant get data from service");
        }

        request.Dispose();
    }

    public void CorrectTime(DateTime newTime)
    {
        if (currentTime.Hour == newTime.Hour && currentTime.Minute == newTime.Minute && currentTime.Second == newTime.Second) return;

        currentTime = newTime;

        clock.SetTime(newTime);
    }

    public void ForceSetTime(DateTime newTime)
    {
        currentTime = newTime;

        clock.SetTimeWithoutAnimation(newTime);
    }

    public void ToDefault()
    {
        if (inEdit)
            EndEdit();

        inEdit = false;


        StartCoroutine(CheckTimerRoutine());
    }

    public void StartEdit()
    {
        editButton.interactable = false;
        endEditButton.interactable = true;

        inEdit = true;

        timeText.EnableEdit();

        clock.EnableEdit();
        clock.StopAnimation();
    }


    public void EditHoursText(string text)
    {
        int hours = int.Parse(text);

        if (hours >= 12) hours -= 12 * (hours / 12);

        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, currentTime.Minute, currentTime.Second);

        timeText.SetText(currentTime);

        ForceSetTime(currentTime);
    }

    public void EditMinutesText(string text)
    {
        int minutes = int.Parse(text);

        if (minutes >= 60) minutes -= 60 * (minutes / 60);

        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, minutes, currentTime.Second);

        timeText.SetText(currentTime);

        ForceSetTime(currentTime);
    }

    public void EndEdit()
    {
        var data = clock.GetInteractData();

        Debug.Log("interactData h: " + data.hoursAngleZ + " id m: " + data.minutesAngleZ);
        int hours = Mathf.CeilToInt(data.hoursAngleZ / 360 * 12);
        int minutes = Mathf.CeilToInt(data.hoursAngleZ / 360 * 12);

        editButton.interactable = true;
        endEditButton.interactable = false;

        inEdit = false;

        clock.DisableEdit();
        timeText.DisableEdit();


        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, minutes, currentTime.Second);

        clock.ResetAnimation();
    }

}
