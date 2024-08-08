using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : ManagerBase<TimeManager>
{
	private Coroutine TimeScaleChange;

    [Tooltip("초 단위로 입력")]
    public float timeLimit; 

    public bool isTimerActived = false;

    public TextMeshProUGUI TimerText;

	public override void InitManager()
	{
		base.InitManager();
		ResetTimeScale();
		ResetTimeChangeCoroutine();
	}

    public void TimeChange(float targetScale, float tick)
    {
        ResetTimeChangeCoroutine();

        if (Time.timeScale < targetScale)
        {
            TimeScaleChange = StartCoroutine(TimeFast(targetScale, tick));
        }
        else
            TimeScaleChange = StartCoroutine(TimeSlow(targetScale, tick));
    }

    public void ResetTimeScale() { Time.timeScale = 1f; }

    public void StopTimeScale() { Time.timeScale = 0f; }

    private void ResetTimeChangeCoroutine()
    {
        float prevTimeScale = Time.timeScale;

        if (TimeScaleChange != null)
        {
            StopCoroutine(TimeScaleChange);
            TimeScaleChange = null;

            Time.timeScale = prevTimeScale;
        }
    }

    private IEnumerator TimeSlow(float targetScale, float tick)
    {

        float tickScale = (Time.timeScale - targetScale) / tick;

        while (Time.timeScale > targetScale)
        {
            if (Time.timeScale - tickScale <= 0f)
            {
                Time.timeScale = 0f;
                break;
            }
            Time.timeScale -= tickScale;

            yield return new WaitForSecondsRealtime(1 / tick);
        }
    }

    private IEnumerator TimeFast(float targetScale, float tick)
    {

        float tickScale = (targetScale - Time.timeScale) / tick;

        while (Time.timeScale < targetScale)
        {
            Time.timeScale += tickScale;
            yield return new WaitForSecondsRealtime(1 / tick);
        }

        if (Time.timeScale > targetScale)
            Time.timeScale = targetScale;
    }

    public void StartTimer()
    {
        isTimerActived = true;
        StartCoroutine(Timer());
        TimerText.gameObject.SetActive(true);
    }

    public void SetTimer(bool value)
    {
        isTimerActived = value;
        TimerText.gameObject.SetActive(value);
    }

    private IEnumerator Timer()
    {
        while(timeLimit >= 0)
        {
            if (!isTimerActived)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            SetTimerUI();
            timeLimit -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void SetTimerUI()
    {
        int t0 = (int)timeLimit; 
        int m = t0 / 60; 
        int s = (t0 - m * 60); 
        int ms = (int)((timeLimit - t0) * 100); 
        TimerText.text = $"[ {m:00} : {s:00} : {ms:00} ]";
    }

}
