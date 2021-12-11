using System;
using _Code.Toolbox.Extensions;
using Trackables;
using UnityEngine;
using UnityEngine.UI;


public class MatchTimer : MonoBehaviour
{
    [SerializeField] private TrackableFloat _playerTime;
    [SerializeField] private Text _textBox;
    [SerializeField] private bool startOnAwake = false;
    private bool timerActive = false;

    private void Awake()
    {
        timerActive = startOnAwake;
        ResetTimer();
        _playerTime.CallbackOnValueChanged.AddListener(AdjustText);
    }

    private void AdjustText(float time)
    {
        string timerString = time.FreyaConvertToTimer();
        
        _textBox.text = timerString;
    }

    private void Update()
    {
        if (!timerActive)
        {
            return;
        }
        
        _playerTime.Value += Time.deltaTime;
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
        _playerTime.Value = 0;
    }
}