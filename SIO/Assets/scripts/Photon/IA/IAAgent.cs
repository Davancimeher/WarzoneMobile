﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[Serializable]
public class IAAgent : Photon.MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshAgent OwnerAgent;
    private Animator _Animator;

    public PhotonView owner = null;
    private Vector3 oldDestination;
    public int ID;
    public AGENT AGENT = new AGENT(0);
    [SerializeField]
    public PhotonView MyPhotonView;
    private Slider slider;
    public Text timeText;
    public float time;
    private bool OnCapturing = false;
    private AgentManagement AgentManagement;
    private SphereCollider CapturingCollider;
    private CapsuleCollider BodyCollider;
    public Image sliderBackgroungImage;

    private bool capturingIssueFixed = false;
    private bool ownerLeaveIssueFixed = false;

    private int actualCapturing = 0;
    // Start is called before the first frame update



    #region monoBehaviour 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _Animator = GetComponent<Animator>();
        slider = GetComponentInChildren<Slider>();
        CapturingCollider = GetComponent<SphereCollider>();
        BodyCollider = GetComponent<CapsuleCollider>();
        AgentManagement = GameObject.FindObjectOfType<AgentManagement>();

        ID = AGENT.id;
        time = AGENT.timeToRes;
        MyPhotonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (time == 0 && owner == null)
            {
                //owner leave game : activate collider
                SendOwnerLeave(MyPhotonView.viewID,1f);
                ownerLeaveIssueFixed = true;
            }
        }
        if (agent.hasPath)
        {
            _Animator.SetFloat("input", agent.remainingDistance);
        }
       
    }
    #endregion

    #region Callback region
    private void OnEnable()
    {
        PhotonNetwork.OnEventCall += OnPhotonEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.OnEventCall -= OnPhotonEvent;

    }
    #endregion

    #region reciever region
    private void OnPhotonEvent(byte eventCode, object content, int senderId)
    {
        NetworkEvent eventType = (NetworkEvent)eventCode;
        switch (eventType)
        {
            case NetworkEvent.sendIATime:
                RecieveUpdateTime(content);
                break;
            case NetworkEvent.sendIAOwner:
                RecieveUpdateOwner(content);
                break;
            case NetworkEvent.OnCapturingIA:
                RecieveOnCapturing(content);
                break;
            case NetworkEvent.sendIADestination:
                RecieveDestinationUpdate(content);
                break;
            case NetworkEvent.SendIAOwnerLeave:
                RecieveOwnerLeave(content);
                break;
            default:
                break;
        }
    }

    private void RecieveUpdateTime(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 2)
        {
            if ((int)datas[0] == MyPhotonView.viewID)
            {
                time = (float)datas[1];
                timeText.text = time.ToString("0.00");
                slider.value = time / 5;
                Debug.Log("recieving ID: " + ID + " Time : " + AGENT.timeToRes);
            }
        }
    }
    private void RecieveUpdateOwner(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 2)
        {
            if ((int)datas[0] == MyPhotonView.viewID)
            {

                if (AgentManagement._PlayerManagement.players.Count != PhotonNetwork.playerList.Length)
                {
                    AgentManagement._PlayerManagement.FindPlayerGameObjects();
                }

                int OwnerViewerId = (int)datas[1];

                GameObject player = AgentManagement._PlayerManagement.players[OwnerViewerId];
                PhotonView pv = player.GetComponent<PhotonView>();
                NavMeshAgent nmAgent = player.GetComponent<NavMeshAgent>();

                owner = pv;

                OwnerAgent = nmAgent;

                timeText.gameObject.SetActive(false);

                slider.gameObject.SetActive(false);

                CapturingCollider.enabled = false;

                BodyCollider.enabled = true;

                if (pv.isMine)
                {
                    AgentManagement.addAgentToCrew(MyPhotonView.viewID, this);
                    Debug.LogError("add to my crew : " + AgentManagement.MyCrew.Count);
                }

                Debug.Log("recieving owner ID: " + ID + " for  : " + OwnerViewerId);
            }
        }
    }
    private void RecieveOnCapturing(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 3)
        {
            if ((int)datas[0] == MyPhotonView.viewID)
            {
                if ((bool)datas[2])
                {
                    Color color = new Color(0, 0, 255);
                    sliderBackgroungImage.color = color;
                }
                else
                {
                    Color color = new Color(255, 0, 0);
                    sliderBackgroungImage.color = color;
                }
                if (AgentManagement._PlayerManagement.players.Count != PhotonNetwork.playerList.Length)
                {
                    AgentManagement._PlayerManagement.FindPlayerGameObjects();
                }

                int CaptureViewerId = (int)datas[1];

                if (AgentManagement._PlayerManagement.players.ContainsKey(CaptureViewerId))
                {
                    GameObject player = AgentManagement._PlayerManagement.players[CaptureViewerId];
                    PhotonView pv = player.GetComponent<PhotonView>();
                    if (!pv.isMine)
                    {
                        CapturingCollider.enabled = (bool)datas[2];
                    }
                    actualCapturing = (int)datas[1];
                    Debug.Log("recieving on capturing for ID: " + ID + " state  : " + OnCapturing);

                }
                else
                {
                    CapturingCollider.enabled = !(bool)datas[2];
                    Debug.Log("recieving on capturing for leaving player ID" + ID + " state  : " + OnCapturing);
                }
                OnCapturing = (bool)datas[2];
              

            }
        }
    }
    private void RecieveDestinationUpdate(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 4)
        {
            if ((int)datas[0] == MyPhotonView.viewID)
            {
                Vector3 pos = new Vector3((float)datas[1], (float)datas[2], (float)datas[3]);
                Debug.Log("recieving destination : " + ID + " position : " + pos);
                MoveToPosition(pos);
            }
        }
    }
    private void RecieveOwnerLeave(object content)
    {
        object[] datas = content as object[];
        if (datas.Length == 2)
        {
            if ((int)datas[0] == MyPhotonView.viewID)
            {
                //reactivate capturing 
                time = (float)datas[1];
                CapturingCollider.enabled = true;
            }
        }
    }
    #endregion

    #region Sender region
    private void SendUpdateTime()
    {
        object[] datas = new object[]
        {
            MyPhotonView.viewID,
            AGENT.timeToRes
        };
        Debug.Log("sending ID: "+ID+" Time : "+AGENT.timeToRes);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendIATime, datas, false, options);
    }
    private void SendUpdateOwner(PhotonView _photonView)
    {
        object[] datas = new object[]
        {
            MyPhotonView.viewID,
            _photonView.viewID
        };

        Debug.Log("sending owner : " + ID + " for  : " + _photonView.viewID);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendIAOwner, datas, false, options);
    }
    private void SendUpdateCapturing(bool OnCapturing,int viewId)
    {

        object[] datas = new object[]
        {
            MyPhotonView.viewID,
            viewId,
            OnCapturing
        };

        Debug.Log("sending On Capturing : " + ID + " is  : " + OnCapturing);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.OnCapturingIA, datas, false, options);

    }
    public void SendUpdateDestination(int Id, Vector3 destination)
    {

        object[] datas = new object[]
        {
            Id,
            destination.x,
            destination.y,
             destination.z
        };

        Debug.Log("sending New destination : " + ID + " is  : "+destination);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.sendIADestination, datas, false, options);

    }
    public void SendOwnerLeave(int Id,float time)
    {

        object[] datas = new object[]
        {
            Id,
            time
        };

        Debug.Log("sending owner Leave: " + ID );

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.SendIAOwnerLeave, datas, false, options);

    }
    #endregion


    #region private functions
    public void MoveToPosition(Vector3 position)
    {
        if(position != oldDestination)
        {
            agent.SetDestination(position);
            oldDestination = position;
        }
    }
    #endregion

    #region collider region
    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        PlayerMouvement playerMouvement = other.GetComponent<PlayerMouvement>();

        if (photonView != null && owner == null && playerMouvement!=null)
        {
            SendUpdateCapturing(true,photonView.viewID);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        PlayerMouvement playerMouvement = other.GetComponent<PlayerMouvement>();

        if (photonView != null && playerMouvement != null)
        {
            if (AGENT.timeToRes >= 0)
            {
                AGENT.timeToRes -= Time.deltaTime;
                //timeText.text = AGENT.timeToRes.ToString();
                slider.value = AGENT.timeToRes / 5;
            }
            if (AGENT.timeToRes < 0 && owner == null)
            {
                AGENT.timeToRes = 0;
                SendUpdateTime();
                SendUpdateOwner(photonView);


                // RPC_TimeBroadcastingFunction(PhotonTargets.All);


                //string key = "FreeAgent" + AGENT.id.ToString();
                //PhotonNetwork.room.CustomProperties.Remove(key);

                //string keyOwned = "OwnedAgent" + AGENT.id.ToString();
                //if (!PhotonNetwork.room.CustomProperties.ContainsKey(keyOwned))
                //{
                //    PhotonNetwork.room.CustomProperties.Add(keyOwned, photonView.ownerId);
                //}

                //OwnerAgent = photonView.gameObject.GetComponent<NavMeshAgent>();

                //PlayerManagement.Instance.ModifyCrew(photonView.owner, this);
                //owner = photonView;
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        PlayerMouvement playerMouvement = other.GetComponent<PlayerMouvement>();

        if (photonView != null && playerMouvement != null)
        {
            if (AGENT.timeToRes >= 0)
            {
                SendUpdateCapturing(false,0);
                SendUpdateTime();
              //  time = AGENT.timeToRes;
              //string key = "FreeAgent" + AGENT.id.ToString();
              //PhotonNetwork.room.CustomProperties[key] = AGENT.timeToRes;

                //RPC_TimeBroadcastingFunction(PhotonTargets.All);

                // slider.value = time / 5;
                //timeText.text = time.ToString("0.00");


            }
        }
    }

   private void OnPhotonPlayerDisconnected (PhotonPlayer otherPlayer)
    {
        int viewdIdLeftPlayer =(int) otherPlayer.CustomProperties["PVID"];
        if(actualCapturing == viewdIdLeftPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                SendUpdateCapturing(false, 0);
            }
        }
    }
    #endregion
}
public class AGENT
{
    public Vector3 position;
    public float timeToRes = 5;
    public byte id;
    public float raduis = 1;
    public AGENT(byte id)
    {
        this.id = id;
    }
}
