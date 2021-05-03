using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


//pull into separate script later.
public class Player
{
    public string playerName;
    public GameObject avatar;
    public int connectionId;

    public Vector2 playerVelocity;
    public Vector2 playerAcceleration;
}


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

    private int ourClientId;
    private int connectionId;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private bool hasName = false;
    private byte error;

    private string playerName;

    public GameObject playerPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

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
            hasName = true;
        }


        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        //setup for messages
        //   game events will need to hook into these
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostId, "184.171.149.137", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;

    }

    private void Start()
    {
        GameObject.Find("Canvas").SetActive(true);
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
            case NetworkEventType.DataEvent:
                {
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    Debug.Log("Receiving : " + msg);
                    string[] splitData = msg.Split('|');
                    switch (splitData[0])
                    {
                        case "ASKNAME":
                            OnAskName(splitData);
                            break;

                        case "CNN":
                            SpawnPlayer(splitData[1], int.Parse(splitData[2]));
                            break;

                        case "DC":
                            PlayerDisconnected(int.Parse(splitData[1]));
                            break;
                        case "ASKPOSITION":
                            if(hasName)
                            {  
                                OnAskPosition(splitData);
                            }
                           
                            break;

                        default:
                            Debug.Log("invalid message");
                            break;
                    }
                }
                break;
            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    private void OnAskName(string[] data)
    {
        //set this client's id
        ourClientId = int.Parse(data[1]);

        //send our name to the server
        Send("NAMEIS|" + playerName, reliableChannel);

        //create all the other players
        for (int i = 2; i < data.Length - 1; i++)
        {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]));
        }
    }

    private void SpawnPlayer(string playerName, int cnnId)
    {
        GameObject playerSpawns = Instantiate(playerPrefab) as GameObject;
        Debug.Log("Spawning Player");
        // is this ours?
        if (cnnId == ourClientId)
        {
            isStarted = true;
            playerSpawns.AddComponent<PlayerScript>();
            Debug.Log("delete canvas");
            GameObject.Find("Canvas").SetActive(false);
            
        }

        Player p = new Player();
        p.avatar = playerSpawns;
        p.playerName = playerName;
        p.connectionId = cnnId;
        p.avatar.GetComponentInChildren<TextMesh>().text = playerName;
        players.Add(cnnId, p);
    }

    private void OnAskPosition(string[] data)
    {

        //update positions of other players
        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] d = data[i].Split('%');
            Debug.Log("D= " + d);

            //don't  let the server update our position, just others' positions
            if (ourClientId != int.Parse(d[0]))
            { 
                Vector3 pos = Vector3.zero;
                pos.x = float.Parse(d[1]);
                pos.y = float.Parse(d[2]);
                players[int.Parse(d[0])].avatar.transform.position = pos;
            }

        }

        // send our own position
        Vector3 myPos = players[ourClientId].avatar.transform.position;
        //string m = "MYPOSITION|" + Compress(myPos.x).ToString() + '|' + Compress(myPos.y).ToString();
        string m = "MYPOSITION|" + myPos.x.ToString() + '|' + myPos.y.ToString();
        Debug.Log(m);
        Send(m, unreliableChannel);
    }

    private void PlayerDisconnected(int cnnId)
    {
        Destroy(players[cnnId].avatar);
        players.Remove(cnnId);
    }

    private void Send(string message, int channelId)
    {
        Debug.Log("Sending : " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);
        
    }

    private short Compress(float pos)
    {

        return (short)(pos * 10);
    }

    private float Decompress(short x)
    {

        return x / 10;
    }
}
