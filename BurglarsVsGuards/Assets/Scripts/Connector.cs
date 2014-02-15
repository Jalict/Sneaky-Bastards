﻿using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour {

    private static Connector instance;

    private bool messaged = false;
    private bool server = false;
    public Transform loader;

    void Start() {
        if (!instance) instance = this;
        MasterServer.RequestHostList("Break In NGJ2014");
        Object.DontDestroyOnLoad(this);
        gameObject.AddComponent<NetworkView>();
        networkView.stateSynchronization = NetworkStateSynchronization.Off;
        networkView.observed = null;
	}

    public static void AddEntity(string name, Vector3 pos, Quaternion rot, string prefab, string id) {
        NetworkViewID vid = Network.AllocateViewID();
        Transform t = ((GameObject)Instantiate(Resources.Load<GameObject>(prefab))).transform;
        t.gameObject.AddComponent<NetworkView>();
        t.networkView.viewID = vid;
        t.networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
        t.networkView.observed = t;
        instance.networkView.RPC("AddStuff", RPCMode.OthersBuffered, prefab, name, vid, id);
    }

    [RPC]
    private void AddStuff(string prefab, string name, NetworkViewID viewid, string id) {
        Transform t = ((GameObject)Instantiate(Resources.Load<GameObject>(name))).transform;
        t.gameObject.AddComponent<NetworkView>();
        t.networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
        t.networkView.observed = t;
        t.networkView.viewID = viewid;
    }

    void OnGUI() {
        if (Network.connections.Length > 0)  {
            if (!messaged) {
                Application.LoadLevel(1);
                messaged = true;
            }
            server = false;
            return;
        }
        messaged = false;

        GUILayout.BeginVertical();
        if (!server && GUILayout.Button("Host")) {
            Debug.Log("Hosting...");
            Network.InitializeServer(8, 3428, !Network.HavePublicAddress());
            MasterServer.RegisterHost("Break In NGJ2014", Random.value.ToString());
            server = true;
        }
        HostData[] data = MasterServer.PollHostList();

	    foreach(HostData element in data)
	    {
		    GUILayout.BeginHorizontal();	
		    string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
		    GUILayout.Label(name);	
		    GUILayout.Space(5);
		    string hostInfo = "[";
		    foreach (string host in element.ip)
			    hostInfo = hostInfo + host + ":" + element.port + " ";
		    hostInfo = hostInfo + "]";
		    GUILayout.Label(hostInfo);	
		    GUILayout.Space(5);
		    GUILayout.Label(element.comment);
		    GUILayout.Space(5);
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Connect"))
		    {
			    Network.Connect(element);
		    }
		    GUILayout.EndHorizontal();
	    }
        GUILayout.EndVertical();
    }
}
