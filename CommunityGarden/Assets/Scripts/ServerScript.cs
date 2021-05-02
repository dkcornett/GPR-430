using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;


public class ServerClient
{
    public int connectionId;
    public string playerName;
}

//server game object script
// https://www.youtube.com/watch?v=qGkkaNkq8co used for reference for beginning setup

public class ServerScript : MonoBehaviour
{

    //how many people can connect? 10 is probably already too many for this type of game
    private const int MAX_CONNECTION = 10;

    //pick port
    private int port = 8888;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private bool isStarted = false;
    private byte error;

    private List<ServerClient> clients = new List<ServerClient>();

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
            case NetworkEventType.ConnectEvent:
                {
                    Debug.Log("Player " + connectionId + " has connected. ");
                    OnConnection(connectionId);
                }
                break;
            case NetworkEventType.DataEvent:
                {
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    Debug.Log(" Receiving from " + connectionId + " : " + msg);
                    string[] splitData = msg.Split('|');
                    switch (splitData[0])
                    {
                        case "NAMEIS":
                            OnNameIs(connectionId, splitData[1]);
                            break;

                        case "CNN":
                            break;

                        case "DC":
                            break;

                        default:
                            Debug.Log("invalid message");
                            break;
                    }

                    break;
                }
            case NetworkEventType.DisconnectEvent:
                {
                    NetworkTransport.Disconnect(hostId, connectionId, out error);
                    Debug.Log("Player " + connectionId + " has disconnected. ");
                    OnDisconnection(connectionId);
                }
                break;
            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    private void OnConnection(int cnnId)
    {
        // Add player to list of players
        ServerClient c = new ServerClient();
        c.connectionId = cnnId;
        c.playerName = "TEMP";
        clients.Add(c);

        // When player joins server print ID
        //Request player name and send the name of all the other players
        string msg = "ASKNAME|" + cnnId + "|";
        foreach (ServerClient sc in clients)
        {
            msg += sc.playerName + '%' + sc.connectionId + '|';
        }

        msg = msg.Trim('|');

        Send(msg, reliableChannel, cnnId);
    }
    private void OnDisconnection(int cnnId)
    {
        //remove player form client list
        clients.Remove(clients.Find(x => x.connectionId == cnnId));
        //tell everybody about it
        Send("DC|" + cnnId, reliableChannel, clients);
    }


    private void Send(string message, int channelId, int cnnId)
    {
        List<ServerClient> c = new List<ServerClient>();
        c.Add(clients.Find(x => x.connectionId == cnnId));
        Send(message, channelId, c);
    }
    //overload
    private void Send(string message, int channelId, List<ServerClient> c)
    {
        Debug.Log("Sending : " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        foreach(ServerClient sc in c)
        {
            NetworkTransport.Send(hostId, sc.connectionId, channelId, msg, message.Length * sizeof(char), out error);
        }
    }

    private void OnNameIs(int cnnId, string playerName)
    {
        // Link the name to the connectionId
        clients.Find(x => x.connectionId == cnnId).playerName = playerName;
        // Tell everybody that a new player has connected
        Send("CNN|" + playerName + '|' + cnnId, reliableChannel, clients);
    }

}
