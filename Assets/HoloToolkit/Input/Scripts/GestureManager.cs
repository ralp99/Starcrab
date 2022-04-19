using HoloToolkit;
using UnityEngine;
using UnityEngine.Events;



public class GestureManager : Singleton<GestureManager>
{

    public UnityEvent ActiveEvent;
    public UnityEvent ReleasedEvent;


    // ---- HAS SCALE BUT MOVE IS BROKEN


    // Tap and Navigation gesture recognizer.
    public UnityEngine.XR.WSA.Input.GestureRecognizer NavigationRecognizer { get; private set; }

    // Manipulation gesture recognizer.
    public UnityEngine.XR.WSA.Input.GestureRecognizer ManipulationRecognizer { get; private set; }

    // Scale gesture recognizer.
    public UnityEngine.XR.WSA.Input.GestureRecognizer ScaleRecognizer { get; private set; }



    // Currently active gesture recognizer.
    public UnityEngine.XR.WSA.Input.GestureRecognizer ActiveRecognizer { get; private set; }

    public bool IsNavigating { get; private set; }

    public Vector3 NavigationPosition { get; private set; }

    public bool IsManipulating { get; private set; }

    public Vector3 ManipulationPosition { get; private set; }




    public bool IsScaling { get; private set; }

    public Vector3 ScalePosition { get; private set; }





    public bool manualMove, manualRotate, manualScale;


    void Awake()
    {
        /* TODO: DEVELOPER CODING EXERCISE 2.b */

        // 2.b: Instantiate the NavigationRecognizer.
        NavigationRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();

        // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
        NavigationRecognizer.SetRecognizableGestures(
            UnityEngine.XR.WSA.Input.GestureSettings.Tap |
            UnityEngine.XR.WSA.Input.GestureSettings.NavigationX);

        // 2.b: Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.
        NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
        // 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
        NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        // 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
        NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        // 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 
        NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        // 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 
        NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;


        // -----------------------------------------------------------------


        // 2.b: Instantiate the NavigationRecognizer.
        ScaleRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();

        // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
        ScaleRecognizer.SetRecognizableGestures(
            UnityEngine.XR.WSA.Input.GestureSettings.Tap |
            UnityEngine.XR.WSA.Input.GestureSettings.NavigationX);

        // 2.b: Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.
        ScaleRecognizer.TappedEvent += ScaleRecognizer_TappedEvent;
        // 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
        ScaleRecognizer.NavigationStartedEvent += ScaleRecognizer_ScaleStartedEvent;
        // 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
        ScaleRecognizer.NavigationUpdatedEvent += ScaleRecognizer_ScaleUpdatedEvent;
        // 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 
        ScaleRecognizer.NavigationCompletedEvent += ScaleRecognizer_ScaleCompletedEvent;
        // 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 
        ScaleRecognizer.NavigationCanceledEvent += ScaleRecognizer_ScaleCanceledEvent;



        // -----------------------------------------------------------------




        // Instantiate the ManipulationRecognizer.
        ManipulationRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();

        // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
        ManipulationRecognizer.SetRecognizableGestures(
            UnityEngine.XR.WSA.Input.GestureSettings.ManipulationTranslate);

        // Register for the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;




        // --------------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------



        //// Instantiate the ScaleRecognizer.
        //ScaleRecognizer = new GestureRecognizer();

        //// Add the ScaleTranslate GestureSetting to the ScaleRecognizer's RecognizableGestures.
        //ScaleRecognizer.SetRecognizableGestures(


        //GestureSettings.ManipulationTranslate);
        ////    GestureSettings.ScaleTranslate);

        //// Register for the Scale events on the ScalenRecognizer.
        //ScaleRecognizer.ScaleStartedEvent += ScaleRecognizer_ScaleStartedEvent;
        //ScaleRecognizer.ScaleUpdatedEvent += ScaleRecognizer_ScaleUpdatedEvent;
        //ScaleRecognizer.ScaleCompletedEvent += ScaleRecognizer_ScaleCompletedEvent;
        //ScaleRecognizer.ScaleCanceledEvent += ScaleRecognizer_ScaleCanceledEvent;



        // --------------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------















        // --------------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------








        ResetGestureRecognizers();
    }  // END AWAKE

    void OnDestroy()
    {

        // 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.
        NavigationRecognizer.TappedEvent -= NavigationRecognizer_TappedEvent;

        NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
        NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
        NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
        NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;

        // Unregister the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;



        // --------------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------



        // Unregister the Scale events on the ScaleRecognizer.
        ScaleRecognizer.ManipulationStartedEvent -= ScaleRecognizer_ScaleStartedEvent;
        ScaleRecognizer.ManipulationUpdatedEvent -= ScaleRecognizer_ScaleUpdatedEvent;
        ScaleRecognizer.ManipulationCompletedEvent -= ScaleRecognizer_ScaleCompletedEvent;
        ScaleRecognizer.ManipulationCanceledEvent -= ScaleRecognizer_ScaleCanceledEvent;



        // --------------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------

    }  // END DESTROY

    /// <summary>
    /// Revert back to the default GestureRecognizer.
    /// </summary>
    /// 

