using UnityEngine;
using System.Collections;

public class shaderCom : MonoBehaviour {

    public GameObject mesh;
    public Material[] usingMat;
  //  public Shader usingShader;
    Vector3 startPos, currentPos, startScale, currentScale;
    Quaternion startRot, currentRot;
    float differencePosX, differencePosY, differencePosZ;
    float differenceScaX, differenceScaY, differenceScaZ;
    float differenceRotX, differenceRotY, differenceRotZ, differenceRotW;

    public bool zeroValues, zeroX, zeroY, zeroZ;

    void Start () {

        startPos = mesh.transform.position;
        startRot = mesh.transform.localRotation;
      //  startScale = mesh.transform.localScale;
        startScale = mesh.transform.lossyScale;
    }



    void Update() {

        currentPos = mesh.transform.position;
        currentRot = mesh.transform.localRotation;
        //  currentScale = mesh.transform.localScale;
        currentScale = mesh.transform.lossyScale;


        differencePosX = (startPos.x - currentPos.x);
        differencePosY = startPos.y - currentPos.y;
        differencePosZ = startPos.z - currentPos.z;

        differenceRotX = (startRot.x - currentRot.x);
        differenceRotY = (startRot.y - currentRot.y);
        differenceRotZ = (startRot.z - currentRot.z);
        differenceRotW = (startRot.w - currentRot.w);

        differenceScaX = (startScale.x - currentScale.x);
        differenceScaY = (startScale.y - currentScale.y);
        differenceScaZ = (startScale.z - currentScale.z);

        if (differenceScaX == 0) differenceScaX = 1;




        //print("startPos " + startPos);
        //print("currentPos " + currentPos);

        //print("differenceX " + differenceX);

        // print("dsf " +mesh.transform.localRotation);
        //  print("currentRot "+currentRot);


        if (zeroValues) differencePosX = differencePosY = differencePosZ = differenceRotX = differenceRotY = differenceRotZ = 0;
        if (zeroX) differencePosX = 0;
        if (zeroY) differencePosY = 0;
        if (zeroZ) differencePosZ = 0;

        if (differenceScaX == 1) differenceScaX = -1;

        foreach (Material pickedMaterial in usingMat)
        {
         //   pickedMaterial.SetVector("_receivedValues", new Vector4(differencePosX, differencePosY, differencePosZ, differencePosX));
         //   pickedMaterial.SetVector("_receivedScale", new Vector3((-1 * differenceScaX), differenceScaY, differenceScaZ));
    }

        // usingShader.SetVector("_receivedValues", new Vector4(differencePosX, differencePosY, differencePosZ, differencePosX));
        //   usingMat.SetVector("_receivedValues", new Vector4(differencePosX, differenceRotY, differenceRotZ, differenceRotW));




    //    Shader.SetGlobalVector("_receivedScale", new Vector3((-1 * differenceScaX), differenceScaY, differenceScaZ));

        Shader.SetGlobalVector("_receivedValues", new Vector4(differencePosX, differencePosY, differencePosZ, differencePosX));



        //  Shader.SetGlobalVector("sendX", new Vector3((-1 * differenceScaX), differenceScaY, differenceScaZ));
        Shader.SetGlobalFloat("sendX", (-1 * differenceScaX));
      //  Shader.SetGlobalFloat("_Volume_Scale", 1);

        //  Shader.SetGlobalColor("_Color", new Color (0,1,0,1) );

    }
}
