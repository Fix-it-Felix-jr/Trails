using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Threading.Tasks;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    async void Start()
    {
        await Task.Delay(1000);
        CreateRoom();
        
        while(true){
            JoinRoom();
            await Task.Delay(3000);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom(){
        //PhotonNetwork.CreateRoom(createImput.text);
        PhotonNetwork.CreateRoom("Default");
    }

    public void JoinRoom(){
        //PhotonNetwork.JoinRoom(joinInput.text);
        PhotonNetwork.JoinRoom("Default");
    }
    public override void OnJoinedRoom(){
        while(true){
            if (PhotonNetwork.PlayerList.Length > 1){
                PhotonNetwork.LoadLevel("Tunnel");
            }
        }
        

    }
}
