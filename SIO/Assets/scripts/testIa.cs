using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class testIa : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
       // agent.SetDestination(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
