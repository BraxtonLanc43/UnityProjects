using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System;

public class Client : MonoBehaviour {
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    public string clientName;
    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //called every frame
    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                {
                   // Debug.Log("OnIncomingData Client");
                    OnIncomingData(data);
                }
            }
        }
    }

    public bool connectToServer(string host, int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Error Here: " + e.Message);
        }

        return socketReady;
    }

    //sending messages to the server
    private void Send(string data)
    {
        if (!socketReady)
        {
            return;
        }
        writer.WriteLine(data);
        writer.Flush();
    }

    //read messages from the server
    private void OnIncomingData(string data)
    {
        Debug.Log("Incoming Client Data: " + data);
        string[] strData = data.Split('|');

        Debug.Log("strData[0]: " + strData[0]);
        
        if(strData[0] == "SWHO")
        {
            for (int i = 1; i < strData.Length-1; i++)
            {
                Debug.Log("For each strData: " + i);
                UserConnected(strData[i], false);
            }
            Send("CWHO|" + clientName);
        }
        else if(strData[0] == "SCNN")
        {
            UserConnected(strData[1], false);
        }
        
    }

    private void UserConnected(string name, bool host)
    {
       // Debug.Log("User Connected");
        GameClient c = new GameClient();
        c.name = name;

        players.Add(c);

       // Debug.Log("Player Count: " + players.Count);
        if (players.Count == 2)
        {
            GameManager.instance.StartGame();
        }
    }

    //on application quit
    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    //close the socket
    private void OnDisable()
    {
        CloseSocket();
    }

    //close the socket
    private void CloseSocket()
    {
        if (!socketReady)
        {
            writer.Close();
            reader.Close();
            socket.Close();
            socketReady = false;
        }
    }
}

public class GameClient
{
    public string name;
    public bool isHost;


}
