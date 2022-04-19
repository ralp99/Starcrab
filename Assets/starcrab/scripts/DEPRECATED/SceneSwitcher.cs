using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    // DEPRECATED
    /*
    
    currently 
    0 = normal
    1 = final boss

     */

    public void SceneSwitch (int scene)
    {
        SceneManager.LoadScene(scene);
    }

}
