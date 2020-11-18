using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum CapturingState
{
    passive,
    OnCapturing,
    Captured
}

public class StateController : Photon.MonoBehaviour
{
    //to change
    public EnemyStats enemyStats;
    public Transform eyes;
    public State currentSate;
    public State remainState;
    public List<Transform> wayPointList;


    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    public float StateTimeElapsed = 0;

    private bool aiActive = true;

    #region Network Variables

    public GameObject ownerPV;
    private Vector3 oldDestination;
    public PhotonView AgentPhotonView;

    private SphereCollider CapturingCollider;
    private CapsuleCollider BodyCollider;

    //HUD
    private Slider slider;
    public Text timeText;
    public Image sliderBackgroungImage;

    public float actualCapturingTime;

    private GameObject actualPlayerCapturing;

    public Vector3 NewDestination;

    public CapturingState capturingState = CapturingState.passive;

    public GameObject Enemy;
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
    private void OnPhotonEvent(byte eventCode, object content, int senderId)
    {

    }
    #endregion
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
    {
        wayPointList = wayPointsFromTankManager;
        aiActive = aiActivationFromTankManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }
    private void Update()
    {
        if (!aiActive) return;
        currentSate.UpdateState(this);

    }
    private void OnDrawGizmos()
    {
        if (currentSate != null && eyes != null)
        {
            Gizmos.color = currentSate.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRaduis);
        }
    }
    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentSate = nextState;
            OnExitState();
        }
    }

    public bool checkIfCountDownElapased(float duration)
    {
        StateTimeElapsed += Time.deltaTime;
        return (StateTimeElapsed >= duration);
    }
    public float GetTimeElapased()
    {
        return StateTimeElapsed;
    }

    private void OnExitState()
    {
        StateTimeElapsed = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (actualPlayerCapturing == null)
            ChangeCapturingState(other, CapturingState.OnCapturing);

        if (capturingState == CapturingState.Captured)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                chaseTarget = other.gameObject.transform;
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (actualPlayerCapturing == null)
            ChangeCapturingState(other, CapturingState.OnCapturing);
    }
    private void OnTriggerExit(Collider other)
    {
        ChangeCapturingState(other, CapturingState.passive);
        if (actualCapturingTime > 0)
            actualPlayerCapturing = null;
        else
        {
            capturingState = CapturingState.Captured;
        }
    }
    private void ChangeCapturingState(Collider collider, CapturingState _capturingState)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        GameObject _capturingPlayer = collider.gameObject;
        if (_capturingPlayer != null)
        {
            actualPlayerCapturing = _capturingPlayer;
            capturingState = _capturingState;

        }
    }
    public void SetNewOwner()
    {
        if (ownerPV != null) return;
        if (actualPlayerCapturing == null) return;
        ownerPV = actualPlayerCapturing;
        actualPlayerCapturing = null;
        CrewManagement crewManagement = ownerPV.GetComponent<CrewManagement>();
        crewManagement.addAgentToCrew(1, this);
    }
    public void scanningTheArea()
    {

    }
    public GameObject FindClosestTarget()
    {
        Vector3 position = transform.position;
        GameObject enemy = GameObject.FindGameObjectsWithTag("Enemy")
            .OrderBy(o => (o.transform.position - position).sqrMagnitude)
            .FirstOrDefault();
        GameObject returnEnemy = null;
        if(enemy != null)
        {
            Debug.Log("Distance : " + Vector3.Distance(transform.position, enemy.transform.position));
            if (Vector3.Distance(transform.position,enemy.transform.position) < enemyStats.lookRange)
            {
                returnEnemy = enemy;
            }
        }

        return returnEnemy;
    }
}
