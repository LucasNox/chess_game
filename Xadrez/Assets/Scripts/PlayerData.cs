using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : MonoBehaviour
{
    public PieceConfig.Color team;
    public string IP = null;
    public int Port;
    private Socket oponentSocket;

    int channelID;
    int socketID;
    int connectionID;
    HostTopology topology;

    private void Start()
    {
    }

    public void createHost()
    {
        ConnectionConfig config = new ConnectionConfig();
        channelID = config.AddChannel(QosType.Reliable);
        topology = new HostTopology(config, 1);
        NetworkTransport.Init();
        if(team == PieceConfig.Color.blue)
        {
            socketID = NetworkTransport.AddHost(topology, 60000);
            Debug.Log(socketID);
        }
        else
        {
            socketID = NetworkTransport.AddHost(topology);
        }
    }

    void OnConnect(int hostID, int connectionID, NetworkError error)
    {
        //Output the given information to the console
        Debug.Log("OnConnect(hostId = " + hostID + ", connectionId = "
            + connectionID + ", error = " + error.ToString() + ")");
    }

    public bool startGame()
    {
        byte error;
        //socketID = NetworkTransport.AddHost(topology, Port);
        connectionID = NetworkTransport.Connect(socketID, "127.0.0.1", 60000, 0, out error);
        
        int outHostID;
        int outConnectionID;
        int outChannelID;
        byte[] buffer = new byte[2048];
        int receivedSize;
        NetworkEventType eventType = NetworkTransport.Receive(out outHostID, out outConnectionID, out outChannelID, buffer, buffer.Length, out receivedSize, out error);
        while(eventType != NetworkEventType.ConnectEvent)
        {
            eventType = NetworkTransport.Receive(out outHostID, out outConnectionID, out outChannelID, buffer, buffer.Length, out receivedSize, out error);
        }
        OnConnect(outHostID, outConnectionID, (NetworkError)error);
        return true;




        // Establish the local endpoint  
        // for the socket. Dns.GetHostName 
        // returns the name of the host  
        // running the application. 
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, this.Port);

        // Creation TCP/IP Socket using  
        // Socket Class Costructor 
        Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // Using Bind() method we associate a 
            // network address to the Server Socket 
            // All client that will connect to this  
            // Server Socket must know this network 
            // Address 
            listener.Bind(localEndPoint);

            // Using Listen() method we create  
            // the Client list that will want 
            // to connect to Server 
            listener.Listen(10);

            // Suspend while waiting for 
            // incoming connection Using  
            // Accept() method the server  
            // will accept connection of client 
            Debug.Log("entrando accept");
            oponentSocket = listener.Accept();
            Debug.Log("saindo accept");
        }
        catch (System.Exception) { return false; }
        return true;
    }

    public bool enterGame()
    {
        byte error;
        //socketID = NetworkTransport.AddHost(topology);
        connectionID = NetworkTransport.Connect(socketID, this.IP, this.Port, 0, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("Error: " + (NetworkError)error);
            return false;
        }
        int outHostID;
        int outConnectionID;
        int outChannelID;
        byte[] buffer = new byte[2048];
        int receivedSize;
        NetworkEventType eventType = NetworkTransport.Receive(out outHostID, out outConnectionID, out outChannelID, buffer, buffer.Length, out receivedSize, out error);
        while (eventType != NetworkEventType.ConnectEvent)
        {
            eventType = NetworkTransport.Receive(out outHostID, out outConnectionID, out outChannelID, buffer, buffer.Length, out receivedSize, out error);
        }
        OnConnect(outHostID, outConnectionID, (NetworkError)error);
        return true;
        try
        {
            // Establish the remote endpoint  
            // for the socket. This example  
            // uses port 11111 on the local  
            // computer. 
            IPHostEntry ipHost = Dns.GetHostEntry(this.IP);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, this.Port);

            // Creation TCP/IP Socket using  
            // Socket Class Costructor 
            oponentSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if (oponentSocket == null)
                Debug.Log("oponent socket null");
            try
            {
                // Connect Socket to the remote  
                // endpoint using method Connect() 
                Debug.Log("entrando connect");
                oponentSocket.Connect(ipAddr, this.Port);
                Debug.Log("saindo connect");
            }
            // Manage of Socket's Exceptions 
            catch (ArgumentNullException ane)
            {
                Debug.Log(ane);
            }
            catch (SocketException se)
            {
                Debug.Log(se);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
        return true;
    }

    void OnData(int hostId, int connectionId, int channelId, int size, NetworkError error)
    {
        //Output the deserialized message as well as the connection information to the console
        Debug.Log("OnData(hostId = " + hostId + ", connectionId = "
            + connectionId + ", channelId = " + channelId + 
            ", size = " + size + ", error = " + error.ToString() + ")");
    }

    public void sendBytes(byte[] bytes)
    {
        byte error;
        NetworkTransport.Send(socketID, connectionID, channelID, bytes, bytes.Length, out error);

        //If there is an error, output message error to the console
        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("Message send error: " + (NetworkError)error);
        }
        return;
        oponentSocket.Send(bytes);
    }

    public byte[] receiveBytes()
    {
        int outHostId;
        int outConnectionId;
        int outChannelId;
        byte[] buffer = new byte[1024];
        int receivedSize;
        byte error;

        //Set up the Network Transport to receive the incoming message, and decide what type of event
        NetworkEventType eventType = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);

        switch (eventType)
        {
            //Use this case when there is a connection detected
            case NetworkEventType.ConnectEvent:
                {
                    //Call the function to deal with the received information
                    OnConnect(outHostId, outConnectionId, (NetworkError)error);
                    break;
                }

            //This case is called if the event type is a data event, like the serialized message
            case NetworkEventType.DataEvent:
                {
                    //Call the function to deal with the received data
                    OnData(outHostId, outConnectionId, outChannelId, receivedSize, (NetworkError)error);
                    return buffer;
                }

            case NetworkEventType.Nothing:
                break;

            default:
                //Output the error
                Debug.LogError("Unknown network message type received: " + eventType);
                break;
        }
        return null;
        byte[] bytes = new byte[2048];
        oponentSocket.Receive(bytes);
        return bytes;
    }

    public void closeSocket()
    {
        byte error;
        NetworkTransport.Disconnect(socketID, connectionID, out error);
        NetworkTransport.Shutdown();
        return;
        oponentSocket.Shutdown(SocketShutdown.Both);
        oponentSocket.Close();
    }
} 
