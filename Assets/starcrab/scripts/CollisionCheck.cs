using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour {


    public string SearchTag = "";
   //[HideInInspector]
    public bool IsHittingTag;
    public bool monitorTaggedCollider;

    private Collider moniteredCollider;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == SearchTag)
        {
            IsHittingTag = true;

            if (moniteredCollider == null && monitorTaggedCollider)
            {
                moniteredCollider = collision.gameObject.GetComponent<Collider>();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == SearchTag)
        {
            IsHittingTag = false;
        }
    }

    private void Update()
    {
        if (!monitorTaggedCollider || moniteredCollider == null)
        {
            return;
        }

        if (!moniteredCollider.enabled)
        {
            // this means the collider object has "died" and shouldn't be considered colliding anymore
            IsHittingTag = false;
        }
    }
}
