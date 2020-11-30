using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class IAController : Photon.MonoBehaviour
{
    public State currentState;

    public IAStats agentStats;
    public NavMeshAgent navMeshAgent;

    public PlayerController playerController;
    public CrewManager PlayerCrewManager;


    private float capturingTime;
    private bool OnCapturing = false;
    public GameObject actualCapturingPlayer=null;
    public int ID;
    public PhotonView MyPhotonView;


    public Text TimeText;
    public Slider timeSlider;
    public Image sliderBackgroungImage;
    public PhotonView owner;
    
    private int actualCapturing = 0;
    private SphereCollider CapturingCollider;
    private CapsuleCollider BodyCollider;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        MyPhotonView = GetComponent<PhotonView>();
        CapturingCollider = GetComponent<SphereCollider>();

        SetupIA();
        ID = MyPhotonView.viewID;
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }

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
        IAAgentEvents eventType = (IAAgentEvents)eventCode;
        switch (eventType)
        {
            case IAAgentEvents.sendTime:
                RecieveUpdateTime(content);
                break;
            case IAAgentEvents.sendOwner:
                RecieveUpdateOwner(content);
                break;
            case IAAgentEvents.OnCapturing:
                RecieveOnCapturing(content);
                break;
            case IAAgentEvents.sendDestination:
                break;
            case IAAgentEvents.SendOwnerLeave:
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
                capturingTime = (float)datas[1];
                TimeText.text = capturingTime.ToString("0.00");
                timeSlider.value = capturingTime / 5;
                Debug.Log("recieving ID: " + ID + " Time : " + capturingTime);
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

                if (InGameDataManager.instance.players.Count != PhotonNetwork.playerList.Length)
                {
                    InGameDataManager.instance.FindPlayerGameObjects();
                }

                int OwnerViewerId = (int)datas[1];

                GameObject player = InGameDataManager.instance.players[OwnerViewerId];

                PhotonView pv = player.GetComponent<PhotonView>();
                NavMeshAgent nmAgent = player.GetComponent<NavMeshAgent>();
                playerController = player.GetComponent<PlayerController>();

                owner = pv;

                TimeText.gameObject.SetActive(false);

                timeSlider.gameObject.SetActive(false);

                //CapturingCollider.enabled = false;

               // BodyCollider.enabled = true;

                if (pv.isMine)
                {
                    // AgentManagement.addAgentToCrew(MyPhotonView.viewID, this);
                    PlayerCrewManager.addAgentToCrew(MyPhotonView.viewID, this);
                   // Debug.LogError("add to my crew : " + CrewManager.MyCrew.Count);
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
                if (InGameDataManager.instance.players.Count != PhotonNetwork.playerList.Length)
                {
                    InGameDataManager.instance.FindPlayerGameObjects();
                }

                int CaptureViewerId = (int)datas[1];

                if (InGameDataManager.instance.players.ContainsKey(CaptureViewerId))
                {
                    GameObject player = InGameDataManager.instance.players[CaptureViewerId];
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
   
    #endregion

    #region Sender region
    private void SendUpdateTime()
    {
        object[] datas = new object[]
        {
            MyPhotonView.viewID,
            capturingTime
        };
        Debug.Log("sending ID: " + ID + " Time : " + capturingTime);

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)IAAgentEvents.sendTime, datas, false, options);
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
        PhotonNetwork.RaiseEvent((byte)IAAgentEvents.sendOwner, datas, false, options);
    }
    private void SendUpdateCapturing(bool OnCapturing, int viewId)
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
        PhotonNetwork.RaiseEvent((byte)IAAgentEvents.OnCapturing, datas, false, options);

    }
    #endregion
    public void SetupIA()
    {
        capturingTime = agentStats.capturingTime;
    }
  
    private void OnDrawGizmos()
    {
        if(currentState != null)
        {
            Gizmos.color = currentState.sceenGizmoColor;
            Gizmos.DrawWireSphere(transform.position, agentStats.lookRange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        PlayerMouvement playerMouvement = other.GetComponent<PlayerMouvement>();

        if (photonView != null && owner == null && playerMouvement != null)
        {
            SendUpdateCapturing(true, photonView.viewID);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        PlayerMouvement playerMouvement = other.GetComponent<PlayerMouvement>();

        if (photonView != null && playerMouvement != null)
        {
            if (capturingTime >= 0)
            {
                capturingTime -= Time.deltaTime;
                //timeText.text = AGENT.timeToRes.ToString();
                timeSlider.value = capturingTime / 5;
            }
            if (capturingTime < 0 && owner == null)
            {
                capturingTime = 0;
                SendUpdateTime();
                SendUpdateOwner(photonView);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        PlayerMouvement playerMouvement = other.GetComponent<PlayerMouvement>();

        if (photonView != null && playerMouvement != null)
        {
            if (capturingTime >= 0)
            {
                SendUpdateCapturing(false, 0);
                SendUpdateTime();
            }
        }
    }
}
