using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks,IConnectionCallbacks, IMatchmakingCallbacks
{
    public GameObject button;

    public InputField createInput;
    //string createInput="Default";
    //createInput.text="Default";
    public InputField joinInput;
    //string joinInput="Default";
    //joinInput.text="Default";

    bool joined=false;

    // Start is called before the first frame update
    void Start()
    {
        JoinRoom();
        
    }

    public void CreateRoom(){
        PhotonNetwork.CreateRoom("Default");
        //PhotonNetwork.CreateRoom(createInput);
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom("Default");
        //PhotonNetwork.JoinRoom(joinInput);
    }
    public override void OnJoinedRoom(){

        joined=true;
        

    }
    void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
    {
        //if (returnCode == ErrorCode.NoRandomMatchFound)
        //{
            CreateRoom();
            // no match found, try another filter or create a room
        //}
    }
    void Update(){
        foreach (var playersName in PhotonNetwork.PlayerList)
            {
            //tell use each player who is in the room
            Debug.Log(playersName + " is in the room");
            button.gameObject.SetActive(false);
        }
        if (joined && PhotonNetwork.PlayerList.Length > 1){
            PhotonNetwork.LoadLevel("Tunnel");
            joined=false;
        }
    }
}
