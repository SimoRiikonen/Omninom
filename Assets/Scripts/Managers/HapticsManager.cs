using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsManager : Singleton<HapticsManager>
{
    public void Vibrate(long milliseconds = 50, int amplitude = 255)
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaObject vibrator = context.Call<AndroidJavaObject>("getSystemService", "vibrator");

        if (vibrator.Call<bool>("hasVibrator"))
        {
            AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
            AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
            vibrator.Call("vibrate", effect);
        }
    #endif
    }
}