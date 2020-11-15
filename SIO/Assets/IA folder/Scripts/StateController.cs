using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateController : MonoBehaviour
{
    public EnemyStats enemyStats;
    public Transform eyes;
    public State currentSate;
    public State remainState;
    public PhotonView Owner;


    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    public float StateTimeElapsed = 0;

    private bool aiActive=true;

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
        if(nextState != remainState)
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
}
