//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SocketIO;
//using System;

//public class Network : MonoBehaviour
//{
//    static SocketIOComponent socket;

//    public GameObject PlayerPrefab;

//    void Start()
//    {
//        socket=GetComponent<SocketIOComponent>();
//        socket.On("open", OnConnected);
//        socket.On("spawn", OnSpawned);

//    }

//    private void OnSpawned(SocketIOEvent e)
//    {
//        Debug.Log("spawned");
//        Instantiate(PlayerPrefab);
//    }

//    private void OnConnected(SocketIOEvent e)
//    {
//        Debug.Log("connected");
//        socket.Emit("move");
//    }
//}
