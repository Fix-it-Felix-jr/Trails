using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviourPunCallbacks
{

    public void Again(){
        //You can join the lobby
        SceneManager.LoadScene("Loading");
    }
}
