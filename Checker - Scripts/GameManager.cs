using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;

    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public InputField nameInput;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        DontDestroyOnLoad(gameObject);
	}

    public void OnConnectButton()
    {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }

    public void HostButton()
    {
        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();

            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if(c.clientName == "")
            {
                c.clientName = "Host";
            }
            c.connectToServer("127.0.0.1", 6321);
        }
        catch(Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        mainMenu.SetActive(false);
        serverMenu.SetActive(true);
    }

    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if(hostAddress == "")
        {
            hostAddress = "127.0.0.1";
        }

        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if(c.clientName == "")
            {
                c.clientName = "Host";
            }
            c.connectToServer(hostAddress, 6321);
            connectMenu.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);

        Server s = FindObjectOfType<Server>();
        if(s != null)
        {
            Destroy(s.gameObject);
        }

        Client c = FindObjectOfType<Client>();
        if (c != null)
        {
            Destroy(c.gameObject);
        }
    }
	
    public void StartGame()
    {
        Debug.Log("StartGame()");
        SceneManager.LoadScene("Game");
    }
}
