using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    private Sequence hoursRotate;
    private Sequence minutesRotate;
    private Sequence secondsRotate;

    private InteractableArrow hoursInteractable;
    private InteractableArrow minutesInteractable;

    public RectTransform hoursArrow;
    public RectTransform minutesArrow;
    public RectTransform secondsArrow;


    private void Start()
    {
        hoursInteractable = hoursArrow.GetComponent<InteractableArrow>();
        minutesInteractable = minutesArrow.GetComponent<InteractableArrow>();
    }

    public void SetTime(DateTime dateTime)
    {
        StopAnimation();

        SetTimeWithoutAnimation(dateTime);

        ResetAnimation();
    }

    public void SetTimeWithoutAnimation(DateTime dateTime)
    {
        hoursArrow.rotation = Quaternion.Euler(Vector3.back * (dateTime.Hour % 12 / 12f) * 360f);
        minutesArrow.rotation = Quaternion.Euler(Vector3.back * (dateTime.Minute / 60f) * 360f);
        secondsArrow.rotation = Quaternion.Euler(Vector3.back * (dateTime.Second / 60f) * 360f);
    }

    public void ResetAnimation()
    {
        hoursRotate = DOTween.Sequence();
        hoursRotate.Append(hoursArrow.DOLocalRotate(Vector3.back * 360, 43200, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear));
        hoursRotate.SetLoops(-1, LoopType.Incremental);

        minutesRotate = DOTween.Sequence();
        minutesRotate.Append(minutesArrow.DOLocalRotate(Vector3.back * 360, 3600, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear));
        minutesRotate.SetLoops(-1, LoopType.Incremental);

        secondsRotate = DOTween.Sequence();
        secondsRotate.Append(secondsArrow.DOLocalRotate(Vector3.back * 360, 60, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear));
        secondsRotate.SetLoops(-1, LoopType.Incremental);
    }

    public void StopAnimation()
    {
        hoursRotate?.Kill();
        minutesRotate?.Kill();
        secondsRotate?.Kill();
    }
    public void ChangeMinutesRotation(float deltaRotation)
    {
        hoursArrow.localEulerAngles = new Vector3(0, 0, hoursArrow.localEulerAngles.z + deltaRotation / 12);
        secondsArrow.localEulerAngles = new Vector3(0, 0, secondsArrow.localEulerAngles.z + deltaRotation * 60);
    }

    public void ChangeHoursRotation(float deltaRotation)
    {
        minutesArrow.localEulerAngles = new Vector3(0, 0, minutesArrow.localEulerAngles.z + deltaRotation * 12);
        secondsArrow.localEulerAngles = new Vector3(0, 0, secondsArrow.localEulerAngles.z + deltaRotation * 12 * 60);
    }

    public void EnableEdit()
    {
        hoursInteractable.canDrag = true;
        minutesInteractable.canDrag = true;
    }

    public void DisableEdit()
    {
        hoursInteractable.canDrag = false;
        minutesInteractable.canDrag = false;
    }

    public InteractData GetInteractData()
    {
        return new InteractData(hoursArrow.localEulerAngles.z, minutesArrow.localEulerAngles.z);
    }
}

public struct InteractData
{
    public float hoursAngleZ;
    public float minutesAngleZ;

    public InteractData(float hoursAngleZ, float minutesAngleZ)
    {
        this.hoursAngleZ = hoursAngleZ;
        this.minutesAngleZ = minutesAngleZ;
    }
}
