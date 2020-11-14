using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerMouvement : Photon.MonoBehaviour
{
    private PhotonView PhotonView;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    public float Health;

    private bool UseTransformView = false;
    private Animator _animator;
    private NavMeshAgent PlayerAgent;
    public AgentManagement agentManagement;
    public int id;
    public bool IsWaitingRoom;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        PhotonView = GetComponent<PhotonView>();
        PlayerAgent = GetComponent<NavMeshAgent>();
        agentManagement = GameObject.FindObjectOfType<AgentManagement>();

        id = photonView.viewID;
        if (!photonView.isMine)
        {
            var camera = GetComponentInChildren<Camera>().gameObject;
            Destroy(camera);
            //Destroy(PlayerAgent);
            //Destroy(this);
        }
        if(SceneManager.GetActiveScene().name != "Game")
        {
            IsWaitingRoom = true;
        }
    }

    private void Update()
    {
        if (photonView.isMine)
            CheckInput();
        else
            SmoothMove();
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (UseTransformView) return;

        if (stream.isWriting)
        {
            stream.SendNext(PlayerAgent.destination);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
        }
    }
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Clicked();
        }

        _animator.SetFloat("input", PlayerAgent.remainingDistance);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("RPC_PerformTaunt", PhotonTargets.All);
        }
    }

    private void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (!IsWaitingRoom)
            {
                agentManagement.MoveAllCrew(hit.point);
            }
            PlayerAgent.SetDestination(hit.point);
        }
    }

    private void SmoothMove()
    {
        if (UseTransformView) return;
        PlayerAgent.SetDestination(TargetPosition);

    }

    [PunRPC]
    private void RPC_PerformTaunt()
    {
        _animator.SetTrigger("Upset");
    }
}
