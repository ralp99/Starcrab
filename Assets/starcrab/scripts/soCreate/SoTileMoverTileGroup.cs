using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTileMoverTileGroupObject", menuName = "sos/Starcrab/NewTileMoverTileGroupObject")]

public class SoTileMoverTileGroup : ScriptableObject
{
        public enum SpecialAction { None, HaltTiles }
        public string note = "";
        [HideInInspector] public int groupId;
        //  public List<SoTileObjectCreate> tileList = new List<SoTileObjectCreate>();  // previous working
        public List<TileSublist> tileList = new List<TileSublist>();
        [HideInInspector]
        public bool culledToBeginingElementAlready;
        public int BeginningElement;
        public TileMover.Randomize randomize;
        public int tileListIterations;
        [HideInInspector] public int currentIteration;
        // [HideInInspector] public List<SoTileObjectCreate> dynamicTileListMembers = new List<SoTileObjectCreate>();
        [HideInInspector] public List<TileSublist> dynamicTileListMembers = new List<TileSublist>();
        [HideInInspector] public int currentListPosition;
        [HideInInspector] public bool didRandomRoll;



    [System.Serializable]
    public class TileSublist
    {
        public string note = "";
        public SoTileObjectCreate TileSo;
        public SpecialAction specialAction;

        [System.Serializable]

        public class instanceSpawnList
        {
            public string note = "";
            public SpawnItems spawnItems;
            public ShotDirection ShotDirection;
            public PresetDepth PresetDepth;
            public PresetHoriz PresetHoriz;
            public PresetVert PresetVert;
            public Vector3 SpawnItemCoords;
        }

        public List<instanceSpawnList> InstanceSpawnList = new List<instanceSpawnList>();
    }
    
}




