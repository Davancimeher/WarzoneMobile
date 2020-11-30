using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
public class IAAnstanciator : MonoBehaviour
{
    public byte IADefaultNmb = 20;
    public Dictionary<int, GameObject> IAObjects = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        AgentInstantiation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void AgentInstantiation()
    {
        if (PhotonNetwork.isMasterClient)
            for (byte i = 0; i < IADefaultNmb; i++)
            {
                GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "NewIA"), GetRandomLocation(), Quaternion.identity, 0, null);
                var pv = obj.GetComponent<PhotonView>();
                if (pv)
                {
                    IAObjects.Add(pv.viewID, obj);
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
