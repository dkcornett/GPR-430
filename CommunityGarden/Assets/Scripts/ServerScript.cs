using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

//server game object script
// https://www.youtube.com/watch?v=qGkkaNkq8co used for reference for beginning setup

public class ServerScript : MonoBehaviour
{
    //how many people can connect? 10 is probably already too many for this type of game
    private int MAX_CONNECTION = 10;

    //pick port
    private int port = 8888;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private bool isStarted = false;
    private byte error;

    private void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        //setup for messages
        //   game events will need to hook into these
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, port, null);
        //make connectable throgh websocket
        webHostId = NetworkTransport.AddWebsocketHost(topo, port, null);

        isStarted = true;

    }

    //update function base pulled right from API
    //https://docs.unity3d.com/Manual/UNetUsingTransport.html
    private void Update()
    {
        if (!isStarted)
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
            case NetworkEventType.ConnectEvent:
                {
                    Debug.Log("Player " + connectionId + " has connected. ");
                    break;
                }
            case NetworkEventType.DataEvent:
                {
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    Debug.Log(" Player " + connectionId + " has sent: " + msg);
                    break;
                }
            case NetworkEventType.DisconnectEvent:
                {
                    NetworkTransport.Disconnect(hostId, connectionId, out error);
                    Debug.Log("Player " + connectionId + " has disconnected. ");
                    break;
                }
            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

}
