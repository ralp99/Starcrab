
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on 
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour
{
    StarGameManager starGameManagerRef;

    public bool useScale, useRotation, useMove;


    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;
    public float scaleMultiply = 1.0f;
    public float moveMultiply = 1.0f;

    private Vector3 manipulationPreviousPosition;
    private Vector3 scalePreviousPosition;


    private float rotationFactor;
    private float scaleFactor;

    float prevScaleX, prevScaleY, prevScaleZ;
    public bool GestureModeActive;


    private void Start()
    {
        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }
    }

    public void SetGesture(bool newState)
    {
        GestureModeActive = newState;
    }

    void Update()
    {
        if (starGameManagerRef == null)
        {
            return;
        }

        if (starGameManagerRef.GamePaused)
        {
            PerformRotation();
            PerformScaleUpdate();
        }
    }

    private void PerformRotation()
    {
        if ((GestureManager.Instance.IsNavigating) && (useRotation))

        {
            rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;
            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }
    
    void PerformManipulationStart(Vector3 position)
    {
        manipulationPreviousPosition = position;
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        if (!starGameManagerRef.GamePaused)
        {
            return;
        }

        if ((GestureManager.Instance.IsManipulating)&&(useMove) && GestureModeActive)
        {

            Vector3 moveVector = Vector3.zero;
            moveVector = position - manipulationPreviousPosition;
            manipulationPreviousPosition = position;

             transform.position += (moveVector*moveMultiply);

        }
    }


    void PerformScaleUpdate()

    {
        if ((GestureManager.Instance.IsScaling)&&(useScale))

                {

            scaleFactor = GestureManager.Instance.ScalePosition.x * RotationSensitivity;

            float usingValue = (GestureManager.Instance.ScalePosition.x * scaleMultiply);

            prevScaleX = transform.localScale.x;
            prevScaleY = transform.localScale.y;
            prevScaleZ = transform.localScale.z;

            transform.localScale += new Vector3(usingValue, usingValue, usingValue);

        }
        
    }
 
}