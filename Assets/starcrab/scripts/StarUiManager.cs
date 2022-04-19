using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum PlacementMode { MoveMode, ScaleMode, RotateMode, NoMode }

public class StarUiManager : MonoBehaviour {


    public enum LevelSelection { Opening, Showcase, Traditional, Test, FinalBoss, BasherBoss, WallBoss, SurroundBoss, Restart}

    PlacementMode placementMode;
    StarGameManager starGameManagerRef;

    public Color StandardButtonHighlightColor;
    public float DurationShowGestureOverview = 5.0f;
    public float DurationShowTutorial = 8.3f;
    public int ShowTutorialsAmount = 2;
    private GameObject tutorialMesh;
    private GameObject gestureOverviewObject;
    public GameObject Reticle;
    public GameObject ScoreUiObject;
    public GestureManager GestureManager;
    public GameObject GestureScaleObject;
    public GameObject GestureMoveRotObject;
    public Animator PerimeterBoxOutlineAnim;
    private Animator tutorialAnimator;
    public string TutorialMoveAnim = "move";
    public string TutorialScaleAnim = "scale";
    public string TutorialSpinAnim = "spin";
    public string PerimeterBoxDim = "dim";

    public bool alreadyShowedTutorials;
    public bool alreadyShowedGestureGuide;
    public bool tutorialsDisabled;

    private IEnumerator currentCoroutine;

    private int moveTutorialCounter;
    private int rotateTutorialCounter;
    private int scaleTutorialCounter;
    public float SensorActiveDelay = 0.3f;


    [Header("Prefabs")]
    public GameObject TitlePrefab;
    public GameObject PagesHolderObjectUI;
    public GameObject InstructionsMenuPrefab;
    public GameObject StartMenuPrefab;
    public GameObject GameTypePrefab;
    public GameObject OptionsMenuPrefab;
    public GameObject PauseMenuPrefab;
    public GameObject FinalBossPausePrefab;

    public GameObject ControllerTestMenuPrefab;
    public GameObject FinalBossPlacePrefab;
    public GameObject ControllerTestReadout;
    public GameObject TutorialPrefab;
    public GameObject WarningMessagePrefab;
    private GameObject generalHudFloater;
    public GameObject BubbleSelectObject;
    public GameObject UIScore;

    [Header("Anchors")]
    public GameObject ControllerTestHudLink;

    [Header("Scene Load Ints")]
    public int OpeningSceneInt = 0;
    public int ShowcaseSceneInt = 3;
    public int FinalBossSceneInt = 2;
    public int TraditionalSceneInt = 1;
    public int TestSceneInt = 3;
    public int WallBossSceneInt = 4;
    public int SurroundBossSceneInt = 5;
    public int BasherBossSceneInt = 6;
    public bool PlayedFinalBossIntroUi;

    public float BubbleSelectLoadTime = 1.35f;


    GestureAction gestureActionScaleObject;
    GestureAction gestureActionRotObject;

    [Header("Debugs")]
    public GameObject FpsReadout;
    public GameObject ControlOrientation;

    public bool showWarning;

    public AudioClip SelectGaze;
    public AudioClip SelectMade;
    public float DelayInstantiatePage = 0.3f;



    public void PlaySelectGazeSound()
    {
        GetComponent<AudioSource>().PlayOneShot(SelectGaze);
    }

    public void PlaySelectMadeSound()
    {
        starGameManagerRef.PlayAudio(GetComponent<AudioSource>(), SelectMade);
    }


    public void SetAbilityOfTutorials (bool enabled)
    {
        tutorialsDisabled = !enabled;
    }

    public void ToggleAbilityOfTutorials()
    {
        tutorialsDisabled = !tutorialsDisabled;
    }

    public void ToggleDebugFPSobject()
    {
        FpsReadout.SetActive(!FpsReadout.activeSelf);
    }

    public void ToggleDebugControlOrientationObject()
    {
        ControlOrientation.SetActive(!ControlOrientation.activeSelf);
    }


    void Start () {
        starGameManagerRef = StarGameManager.instance;
	}

    public void SceneSwitch(LevelSelection levelSelection)

