using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PieceConfig.Color team;
    public string IP = null;
    public int Port;
    private Socket oponentSocket;

    public bool startGame()
    {
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

    public void sendBytes(byte[] bytes)
    {
        oponentSocket.Send(bytes);
    }

    public byte[] receiveBytes()
    {
        byte[] bytes = new byte[2048];
        oponentSocket.Receive(bytes);
        return bytes;
    }

    public void closeSocket()
    {
        oponentSocket.Shutdown(SocketShutdown.Both);
        oponentSocket.Close();
    }
} 
