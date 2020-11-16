using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;

public class AgentInstanciator : MonoBehaviour
{
    public byte agentNmb;
    public Dictionary<int, GameObject> IAList = new Dictionary<int, GameObject>();
    private void Awake()
    {
        AgentInstantiation();
    }
    void Start()
    {
       
    }

    void Update()
    {
        
    }
    private void AgentInstantiation()
    {
        if (PhotonNetwork.isMasterClient)
            for (byte i = 0; i < agentNmb; i++)
            {
                GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "IA"), GetRandomLocation(), Quaternion.identity, 0, null);
                PhotonView photonView = obj.GetComponent<PhotonView>();
                if (!IAList.ContainsKey(photonView.viewID))
                {
                    IAList.Add(photonView.viewID, obj);
                }
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
}
