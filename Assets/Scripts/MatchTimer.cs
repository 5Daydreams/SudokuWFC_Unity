using System;
using SimpleValues;
using UnityEngine;


public class MatchTimer : MonoBehaviour
{
    [SerializeField] private FloatValue PlayerTime;
    [SerializeField] private bool startOnAwake = false;
    private bool timerActive = false;

    private void Awake()
    {
        timerActive = startOnAwake;
        ResetTimer();
    }

    private void Update()
    {
        if (!timerActive)
        {
            return;
        }
        
        PlayerTime.Value += Time.deltaTime;
    }

    public void PauseTimer()
    {
        timerActive = false;
    }

    public void ResumeTimer()
    {
        timerActive = true;
    }

    public void ResetTimer()
    {
        PlayerTime.Value = 0;
    }
}