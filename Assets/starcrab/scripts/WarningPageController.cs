using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPageController : MonoBehaviour {

    public float Duration = 13.13f;
    public float TextAppearDelay = 1.0f;
    public float AnimEndDelay = 11;
    public float GlowsAppearTimer = 5.5f;
    public float GlowsVisibleDuration = 1.5f;
    public float GlowsHiddenDuration = 0.35f;
    public bool DestroyAtClose = true;
    public bool ToggleHeroVulnerability = true;
    public Animator Animator;
    public string ShutdownAnimString = "shutDown";

    [Header("GameObjects")]
    public GameObject FrontOrthographicObj;
    public GameObject FrontGlowObj;
    public GameObject SideOrthographicObj;
    public GameObject SideGlowObj;
    public GameObject TopOrthographicObj;
    public GameObject TopGlowObj;
    public GameObject RotateObj;
    public GameObject TextObject;
    public GameObject[] Glows;

    private Sprite frontSprite;
    private Sprite frontGlowSprite;
    private Sprite sideSprite;
    private Sprite sideGlowSprite;
    private Sprite topSprite;
    private Sprite topGlowSprite;
    private Sprite[] rotateSprite;

    [HideInInspector]
    public SoWarningMessage soWarningMessage;

    private spriteCycle spriteCycle;

    private StarGameManager starGameManagerRef;
    private bool currentGlowState;

    IEnumerator WarningShutdownCounter()
    {
        yield return new WaitForSeconds(Duration);
        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        EndBossWarning();

        if (DestroyAtClose)
        {
            Destroy(gameObject);
        }
    }


     void EndBossWarning()
    {

        if (ToggleHeroVulnerability)
        {
            starGameManagerRef.starmanMode = false;
        }

        starGameManagerRef.BossWarningMessagePlaying = false;
    }

    IEnumerator GlowTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }
        foreach (GameObject picked in Glows)
        {
            picked.SetActive(!currentGlowState);
        }

        currentGlowState = !currentGlowState;

        float newGlowDuration = GlowsVisibleDuration;

        if (currentGlowState)
        {
            newGlowDuration = GlowsHiddenDuration;
        }

        StartCoroutine(GlowTimer(newGlowDuration));
    }

    IEnumerator TextAppearTimer()
    {
        yield return new WaitForSeconds(TextAppearDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }
        TextObject.SetActive(true);
    }

    IEnumerator AnimEndTimer()
    {
        yield return new WaitForSeconds(AnimEndDelay);

        while (starGameManagerRef.GamePaused)
        {
            yield return null;
        }

        Animator.SetTrigger(ShutdownAnimString);
    }

    private void OnDisable()
    {
        EndBossWarning();
    }

    void Start () {

        if (StarGameManager.instance != null)
        {
            starGameManagerRef = StarGameManager.instance;
        }
     
        StartCoroutine(WarningShutdownCounter());
        StartCoroutine(GlowTimer(GlowsAppearTimer));
        StartCoroutine(TextAppearTimer());
        StartCoroutine(AnimEndTimer());

        TextObject.SetActive(false);

        if (Glows.Length != 0)
        {
            foreach (GameObject picked in Glows)
            {
                picked.SetActive(false);
            }
        }

        if (ToggleHeroVulnerability)
        {
            starGameManagerRef.starmanMode = true;
        }

        if (RotateObj != null)
        {
            spriteCycle = RotateObj.GetComponent<spriteCycle>();
        }

        frontSprite = soWarningMessage.Front;
        frontGlowSprite = soWarningMessage.FrontGlow;
        sideSprite = soWarningMessage.Side;
        sideGlowSprite = soWarningMessage.SideGlow;
        topSprite =  soWarningMessage.Top;
        topGlowSprite = soWarningMessage.TopGlow;
        rotateSprite = soWarningMessage.Rotate;

        FrontOrthographicObj.GetComponent<SpriteRenderer>().sprite = frontSprite;
        FrontGlowObj.GetComponent<SpriteRenderer>().sprite = frontGlowSprite;
        SideOrthographicObj.GetComponent<SpriteRenderer>().sprite = sideSprite;
        SideGlowObj.GetComponent<SpriteRenderer>().sprite = sideGlowSprite;
        TopOrthographicObj.GetComponent<SpriteRenderer>().sprite = topSprite;
        TopGlowObj.GetComponent<SpriteRenderer>().sprite = topGlowSprite;
        RotateObj.GetComponent<SpriteRenderer>().sprite = rotateSprite[0];
        RotateObj.GetComponent<spriteCycle>().spriteNew = rotateSprite;
    }

}
