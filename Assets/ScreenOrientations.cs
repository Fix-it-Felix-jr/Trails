using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Device orientation = "+Screen.orientation);
        if(Screen.orientation == ScreenOrientation.Portrait){
            Debug.Log("Portrait");
            Screen.autorotateToPortrait = true;
            Screen.autorotateToLandscapeLeft=false;
            Screen.autorotateToLandscapeRight=false;
            Screen.autorotateToPortraitUpsideDown=false;

        }
        if(Screen.orientation == ScreenOrientation.LandscapeLeft){
            Debug.Log("Left");
            Screen.autorotateToLandscapeLeft=true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToLandscapeRight=false;
            Screen.autorotateToPortraitUpsideDown=false;
        }
        if(Screen.orientation == ScreenOrientation.LandscapeRight){
            Debug.Log("Right");
            Screen.autorotateToLandscapeRight=true;
            Screen.autorotateToLandscapeLeft=false;
            Screen.autorotateToPortraitUpsideDown=false;
            Screen.autorotateToPortrait = false;
        }
        if(Screen.orientation == ScreenOrientation.PortraitUpsideDown){
            Debug.Log("UpsideDown");
            Screen.autorotateToPortraitUpsideDown=true;
            Screen.autorotateToLandscapeLeft=false;
            Screen.autorotateToLandscapeRight=false;
            Screen.autorotateToPortrait = false;
        }
    }

}
