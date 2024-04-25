using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private Game_Manager gameManager;
    public float dropHeight = 3.0f;

    public PhotonView photonView;
    public SphereCollider collider;
    public Rigidbody rb;
    public GameObject crystal;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Game_Manager>();

        collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        crystal = transform.GetChild(0).gameObject;

        //photonView.RPC("DropGemAtPosition", RpcTarget.All, transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Collision with player
        if (collision.gameObject.name == "XR Origin")
        {
            photonView.RequestOwnership();

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                string playerWizardName = "";
                PhotonView playerPhotonView = players[i].transform.GetChild(0).GetComponent<PhotonView>();
                if (playerPhotonView == null)
                {
                    return;
                }
                if (playerPhotonView.IsMine == true) // Player grabbed Gem
                {
                    playerWizardName = playerPhotonView.transform.parent.name;
                    photonView.RPC("PickUpGem", RpcTarget.All, playerWizardName);//PickUpGem(wizard);
                }
            }
        }
    }

    [PunRPC]
    public void PickUpGem(string wizardName)
    {
        Debug.Log(wizardName + " picked up gem!");

        collider.enabled = false;
        rb.useGravity = false;
        crystal.SetActive(false);

        PhotonView gameManagerPhotonView = gameManager.GetComponent<PhotonView>();
        gameManagerPhotonView.RPC("GemGrabbed", RpcTarget.All, wizardName);
    }
    
    public void DropGemAtPosition(Vector3 position, bool isIce)
    {
        //Unparent if necessary
        Debug.Log("Drop DropGemAtPosition called at " + position);
        transform.position = position;

        photonView.RPC("FixComponentOnDrop", RpcTarget.All);

        PhotonView gameManagerPhotonView = gameManager.GetComponent<PhotonView>();
        gameManagerPhotonView.RPC("GemDropped", RpcTarget.All, isIce);
    }

    [PunRPC]
    public void FixComponentOnDrop()
    {
        collider.enabled = true;
        rb.useGravity = true;
        crystal.SetActive(true);
    }
}
