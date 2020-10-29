using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if(photonView != null && photonView.isMine)
        {
            PlayerManagement.Instance.ModifyHealth(photonView.owner, -10);
        }
    }

}