    void Update()

    {

        if (manualRotate)
        {
            Transition(NavigationRecognizer);
        }

        if (manualMove)
        {
            Transition(ManipulationRecognizer);
        }

        if (manualScale)
        {
            Transition(ScaleRecognizer);
        }


    }


    public void ResetGestureRecognizers()
    {


       // print("RESETTING GR-------------------");

        // Default to the navigation gestures.

        ReleasedEvent.Invoke();  // WAS KINDA GOOD HERE

        //Transition(NavigationRecognizer);

        // TRY DISABLE HERE




    }

    /// <summary>
    /// Transition to a new GestureRecognizer.
    /// </summary>
    /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
    public void Transition(UnityEngine.XR.WSA.Input.GestureRecognizer newRecognizer)
    {

        // GRAB EVENT works nowhere in here!!!!! ----------------------------------------------------

        // ReleasedEvent.Invoke();  // Cant use it here, just keeps blinking

        // print("TRANSITION");

        if (newRecognizer == null)
        {

            //   ReleasedEvent.Invoke(); // no effect here
            return;
        }

        if (ActiveRecognizer != null)
        {



            if (ActiveRecognizer == newRecognizer)
            {
             //   ReleasedEvent.Invoke();  // blinks here too

                return;
            }

            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();

            //   ReleasedEvent.Invoke();  does nothing here

        }

        //   ReleasedEvent.Invoke();  // does nothing


        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    } // END TRANSITION






    // Navigation = rotation
    private void NavigationRecognizer_NavigationStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        
        ActiveEvent.Invoke();


        // 2.b: Set IsNavigating to be true.
        IsNavigating = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        NavigationPosition = relativePosition;
    }

    private void NavigationRecognizer_NavigationUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {

      //  ActiveEvent.Invoke();

        // 2.b: Set IsNavigating to be true.
        IsNavigating = true;
        //   rotateRA = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        NavigationPosition = relativePosition;
    }

    private void NavigationRecognizer_NavigationCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {

     //   ReleasedEvent.Invoke();

        // 2.b: Set IsNavigating to be false.
        IsNavigating = false; //RA DISABLED



    }

    private void NavigationRecognizer_NavigationCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {

     //   ReleasedEvent.Invoke();

        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;  // NOT EITHER
    }





    // --------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------










    private void ScaleRecognizer_ScaleStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        ActiveEvent.Invoke();

        // 2.b: Set IsNavigating to be true.
        IsScaling = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        ScalePosition = relativePosition;
    }

    private void ScaleRecognizer_ScaleUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {



        // 2.b: Set IsScaling to be true.
        IsScaling = true;
        //   rotateRA = true;

        // 2.b: Set ScalePosition to be relativePosition.
        ScalePosition = relativePosition;
    }

    private void ScaleRecognizer_ScaleCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
     //   ReleasedEvent.Invoke(); 

        // 2.b: Set IsScaling to be false.
        IsScaling = false; //RA DISABLED



    }

    private void ScaleRecognizer_ScaleCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {

      //  ReleasedEvent.Invoke();


        // 2.b: Set IsScaling to be false.
        IsScaling = false;  // NOT EITHER
    }



   







    // --------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------





    private void ManipulationRecognizer_ManipulationStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (HandsManager.Instance.FocusedGameObject != null)
        {
            ActiveEvent.Invoke();

            IsManipulating = true;

            ManipulationPosition = position;

            HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position, SendMessageOptions.DontRequireReceiver);

            //  print("moving by finger"); // RA

        }
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (HandsManager.Instance.FocusedGameObject != null)
        {

         //   ActiveEvent.Invoke();

            IsManipulating = true;

            ManipulationPosition = position;

            HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void ManipulationRecognizer_ManipulationCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)
    {

     //   ReleasedEvent.Invoke();

        IsManipulating = false;

    }

    private void ManipulationRecognizer_ManipulationCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray ray)
    {
      //  ReleasedEvent.Invoke();

        IsManipulating = false;

    }



    // --------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------


        // nav is rotation
    private void NavigationRecognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray ray)
    {

        GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

        if (focusedObject != null)
        {
            focusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
        }
    }



    // ------------------------



    private void ScaleRecognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray ray)
    {

        GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

        if (focusedObject != null)
        {
            focusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
        }
    }












    // --------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------






    //private void ScaleRecognizer_ScaleStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    //{
    //    if (HandsManager.Instance.FocusedGameObject != null)
    //    {
    //        IsScaling = true;

    //        ScalePosition = position;



    //        HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformScaleStart", position);



    //        //  print("scaling by finger"); // RA

    //    }
    //}

    //private void ScaleRecognizer_ScaleUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    //{
    //    if (HandsManager.Instance.FocusedGameObject != null)
    //    {
    //        IsScaling = true;

    //        ScalePosition = position;


    //        HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformScaleUpdate", position);
    //    }
    //}

    //private void ScaleRecognizer_ScaleCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    //{
    //    IsScaling = false;

    //}

    //private void ScaleRecognizer_ScaleCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    //{
    //    IsScaling = false;

    //}





}