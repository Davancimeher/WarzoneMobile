using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEyes : MonoBehaviour
{
    public CrewManagement MyCrewMnagament;
    public CrewManagement enemyCrew;
    private void Start()
    {
        MyCrewMnagament = GetComponent<CrewManagement>();
    }

    // Update is called once per frame
    private void Update()
    {
        FindClosestTarget();
        if (enemyCrew != null)
        {
            if (enemyCrew != MyCrewMnagament.enemyCrew)
            {
                MyCrewMnagament.enemyCrew = enemyCrew;
                MyCrewMnagament.AttackWithCrew();
            }
        }
    }

    public void FindClosestTarget()
    {
        Vector3 position = transform.position;
        GameObject enemy = GameObject.FindGameObjectsWithTag("Enemy")
            .OrderBy(o => (o.transform.position - position).sqrMagnitude)
            .FirstOrDefault();
        if (enemy != null)
        {
            // Debug.Log("Distance : " + Vector3.Distance(transform.position, enemy.transform.position));
            if (Vector3.Distance(transform.position, enemy.transform.position) < 7f)
            {
                enemyCrew = GetCrewManagement(enemy);
                if (enemyCrew != null)
                    Debug.Log("Find enemy Crew !");
            }
        }
    }

    private CrewManagement GetCrewManagement(GameObject _objectToScan)
    {
        CrewManagement crew = null;
        var iac = _objectToScan.GetComponent<IAController>();

        if (iac != null)
        {
            crew = iac.owner.GetComponent<CrewManagement>();
        }
        else
        {
            crew = _objectToScan.GetComponent<CrewManagement>();
        }
        return crew;
    }
}
public class test : StateMachineBehaviour
{

}