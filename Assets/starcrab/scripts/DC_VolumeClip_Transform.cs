using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DC_VolumeClip_Transform : MonoBehaviour
{
	public Transform clipVolume;
	private Transform thisTransform;
	private Material[] mtls;
    StarGameManager starGameManagerRef;
    public bool disable;


    void OnEnable ()
	{

        if (starGameManagerRef == null)
        {
            starGameManagerRef = StarGameManager.instance;
        }

        if (clipVolume == null)
		{
            if (starGameManagerRef.DrawVolume != null)
            {
                clipVolume = starGameManagerRef.DrawVolume.transform;
            }
		}

		thisTransform = transform;
		mtls = thisTransform.GetComponent<Renderer>().sharedMaterials;
	}

	void Update()
	{
        if (!clipVolume)
        {
            return;
        }
        if (!disable)
        {
            for (int i = 0; i < mtls.Length; i++)
            {
                mtls[i].SetMatrix("_TransformMatrix", clipVolume.worldToLocalMatrix);
            }
        }

        else
        {

            /*
             // always draws nothing
            Quaternion rot = Quaternion.Euler(1,1,1);

            Matrix4x4 m = Matrix4x4.TRS(Vector3.one, rot, Vector3.one);
            */

            Quaternion rot = Quaternion.Euler(0, 0, 0);

            Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.zero);


            for (int i = 0; i < mtls.Length; i++)
            {
                mtls[i].SetMatrix("_TransformMatrix", m);
            }
        }


	}
}
