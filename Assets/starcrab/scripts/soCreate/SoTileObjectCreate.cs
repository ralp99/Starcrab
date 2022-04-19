using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTileObject", menuName = "sos/Starcrab/NewTileObject")]
public class SoTileObjectCreate : ScriptableObject {
    public string note = "";
    public GameObject ParentTile;
    public GameObject ChildTile;

    public Vector3 ParentTileRotOffset;
    public Vector3 ChildTileRotOffset;

    [System.Serializable]

    public class FeatureList

    {
        public GameObject FeatureObject;
        public Vector3 FeaturePosOffset;
        public Vector3 FeatureRotOffset;
    }

    public List<FeatureList> featureList;
}
