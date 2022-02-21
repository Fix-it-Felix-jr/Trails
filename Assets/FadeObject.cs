using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class FadeObject : MonoBehaviour
{
    public GameObject FadePanel;

    //public float fadeSpeed = 0.1f;

    float value=1f;

    public static bool Fout=true;

    public static bool Fin=false;

    // Use this for initialization
    async void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (Fout && value>=0){
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, value);
            value-=0.01f;
            Debug.Log("OUT "+value);
        }
        

        if (Fin && value<=1){
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, value);
            value+=0.01f;
            Debug.Log("IN "+value);
        }

        if(value<0){
            value=0f;
        }
        if(value>1){
            value=1f;
        }
    }

    public static void FadeIn(){
        Fin=true;
        Fout=false;
    }
    public static void FadeOut()
    {
        Fin=false;
        Fout=true;
    }
}


