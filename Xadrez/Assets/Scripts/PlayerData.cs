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
    private Socket oponentSocket;

    public bool startGame()
    {
        // Establish the local endpoint  
        // for the socket. Dns.GetHostName 
        // returns the name of the host  
        // running the application. 
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

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
            oponentSocket = listener.Accept();
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
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using  
            // Socket Class Costructor 
            oponentSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Connect Socket to the remote  
                // endpoint using method Connect() 
                oponentSocket.Connect(localEndPoint);
            }
            // Manage of Socket's Exceptions 
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
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
} 
