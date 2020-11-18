using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MouvementTesting : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    private CrewManagement CrewManagement;

    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        CrewManagement = GetComponent<CrewManagement>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
               
                CrewManagement.MoveAllCrew(hit.point);
                
                NavMeshAgent.SetDestination(hit.point);
            }
        }
    }
}