    {

        if (starGameManagerRef.ForceShowcaseFeatures)
        {
            starGameManagerRef.gameplayMode = StarGameManager.GameplayMode.Showcase;
        }


        int levelInt = 0;
        switch (levelSelection)
        {
            case LevelSelection.Opening:
                levelInt = OpeningSceneInt;
                break;
            case LevelSelection.Showcase:
                levelInt = ShowcaseSceneInt;
                starGameManagerRef.gameplayMode = StarGameManager.GameplayMode.Showcase;
                break;
            case LevelSelection.Traditional:
                levelInt = TraditionalSceneInt;
                if (!starGameManagerRef.ForceShowcaseFeatures)
                {
                    starGameManagerRef.gameplayMode = StarGameManager.GameplayMode.Traditional;
                }
                break;
            case LevelSelection.FinalBoss:
                starGameManagerRef.LoadFinalBossUi();
                levelInt = FinalBossSceneInt;
                break;
            case LevelSelection.Test:
                levelInt = TestSceneInt;
                if (!starGameManagerRef.ForceShowcaseFeatures)
                {
                    starGameManagerRef.gameplayMode = StarGameManager.GameplayMode.Test;
                }
                break;
            case LevelSelection.BasherBoss:
                levelInt = BasherBossSceneInt;
                break;
            case LevelSelection.WallBoss:
                levelInt = WallBossSceneInt;
                break;
            case LevelSelection.SurroundBoss:
                levelInt = SurroundBossSceneInt;

                break;
            //    case LevelSelection.Restart:
            //    levelInt = TestSceneInt;
            //   break;
            default:
                break;
        }

        SceneManager.LoadScene(levelInt);
    }



    /// <summary>
    /// page openers
    /// </summary>

    public void OpenInstructionsMenu()
    {
        InstantiatePage(InstructionsMenuPrefab, true);
    }

    public void OpenControllerTest()
    {
        InstantiatePage(ControllerTestMenuPrefab, true);

        generalHudFloater = Instantiate(ControllerTestReadout) as GameObject;
        generalHudFloater.transform.parent = ControllerTestHudLink.transform;

        generalHudFloater.transform.localPosition = Vector3.zero;
        generalHudFloater.transform.localEulerAngles = Vector3.zero;

        generalHudFloater.SetActive(true);
    }

    public void OpenStartMenu()
    {
        InstantiatePage(StartMenuPrefab, true);
    }

    public void OpenOptionsMenu()
    {
        InstantiatePage(OptionsMenuPrefab, true);
    }

    public void OpenTitlePage()
    {
        InstantiatePage(TitlePrefab, false);
    }

    public void OpenGameTypeMenu()
    {
        InstantiatePage(GameTypePrefab, true);
    }


    public void OpenFinalBossPlaceMenu()
    {
        if (!starGameManagerRef.ForceFinalBossPlacement)
        {
            // this should only be used at 1st launch
            InstantiatePage(FinalBossPlacePrefab, true);
            PlayedFinalBossIntroUi = true;
            starGameManagerRef.MakePauseAvailable(false);
        }
        else
        {
            print("from StarUImanager - OpenFinalBossPlaceMenu");
            starGameManagerRef.ActivateBossLevelGameplay();
        }
    }

    public void PlayWarningMessage(SoWarningMessage outgoingMessage)
    {
        generalHudFloater = InstantiateAtHeadspace(WarningMessagePrefab);
        generalHudFloater.GetComponent<WarningPageController>().soWarningMessage = outgoingMessage;
    }

    
    GameObject InstantiateAtHeadspace(GameObject spawnObject)
    {
        generalHudFloater = Instantiate(spawnObject) as GameObject;
        generalHudFloater.transform.parent = ControllerTestHudLink.transform;

        generalHudFloater.transform.localPosition = Vector3.zero;
        generalHudFloater.transform.localEulerAngles = Vector3.zero;
        generalHudFloater.SetActive(true);

        return generalHudFloater;
    }
   


    public void OpenPauseMenu()
    {

        GestureManagerState(true);

        GameObject usingPausemenu = PauseMenuPrefab;
        if (PlayedFinalBossIntroUi)
        {
            usingPausemenu = FinalBossPausePrefab;
        }

        InstantiatePage(usingPausemenu, true);

        if (PlayedFinalBossIntroUi)
        {
            return;
        }

        generalHudFloater = InstantiateAtHeadspace(TutorialPrefab);
        UiCommunicate uiCommunicate = generalHudFloater.GetComponent<UiCommunicate>();

        gestureOverviewObject = uiCommunicate.Subpages[0];
        tutorialMesh = uiCommunicate.Subpages[1];
        tutorialAnimator = tutorialMesh.GetComponent<Animator>();
    }

    void InstantiatePage (GameObject prefab, bool showReticle)
    {
        DestroyCurrentPage();
        GameObject currentPage;
        currentPage = Instantiate(prefab) as GameObject;
        currentPage.transform.parent = PagesHolderObjectUI.transform;
        currentPage.transform.localPosition = Vector3.zero;
        currentPage.transform.localEulerAngles = Vector3.zero;
        currentPage.SetActive(true);
        Reticle.SetActive(showReticle); // disable if a gameplay page
    }

