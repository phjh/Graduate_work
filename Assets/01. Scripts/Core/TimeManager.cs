using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : ManagerBase<TimeManager>
{
	private Coroutine TimeScaleChange;

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
}
