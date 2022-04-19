using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStrobes : MonoBehaviour {

    public GameObject[] StrobingObjects;
    public Color[] Colors;
    public float Speed = 0.35f;
    public float SpeedJitter = 0.07f;
    public float useSpeed;

    public Dictionary<GameObject, Color> ObjectInitColors =
    new Dictionary<GameObject, Color>();

    public Dictionary<GameObject, Color> ObjectGoalColors =
    new Dictionary<GameObject, Color>();

    private Color[] origColor;
    bool changeColor = true;

    StarGameManager starGameManagerRef;

    void Start () {


        if (StrobingObjects.Length != 0)
        {
            for (int i = 0; i < StrobingObjects.Length; i++)
            {
                ObjectInitColors.Add(StrobingObjects[i], StrobingObjects[i].GetComponent<Renderer>().material.color);
                ObjectGoalColors.Add(StrobingObjects[i], Colors[Random.Range(0, Colors.Length)]);
            }
        }
        ResetUseSpeed();
    }

    void StarGameManagerRefChecker()
    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }
    }

    private void OnEnable()
    {
        StarGameManagerRefChecker();
        changeColor = true;
    }

    void ResetUseSpeed()
    {
        useSpeed = Random.Range(Speed, Speed + SpeedJitter);
    }

    IEnumerator ColorFlipper()
    {
        yield return new WaitForSeconds(useSpeed);
        if (starGameManagerRef != null)
        {
            while (starGameManagerRef.GamePaused)
            {
                yield return null;
            }
        }
        changeColor = !changeColor;
    }


    void ColorShifter()
    {

        if (changeColor)
        {
            changeColor = false;
            
            foreach (GameObject picked in StrobingObjects)
            {
                if (picked.GetComponent<Renderer>().material.color == ObjectInitColors[picked])
                {
                    picked.GetComponent<Renderer>().material.color = ObjectGoalColors[picked];
                }
                else
                {
                    picked.GetComponent<Renderer>().material.color = Colors[Random.Range(0, Colors.Length)];
                }
            }
            
            StartCoroutine(ColorFlipper());
        }


        foreach (GameObject picked in StrobingObjects)
        {
            picked.GetComponent<Renderer>().material.color =
               Color.Lerp(picked.GetComponent<Renderer>().material.color, ObjectGoalColors[picked], Time.deltaTime * useSpeed);
        }
        
    }
    
    void Update () {
        ColorShifter();
    }
}
