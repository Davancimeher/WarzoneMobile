using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour
{
    public Dictionary<int, IAController> MyCrew = new Dictionary<int, IAController>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void addAgentToCrew(int id, IAController agent)
    {
        if (!MyCrew.ContainsKey(id))
        {
            MyCrew.Add(id, agent);
        }
    }
    public void DeleteFromCrew(int id)
    {
        if (MyCrew.ContainsKey(id))
            MyCrew.Remove(id);
    }

    public void MoveAllCrew(Vector3 destination)
    {
        if (MyCrew.Count <= 0) return;

        List<Vector3> targetPostionList = GetPositionListAround(destination, new float[] { 3f, 5f, 7f }, new int[] { 5, 10, 20 });
        int i = -1;
        foreach (var agent in MyCrew)
        {
            i++;
            //MyCrew[i].SendUpdateDestination(MyCrew[i].MyPhotonView.viewID, targetPostionList[i]);
        }
    }

    #region crew positions
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringingDistanceArray, int[] ringingPositionArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < ringingDistanceArray.Length; i++)
        {
            positionList.AddRange(getPositionAround(startPosition, ringingDistanceArray[i], ringingPositionArray[i]));
        }
        return positionList;
    }
    private List<Vector3> getPositionAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }
    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
    #endregion
}