    public void DestroyCurrentPage()
    {
        Reticle.SetActive(false);  
        foreach (Transform child in PagesHolderObjectUI.transform)
        {
            Destroy(child.gameObject);
        }

        if (generalHudFloater != null)
        {
            Destroy(generalHudFloater);
        }
    }

    /// <summary>
    /// end page openers
    /// </summary>

    public void Unpaused()
    {
        if (tutorialMesh != null)
        {
            tutorialMesh.SetActive(false);
        }

        if (gestureOverviewObject != null)
        {
            gestureOverviewObject.SetActive(false);
        }

        GestureObjectsActionSet(false);


        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        GestureManagerState(false);
    }


    public void GestureManagerState(bool state)
    {
        // this cannot be used in this way when I have ingame gesture controls to move ship etc
      //  GestureManager.gameObject.SetActive(state);
    }


    void GestureObjectsActionSet(bool state)
    {
        if (gestureActionRotObject == null)
        {
            gestureActionRotObject = GestureScaleObject.GetComponent<GestureAction>();
        }

        if (gestureActionScaleObject == null)
        {
            gestureActionScaleObject = GestureMoveRotObject.GetComponent<GestureAction>();
        }

        gestureActionScaleObject.SetGesture(state);
        gestureActionRotObject.SetGesture(state);
    }

    public void EnablePlacementMode(PlacementMode placementMode)
    {

        GestureManager.manualMove = false;
        GestureManager.manualScale = false;
        GestureManager.manualRotate = false;

        bool tutorialCanPresent = !tutorialsDisabled;
        
        switch (placementMode)
        {
            case PlacementMode.MoveMode:
                GestureManager.manualMove = true;

                if (alreadyShowedGestureGuide && tutorialCanPresent)
                {
                    moveTutorialCounter++;
                }

                if (moveTutorialCounter > ShowTutorialsAmount)
                {
                    tutorialCanPresent = false;
                }
                break;
            case PlacementMode.ScaleMode:
                GestureManager.manualScale = true;

                if (alreadyShowedGestureGuide && tutorialCanPresent)
                {
                    scaleTutorialCounter++;
                }

                if (scaleTutorialCounter > ShowTutorialsAmount)
                {
                    tutorialCanPresent = false;
                }
                break;
            case PlacementMode.RotateMode:
                GestureManager.manualRotate = true;

                if (alreadyShowedGestureGuide && tutorialCanPresent)
                {
                    rotateTutorialCounter++;
                }

                if (rotateTutorialCounter > ShowTutorialsAmount)
                {
                    tutorialCanPresent = false;
                }
                break;
            case PlacementMode.NoMode:
                break;
            default:
                break;
        }


        Reticle.SetActive(false);  // prev only airTap did this

        // move and scale gesture objects Active


        GestureObjectsActionSet(true);

        PerimeterBoxOutlineAnim.SetTrigger(PerimeterBoxDim);

        if (!alreadyShowedGestureGuide && !tutorialsDisabled)
        {
            currentCoroutine = GestureOverviewTimer(placementMode);
            StartCoroutine(currentCoroutine);
            return;
        }


        if (tutorialCanPresent)
        {
            tutorialMesh.SetActive(true);

            switch (placementMode)
            {
                case PlacementMode.MoveMode:
                    tutorialAnimator.SetTrigger(TutorialMoveAnim);
                    break;
                case PlacementMode.RotateMode:
                    tutorialAnimator.SetTrigger(TutorialSpinAnim);
                    break;
                case PlacementMode.ScaleMode:
                    tutorialAnimator.SetTrigger(TutorialScaleAnim);
                    break;
                case PlacementMode.NoMode:
                    break;
                default:
                    break;
            }

            if (currentCoroutine == null)
            {
                currentCoroutine = TutorialTimer();
            }

            StartCoroutine(currentCoroutine);
        }
    }

    IEnumerator GestureOverviewTimer(PlacementMode placementMode)
    {
        ScoreUiObject.SetActive(false);
        gestureOverviewObject.SetActive(true);
        alreadyShowedGestureGuide = true;

        yield return new WaitForSeconds(DurationShowGestureOverview);

        gestureOverviewObject.SetActive(false);
        currentCoroutine = null;
        EnablePlacementMode(placementMode);

    }



    IEnumerator TutorialTimer()
    {
        ScoreUiObject.SetActive(false);
        yield return new WaitForSeconds(DurationShowTutorial);
        ScoreUiObject.SetActive(true);
        tutorialMesh.SetActive(false);

        currentCoroutine = null;
    }

   

}
