using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    passive,
    OnCapturing,
    OnCrew,
    Attacking
}

public class IAController : Photon.MonoBehaviour
{
    public EnemyStats _EnemyStats;

    public AgressiveBehvaiour agressiveBehvaiour;
    [HideInInspector] public Vector3 NewDestination;

    public AgentState IAState;

    private GameObject _playerCapturing = null;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    private float _timeToCapturing;

    public CrewManagement EnemyCrew;
    public GameObject owner = null;
    private bool coroutineActionRunning = false;
    private void Awake()
    {
        _timeToCapturing = _EnemyStats.capturingTime;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {

    }
    private void Update()
    {
        RunningBehaviours();
    }
    public void UpdateState(AgentState _agentState)
    {
        if (IAState != _agentState)
            IAState = _agentState;
    }
    public void RunningBehaviours()
    {
        switch (IAState)
        {
            case AgentState.passive:

                break;
            case AgentState.OnCapturing:

                break;
            case AgentState.OnCrew:
                MoveToPosition();
                break;
            case AgentState.Attacking:
                if (!coroutineActionRunning)
                    StartCoroutine(attackAction());
                break;
        }
    }

    private IEnumerator attackAction()
    {
        coroutineActionRunning = true;
        yield return new WaitForSeconds(_EnemyStats.attaqueRate);
        agressiveBehvaiour.AttackAction(this);
        coroutineActionRunning = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (_playerCapturing != null) return;
        Debug.Log("_playerCapturing == null");

        if (_timeToCapturing > 0 && IAState == AgentState.passive)
        {
            var CM = other.GetComponent<CrewManagement>();
            if (CM != null)
            {
                IAState = AgentState.OnCapturing;
                _playerCapturing = other.gameObject;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != _playerCapturing) return;

        if (_timeToCapturing > 0)
        {
            var CM = other.GetComponent<CrewManagement>();
            if (CM != null)
            {
                _timeToCapturing -= Time.deltaTime;
                Debug.Log(_timeToCapturing);
                if (_timeToCapturing <= 0)
                {
                    CM.addAgentToCrew(this);
                    setOwner(_playerCapturing);
                    IAState = AgentState.OnCrew;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != _playerCapturing) return;

        if (_timeToCapturing > 0 && IAState != AgentState.passive)
        {
            var CM = other.GetComponent<CrewManagement>();
            if (CM != null)
            {
                IAState = AgentState.passive;
                _playerCapturing = null;
            }
        }
    }

    public void setOwner(GameObject _owner)
    {
        owner = _owner;
    }
    public void MoveToPosition()
    {
        animator.SetFloat("input", navMeshAgent.remainingDistance);
        if (navMeshAgent.destination != NewDestination)
        {
            navMeshAgent.SetDestination(NewDestination);
        }
    }
    public GameObject FindClosestTargetInCrew()
    {
        Vector3 position = transform.position;
        GameObject enemy = EnemyCrew.MyCrew.Values.ToList().OrderBy(o => (o.gameObject.transform.position - position).sqrMagnitude).FirstOrDefault().gameObject;
        GameObject returnEnemy = null;
        if (enemy != null)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < _EnemyStats.lookRange)
            {
                returnEnemy = enemy;
            }
        }
        return returnEnemy;
    }
}
