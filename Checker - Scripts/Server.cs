using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;

public class Server : MonoBehaviour {

    public int port = 6321;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted) return;

        foreach(ServerClient c in clients)
        {
            //Is the client still connected??
            if (!isConnected(c.tcp))
            {
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    Debug.Log("Server Update data: " + data);
                    if(data != null)
                    {
                        OnIncomingData(c, data);
                    }
                }
            }
        }

        for (int i = 0; i < disconnectList.Count-1; i++)
        {
            //Tell our player somebody has disconnected



            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    //read from server
    private void OnIncomingData(ServerClient sc, string data)
    {
        Debug.Log("Server Incoming Data: " + data);

        string[] strData = data.Split('|');

        switch (strData[0])
        {
            case "CWHO":
                sc.clientName = strData[1];
                Broadcast("SCNN|" + sc.clientName, clients);
                break;
        }
    }
    
    private void StartListening()
    {
       // Debug.Log("Start Listening");
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient serverClients in clients)
        {
            allUsers += serverClients.clientName + "|";
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);
       // Debug.Log("Somebody has connected");

        StartListening();
        
        Broadcast("SWHO" + allUsers, clients[clients.Count - 1]);
    }

    //send from server
    private void Broadcast(string data, List<ServerClient> cl)
    {
        Debug.Log("Broadcast: " + data);
        foreach (ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch(Exception e)
            {
                Debug.Log("Error: " + e.Message);
            }
        }
    }

    private void Broadcast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };
        Broadcast(data, sc);
    }



    private bool isConnected(TcpClient c)
    {
        try
        {
            if(c != null && c.Client != null && c.Client.Connected)
            {
                if(c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

}

public class ServerClient
{
    public string clientName;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }


}