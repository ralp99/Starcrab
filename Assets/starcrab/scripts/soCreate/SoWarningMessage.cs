using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWarningMessage", menuName = "sos/Starcrab/NewWarningMessageAsset")]

public class SoWarningMessage : ScriptableObject {

    public Sprite Front;
    public Sprite FrontGlow;
    public Sprite Side;
    public Sprite SideGlow;
    public Sprite Top;
    public Sprite TopGlow;
    public Sprite[] Rotate;
    
}
