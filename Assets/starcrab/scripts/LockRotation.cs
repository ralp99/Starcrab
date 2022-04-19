using UnityEngine;
using System.Collections;

public class LockRotation : MonoBehaviour
{
    public bool lockX;
    public bool lockY;
    public bool lockZ;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float x = this.transform.eulerAngles.x;
        float y = this.transform.eulerAngles.y;
        float z = this.transform.eulerAngles.z;

        if (lockX)
        {
            x = 0.0f;
        }

        if (lockY)
        {
            y = 0.0f;
        }

        if (lockZ)
        {
            z = 0.0f;
        }

        this.transform.eulerAngles = new Vector3(x, y, z);
    }

}
