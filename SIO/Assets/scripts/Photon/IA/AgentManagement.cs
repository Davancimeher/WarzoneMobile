using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class AgentManagement : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _agentCostumeProperties = new ExitGames.Client.Photon.Hashtable();
    private  List<IAAgent> iAAgents = new List<IAAgent>();
    private Dictionary<byte, GameObject> GameObjectsDict = new Dictionary<byte, GameObject>();
    private Dictionary<byte, IAAgent> IAagentDict = new Dictionary<byte, IAAgent>();
    

    [SerializeField]
    private  List<GameObject> ia = new List<GameObject>();

    public Dictionary<int,IAAgent> MyCrew = new Dictionary<int, IAAgent>();
    public List<IAAgent> MyCrewMember = new List<IAAgent>();

    public PlayerManagement _PlayerManagement;

    private byte agentNmb = 20;

    private PhotonView photonView;

    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        PhotonNetwork.automaticallySyncScene = true;
        _PlayerManagement = GameObject.FindObjectOfType<PlayerManagement>();
        AgentInstantiation();
    }

   

    private void AgentInstantiation()
    {
        if(PhotonNetwork.isMasterClient)
        for (byte i = 0; i < agentNmb; i++)
        {
            AGENT agent = new AGENT(i);
            agent.position = GetRandomLocation();
            GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "IA"), agent.position, Quaternion.identity, 0,null);
            var iaAgent = obj.GetComponent<IAAgent>();
            iaAgent.AGENT = agent;
        
        }
    }
  
    Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
    public void addAgentToCrew(int id,IAAgent agent)
    {
        if (!MyCrew.ContainsKey(id))
        {
            MyCrew.Add(id, agent);
            MyCrewMember.Add(agent);
        }
    }
    public void DeleteFromCrew(int id)
    {
        if (MyCrew.ContainsKey(id))
            MyCrew.Remove(id);
    }

    #region crew position 

    public void MoveAllCrew(Vector3 destination)
    {
        if (MyCrew.Count <= 0) return;

        List<Vector3> targetPostionList = GetPositionListAround(destination, new float[] {3f,5f,7f}, new int[] { 5,10,20});

        for (int i = 0; i < MyCrew.Count; i++)
        {
            MyCrewMember[i].SendUpdateDestination(MyCrewMember[i].MyPhotonView.viewID, targetPostionList[i]);
        }
    }
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
        return Quaternion.Euler(0, angle,0 ) * vec;
    }
    #endregion
}
