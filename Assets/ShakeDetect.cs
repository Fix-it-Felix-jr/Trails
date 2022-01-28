using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using Hashtable = ExitGames.Client.Photon.Hashtable;
namespace TMPro.Examples{

public class ShakeDetect : MonoBehaviour
{
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the
    // filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    // This next parameter is initialized to 2.0 per Apple's recommendation,
    // or at least according to Brady! ;)
    float shakeDetectionThreshold = 2.0f;

    float lowPassFilterFactor;
    Vector3 lowPassValue;
    float penalty;
    float timeLeft;
    public Text txtPlayerSpeed;
    Hashtable data = new Hashtable();
    float[] penaltyValues = new float[2];
    bool end=false;
    bool started=false;
    bool first=true;
    public TMP_Text countdown;
    public int timeBeforeStarts=7000;

    public AudioSource audioSource;

    async void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        timeLeft=0.5f;
        penalty=0f;

        await Task.Delay(timeBeforeStarts);
        
        //3
        Vibrator.Vibrate(20);
        countdown.text="3";

        await Task.Delay(500);

        //2
        Vibrator.Vibrate(20);
        countdown.text="2";

        await Task.Delay(500);

        //1
        Vibrator.Vibrate(20);
        countdown.text="1";

        await Task.Delay(500);

        //GO
        Vibrator.Vibrate(20);
        countdown.text="GO!";
        started=true;
        
        await Task.Delay(500);
        countdown.text="";
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        timeLeft -= Time.deltaTime;

        data["pen"]=penalty;
        PhotonNetwork.LocalPlayer.SetCustomProperties(data);

        //Show custom properties of all players
        int j=0;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (!end && item.CustomProperties.ContainsKey("pen"))
            {
                Debug.Log("Player " +j + " penalty: " + item.CustomProperties["pen"]);
                penaltyValues[j]=(float)item.CustomProperties["pen"];
            }
            j++;
        }

        
        if (started && first) {
            timeLeft=0.5f;
            first=false;
        }

        

        if (started && !end && timeLeft<0.1 && deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            // Perform your "shaking actions" here. If necessary, add suitable
            // guards in the if check above to avoid redundant handling during
            // the same shake (e.g. a minimum refractory period).
            Debug.Log("Shake event detected at time "+Time.time);
            Vibrator.Vibrate(20);
            penalty+=Math.Abs(timeLeft);

            
            //Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
            //score[PunPlayerScores.PlayerScoreProp] = Math.Round(penalty, 2);

            //PhotonNetwork.player.SetScore(Math.Round(penalty, 2));

            timeLeft=0.5f;
        }
        if (!end && timeLeft<-10){
            penalty+=Math.Abs(timeLeft);
        }
        txtPlayerSpeed.text = "Penalty: " + Math.Round(penalty, 2).ToString();

        if (!end && !audioSource.isPlaying)
        {
            //Finished
            end=true;
            Winner();
        }
    }
    public float Penalty(){
        return penalty;
    }

    public void Winner(){
        if (penaltyValues[0]<penaltyValues[1]){
            Debug.Log("Player 0 WON!");
            if (PhotonNetwork.PlayerList[0]==PhotonNetwork.LocalPlayer){
                Debug.Log("YOU WON!");
            }
            //Do something to show that
        }
        if (penaltyValues[0]>penaltyValues[1]){
            Debug.Log("Player 1 WON!");
            if (PhotonNetwork.PlayerList[0]==PhotonNetwork.LocalPlayer){
                Debug.Log("YOU WON!");
            }
            //Do something to show that
        }
        if(penaltyValues[0]==penaltyValues[1]){
            Debug.Log("Both are winners! Same score!");
            //Do something to show that
        }
        
    }
}
}