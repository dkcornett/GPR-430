using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//client game object script
// https://www.youtube.com/watch?v=qGkkaNkq8co used for reference for beginning setup

public class ClientScript : MonoBehaviour
{
    //how many people can connect? 10 is probably already too many for this type of game
    private int MAX_CONNECTION = 10;

    //pick port
    private int port = 8888;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private int connectionId;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private byte error;

    private string playerName;

    public void Connect()
    {
        //Does player have a name?
        string playerNameInput = GameObject.Find("NameInput").GetComponent<InputField>().text;
        if (playerNameInput == "")
        {
            Debug.Log("You must enter a name!");
                return;
        }
        else
        {
            playerName = playerNameInput;
        }


        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        //setup for messages
        //   game events will need to hook into these
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostId, "LOCALHOST", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;

    }

    //update function base pulled right from API
    //https://docs.unity3d.com/Manual/UNetUsingTransport.html
    private void Update()
    {
        if (!isConnected)
        {
            return;
        }

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing: break;
            case NetworkEventType.ConnectEvent: break;
            case NetworkEventType.DataEvent: break;
            case NetworkEventType.DisconnectEvent: break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
    }
}
