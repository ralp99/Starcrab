using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipControllerParametersObject", menuName = "sos/Starcrab/NewShipControllerParametersObject")]
public class SoShipControllerParameters : ScriptableObject {
    public string note = "";
    public float AltitudeMin;
    public float AltitudeMax;
    public float DepthMin;
    public float DepthMax;
    public float LateralMin;
    public float LateralMax;

    public Vector3 HeroBasePos;
    public Vector3 HeroRebirthPos;

}
