using UnityEngine;
using System.Collections;

public class ValueStore : MonoBehaviour {

    public bool highScore;
    public int currentScore;
    public int beginHealth;
    public int currentHealth;
    public int beginLives;
    public int currentLives;
    public float scrollSpeed;
    //  public float controllerMultiply;
    public GameObject[] lifeOverObject;

    public GameObject[] gameOverObject;



    void Start()

    {
        currentLives = beginLives;
        currentHealth = beginHealth;
    }

    void Update()
    {

        if (currentHealth <= 0)
        {
            currentLives = currentLives - 1;
            currentHealth = beginHealth;
            foreach (GameObject picked in lifeOverObject)
            {
                picked.SetActive(true);
            }

        }

        if (currentLives <= 0)
            if (gameOverObject != null)
                foreach (GameObject picked in gameOverObject)
                {
                    picked.SetActive(true);
                }
    }




    }
