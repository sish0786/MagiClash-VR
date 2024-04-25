using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayer : MonoBehaviour
{
    public Transform face;
    public Transform head;
    public Transform lefthand;
    public Transform righthand;
    public PhotonView photonView;

    public Transform headRig;
    public Transform leftHandRig;
    public Transform rightHandRig;

    private void Start()
    {
        headRig = GameObject.Find("Main Camera").GetComponent<Transform>();
        leftHandRig = GameObject.Find("LeftRig").GetComponent<Transform>();
        rightHandRig = GameObject.Find("RightRig").GetComponent<Transform>();
    }

    void Update()
    {
        if (photonView.IsMine) 
        {
            if(face.gameObject.activeSelf)
                face.gameObject.SetActive(false);
                
            MapPosition(head, headRig);
            MapPosition(righthand, rightHandRig);
            MapPosition(lefthand, leftHandRig);
        }
    }

    void MapPosition(Transform target, Transform rigTransform) 
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
