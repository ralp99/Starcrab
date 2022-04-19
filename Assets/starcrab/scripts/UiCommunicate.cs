using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCommunicate : MonoBehaviour {



    StarGameManager starGameManagerRef;
    StarUiManager starUiManager;
    public GameObject MainSubpage;
    public GameObject[] Subpages;
    public GameObject ConfirmButton;
    TapToPlaceParent tapToPlaceParent;

    void Start () {
        if (!StarGameManager.instance)
        {
            return;
        }

        starGameManagerRef = StarGameManager.instance;
        starUiManager = starGameManagerRef.UiManager;
        starGameManagerRef.uiCommunicate = gameObject.GetComponent<UiCommunicate>();
	}

    public void HeroVulnerability(bool state)
    {
        starGameManagerRef.starmanMode = state;
    }

    public void ActivateConfirmButton()
    {
        ConfirmButton.SetActive(true);
        starGameManagerRef.localSceneManagement.FinalBossPlacementObject.gameObject.GetComponent<Animator>().SetBool("dim", true);
    }

    public void SetBoolControllerTrue(string boolName)
    {
        starUiManager.PerimeterBoxOutlineAnim.SetBool(boolName, true);
    }

    public void SetBoolControllerFalse(string boolName)
    {
        starUiManager.PerimeterBoxOutlineAnim.SetBool(boolName, false);
    }

    void LoadLevel(StarUiManager.LevelSelection levelSelection)
    {
        starUiManager.SceneSwitch(levelSelection);
        DestroyCurrentUIpage();
    }

    public void LoadLevelOpening()
    {
        LoadLevel(StarUiManager.LevelSelection.Opening);
    }

    public void LoadLevelShowcase()
    {
        LoadLevel(StarUiManager.LevelSelection.Showcase);
    }

    public void LoadLevelTraditional()
    {
        LoadLevel(StarUiManager.LevelSelection.Traditional);
    }

    public void LoadLevelFinalBoss()
    {
        LoadLevel(StarUiManager.LevelSelection.FinalBoss);
    }

    public void LoadLevelRestart()
    {
        starGameManagerRef.RestartGame();
        LoadLevel(StarUiManager.LevelSelection.Opening);
    }

    public void OpenStartMenu()
    {
        starUiManager.OpenStartMenu();
    }

    public void OpenOptionsMenu()
    {
        starUiManager.OpenOptionsMenu();
    }

    public void OpenInfoMenu()
    {

    }

    public void OpenGameTypeMenu()
    {
        starUiManager.OpenGameTypeMenu();
    }

    public void OpenControllerTest()
    {
        starUiManager.OpenControllerTest();
    }

    public void OpenInstructionsMenu()
    {
        starUiManager.OpenInstructionsMenu();
    }

    public void DestroyCurrentUIpage()
    {
        starUiManager.DestroyCurrentPage();
    }


    public void AudioToggle()
    {

    }

    public void CameraClipToggle ()
    {
        starGameManagerRef.ToggleCameraClipPlane();
    }

    public void InvincibilityToggle ()
    {
        // starGameManagerRef.SetHeroInvincibility(true);  // change it to opposite of what it is
        starGameManagerRef.ToggleHeroInvincibilityDebug();
    }


    //  --- PAUSE MENU ---

    public void StagePlacement()
    {
        starUiManager.GestureScaleObject.GetComponent<GestureAction>().SetGesture(false);
        starUiManager.GestureMoveRotObject.GetComponent<GestureAction>().SetGesture(false);
        starUiManager.PerimeterBoxOutlineAnim.gameObject.SetActive(true);
    }

    public void ResumeGameplay()
    {
        // hide reticle
        // destroy this window
        starGameManagerRef.TogglePause();
    }

    public void QuitGame()
    {
        starGameManagerRef.QuitGame();
    }

    IEnumerator DelayedPagesClose()
    {
        yield return new WaitForSeconds(0.05f);

        if (MainSubpage != null)
        {
            MainSubpage.SetActive(true);
        }

        if (Subpages.Length > 0)
        {
            foreach (GameObject picked in Subpages)
            {
                picked.SetActive(false);
            }
        }
    }
    

    public void EnablePlacementMode_Movement()
    {
        starUiManager.EnablePlacementMode(PlacementMode.MoveMode);
        StartCoroutine(DelayedPagesClose());
    }

    public void EnablePlacementMode_Rotate()
    {
        starUiManager.EnablePlacementMode(PlacementMode.RotateMode);
        StartCoroutine(DelayedPagesClose());
    }

    public void EnablePlacementMode_Scale()
    {
        starUiManager.EnablePlacementMode(PlacementMode.ScaleMode);
        StartCoroutine(DelayedPagesClose());
    }

    public void ToggleTutorialHelp()
    {
        starUiManager.ToggleAbilityOfTutorials();
    }

    public void ToggleDebugFPS()
    {
        starUiManager.ToggleDebugFPSobject();

    }

    public void ToggleDebugControlOrientation()
    {
        starUiManager.ToggleDebugControlOrientationObject();
    }

    public void ToggleBossPlacement()
    {

        if (tapToPlaceParent == null)
        {
            tapToPlaceParent = starGameManagerRef.FinalBossPlacementObject;
        }
       tapToPlaceParent.DoSelectMode();

        //  print("TOGGLE TTP");  // maybe unfade here?
        starGameManagerRef.localSceneManagement.FinalBossPlacementObject.gameObject.GetComponent<Animator>().SetBool("dim", false);

    }


    public void ActivateFinalBossPlacementFromPauseMenu()
    {
        // starGameManagerRef.localSceneManagement.BossPlacementObject.SetActive(true);
        LocalSceneManagement localSceneManagement = starGameManagerRef.localSceneManagement;
        localSceneManagement.FinalBossHolderAnimator.SetTrigger(localSceneManagement.FinalBossGoAwayTrigger);
        localSceneManagement.FinalBossPlacementObject.gameObject.SetActive(true);

    }

      public void ActivateBossLevelGameplay(bool firstLaunch = true)
    {
        LocalSceneManagement localSceneManagement = starGameManagerRef.localSceneManagement;
        localSceneManagement.FinalBossHolderAnimator.SetTrigger(localSceneManagement.FinalBossReturnTrigger);
        localSceneManagement.FinalBossPlacementObject.gameObject.SetActive(false);
        starGameManagerRef.ActivateBossLevelGameplay(firstLaunch);
        DestroyCurrentUIpage();
    }

    public void ActivatePhotoMode()
    {
        DestroyCurrentUIpage();
        starGameManagerRef.ObjectIsEnabled(starUiManager.UIScore, false);
    }

    public void DebugKillBoss()
    {
        DestroyCurrentUIpage();
        starGameManagerRef.BossDefeated = true;
        ResumeGameplay();

    }
}
