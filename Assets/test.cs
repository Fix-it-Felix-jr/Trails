using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class test : MonoBehaviour
{
    public ShakeData MyShakeData;

    public int StartDelayMilliseconds=0;
    //public int bpm=1200;
    public float time=0.6f;
    public void Vibrate()
    {
        //Vibrator.Vibrate(20);
        CameraShakerHandler.Shake(MyShakeData);
    }

    public void Start(){
        Task.Delay(StartDelayMilliseconds);
        InvokeRepeating("Vibrate", StartDelayMilliseconds*1000, time);

    }




}
