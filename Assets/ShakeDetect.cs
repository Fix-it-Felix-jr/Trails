using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using CameraFading;
using UnityEngine.Audio;
namespace TMPro.Examples{

public class ShakeDetect : MonoBehaviour
{
    public AudioMixer filterMixer;
    public AudioMixerSnapshot[] filterSnapshots;
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
    public TMP_Text txtPlayerSpeed;
    Hashtable data = new Hashtable();
    bool[] endValues = new bool[2];
    float[] penaltyValues = new float[2];
    bool end=false;
    bool started=false;
    bool first=true;
    public TMP_Text countdown;
    public int timeBeforeStarts=7000;
    bool disconnect=false;
    public float[] pesi=new float[2];
    //bool canShake=true;
    //GameObject myCamera = GameObject.Find("Main Camera");
    //static CVDFilter filtro=myCamera.GetComponent<CVDFilter>(); 

    public AudioSource audioSource;


    async void Start()
    {
        
        //myCamera.SendMessage("getCanShake", canShake);
        //myCamera.SendMessage("CameraFade.In()");

        audioSource = audioSource.GetComponent<AudioSource>();
        CameraFade.In(2f);
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

    async void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        timeLeft -= Time.deltaTime;

        data["pen"]=penalty;
        data["end"]=end;
        PhotonNetwork.LocalPlayer.SetCustomProperties(data);
        int endCounter=0;

        //Show custom properties of all players
        int j=0;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (!end && item.CustomProperties.ContainsKey("pen"))
            {

                //Debug.Log("Player " +j + " penalty: " + item.CustomProperties["pen"]);
                penaltyValues[j]=(float)item.CustomProperties["pen"];
            }
            if (item.CustomProperties.ContainsKey("end"))
            {
                endValues[j]=(bool)item.CustomProperties["end"];
                if (endValues[j]){
                    endCounter++;
                }
            }
            endCounter=0;
            j++;

        }

        
        if (started && first) {
            timeLeft=0.5f;
            first=false;
        }

        if (!disconnect && PhotonNetwork.PlayerList.Length<2){
            FadeObject.FadeIn();
            //CameraFade.Out(2f);
            disconnect=true;
            countdown.text="Disconnection";
            await Task.Delay(5000);
            countdown.text="No contest";
            await Task.Delay(5000);
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("PlayAgain");
        }

        //Debug.Log("CanShake="+CVDFilter.canShake);

        if(!(CVDFilter.canShake)){
            audioSource.volume=0.7f;
            
            pesi[0]=0;
            pesi[1]=100;
        }
        else{
            pesi[0]=100;
            pesi[1]=0;
            audioSource.volume=1f;
        }
        filterMixer.TransitionToSnapshots(filterSnapshots, pesi, 0.05f);
        

        if (started && !end && timeLeft<0.1 && deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            // Perform your "shaking actions" here. If necessary, add suitable
            // guards in the if check above to avoid redundant handling during
            // the same shake (e.g. a minimum refractory period).
            //Debug.Log("Shake event detected at time "+Time.time);
            Vibrator.Vibrate(20);
            
            if (!(CVDFilter.canShake)){
                penalty+=Math.Abs(timeLeft)*100;
                penalty+=10;
            }
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
            FadeObject.FadeIn();
            //Finished
            end=true;
            //let's wait for the others
            countdown.text="Finished!";
            await Task.Delay(2000);
            Winner();
        }
    }
    public float Penalty(){
        return penalty;
    }
    

    async public void Winner(){
        if (!disconnect && PhotonNetwork.PlayerList.Length>1){
            CameraFade.In(2f);
            if (penaltyValues[0]<penaltyValues[1]){
                Debug.Log("Player 0 WON!");
                if (PhotonNetwork.PlayerList[0]==PhotonNetwork.LocalPlayer){
                    Debug.Log("YOU WON!");
                    countdown.text="YOU WON!";
                }
                else{
                    countdown.text="Next time";
                }
                //Do something to show that
            }
            if (penaltyValues[0]>penaltyValues[1]){
                Debug.Log("Player 1 WON!");
                if (PhotonNetwork.PlayerList[1]==PhotonNetwork.LocalPlayer){
                    Debug.Log("YOU WON!");
                    countdown.text="YOU WON!";
                }
                else{
                    countdown.text="Next time";
                }
                //Do something to show that
            }
            if(penaltyValues[0]==penaltyValues[1]){
                Debug.Log("Both are winners! Same score!");
                countdown.text="TIE";
                //Do something to show that
            }
        }
        else{
            FadeObject.FadeIn();
            countdown.text="Disconnection";
            await Task.Delay(5000);
            countdown.text="No contest";
        }
        await Task.Delay(5000);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("PlayAgain");
    }
}
}