using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float BAR_MOVE_SPEED = 0.2f;
    public static float TIME_BETWEEN_RED_ORANGE_BARS = 0.5f;

    public MyObject health_bar_object;
    public MyObject red_bar_object;
    public MyObject orange_bar_object;

    private Coroutine curr_routine;

    void Awake()
    {
        // set bar objects to be full
        red_bar_object.transform.localScale = Vector3.one;
        orange_bar_object.transform.localScale = Vector3.one;

        // hide bar
        transform.localScale = Vector3.zero;
    }

    public void ShowBar()
    {
        health_bar_object.SquishyChangeScale(1.1f, 1f, 0.1f, 0.1f);
    }

    public void UpdateBar(float percent)
    {
        // make sure percent is between 0 and 1
        if (percent < 0f || percent > 1f)
            return;

        // stop current routine iff one is active
        if (curr_routine != null)
        {
            StopCoroutine(curr_routine);
        }
        // start routine
        curr_routine = StartCoroutine(UpdateBarPercent(percent));
    }

    private IEnumerator UpdateBarPercent(float percent)
    {
        red_bar_object.ChangeScale(new Vector2(1f, percent), BAR_MOVE_SPEED);
        yield return new WaitForSeconds(TIME_BETWEEN_RED_ORANGE_BARS);
        orange_bar_object.ChangeScale(new Vector2(1f, percent), BAR_MOVE_SPEED);
    }
}   
