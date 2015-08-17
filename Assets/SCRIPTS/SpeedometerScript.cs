using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedometerScript : MonoBehaviour {
    public static SpeedometerScript tachO;

    public Text speedO;

	void Awake () {
        if(tachO != null)
             GameObject.Destroy(tachO);
         else
            tachO = this;

        DontDestroyOnLoad(this);
    }

    public void UpdateText(float speed)
    {
        speedO.text = speed.ToString("0") + " MPH";
    }

}
