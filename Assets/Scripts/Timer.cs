using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeLeft;
    private bool timerOn = false;
    [SerializeField] private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            updateTimer(timeLeft);
        }
        else
        {
            timeLeft = 0;
            timerOn = false;
        }
    }

    private void updateTimer(float currentTime)
    {
        currentTime += 1;
        int currentMins = Mathf.FloorToInt(currentTime / 60);
        int currentSecs = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", currentMins, currentSecs);
    }
}