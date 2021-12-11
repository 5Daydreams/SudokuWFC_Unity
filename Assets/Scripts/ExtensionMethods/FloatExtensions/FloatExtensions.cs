using UnityEngine;

namespace _Code.Toolbox.Extensions
{
    public static class FloatExtensions
    {
        public static string ConvertSecondsToTimer(this float self)
        {
            string timerDisplay = "";

            int hours = (int) (self / 3600.0f);
            if (hours > 0)
            {
                self = self % 3600.0f;
                timerDisplay += hours.ToString("00") + ":";
            }

            int minutes = (int) (self / 60.0f);
            if (minutes > 0)
            {
                self = minutes % 60.0f;
            }

            int seconds = 0;
            seconds = Mathf.FloorToInt(self);

            int miliseconds = 0;
            miliseconds = Mathf.FloorToInt((self - seconds) * 1000);
            var miliString = miliseconds.ToString("000");

            timerDisplay += minutes.ToString("00") + ":" 
                            + seconds.ToString("00") + "." 
                            + miliString;

            return timerDisplay;
        }

        public static string FreyaConvertToTimer(this float self)
        {
            float fTotalMinutes = self/60f;
            float fTotalHours = fTotalMinutes/60f;
            int h = (int)fTotalHours;
            int m = (int)(fTotalMinutes-h*60);
            int s = (int)(self-h*60*60-m*60);
            int ms = (int)((self-h*60*60-m*60-s)*1000);
            
            return $"{h:00}:{m:00}:{s:00}.{ms:000}";
        }
    }
}