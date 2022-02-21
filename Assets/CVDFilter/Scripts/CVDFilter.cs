using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(Volume))]

public class CVDFilter : MonoBehaviour
{
public bool randomize=true;
public int timeInterval=500;
public int initialDelay=0;

    enum ColorType { Normal, Protanopia, Protanomaly, Deuteranopia, Deuteranomaly, Tritanopia, Tritanomaly, Achromatopsia, Achromatomaly }

    [SerializeField] ColorType visionType = ColorType.Normal;
    [SerializeField] ColorType newVision;
    ColorType currentVisionType;
    public static bool canShake;
    VolumeProfile[] profiles;
    Volume postProcessVolume;
    

    async void Start()
    {
        visionType = ColorType.Normal;
        currentVisionType = visionType;
        InitVolume();
        LoadProfiles();
        ChangeProfile();


        await Task.Delay(9500);

        await Task.Delay(initialDelay);

        while (randomize){
            await Task.Delay(timeInterval);
            newVision=(ColorType)Random.Range(1, 8);
            while (newVision==visionType){
                newVision=(ColorType)Random.Range(1, 8);
            }
            visionType = newVision;
            //currentVisionType = visionType;
        }
    }

    async void Update()
    {
        if (visionType != currentVisionType)
        {
            currentVisionType = visionType;
            ChangeProfile();
        }
        if (visionType == ColorType.Protanomaly){
            canShake=false;
        }
        else{
            canShake=true;
        }
    }

    void InitVolume()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.isGlobal = true;
    }

    public void LoadProfiles()
    {
        Object[] profileObjects = Resources.LoadAll("", typeof(VolumeProfile));
        profiles = new VolumeProfile[profileObjects.Length];

        for (int i = 0; i < profileObjects.Length; i++)
        {
            if (profileObjects[i].name.Contains("CVD"))
            {
                profiles[i] = (VolumeProfile)profileObjects[i];
            }
        }
    }

    void ChangeProfile()
    {
        if (profiles.Length == 0)
        {
            Debug.LogError(string.Format("[{0}]({1}) Error: Profiles could not be loaded.\nPlease ensure that they are placed in a folder names \"Resources\" and have not been renamed", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name));
            return;
        }
        else if (profiles.Length < 9)
        {
            Debug.LogWarning(string.Format("[{0}]({1}) Warning: Not all profiles could be loaded.\nPlease ensure that they are placed in a folder names \"Resources\" and have not been renamed", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name));
            return;
        }
        else if (profiles.Length > 9)
        {
            Debug.LogWarning(string.Format("[{0}]({1}) Warning: Unrecognized profiles have been loaded.\nPlease ensure that there are no other post processing profiles containing the term \"CVD\"", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name));
            return;
        }

        postProcessVolume.profile = profiles[(int)currentVisionType];
    }
}