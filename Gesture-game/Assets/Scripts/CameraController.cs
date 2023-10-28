using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;

    List<Vector3> Points = new List<Vector3>
    {
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero
    };

    void Start()
    {
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
    }

    void GetData()
    {
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        client = server.AcceptTcpClient();

        running = true;
        while (running)
        {
            Connection();
        }
        server.Stop();

    }

    void Connection()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        if (dataReceived != null && dataReceived != "")
        {
            Points = ParseData(dataReceived);
            nwStream.Write(buffer, 0, bytesRead);
        }
    }

    public static List<Vector3> ParseData(string dataString)
    {
        string[] stringArray = dataString.Split(',');

        List<Vector3> result = new List<Vector3>()
        {
            new Vector3(float.Parse(stringArray[0]),
            float.Parse(stringArray[1]),
            float.Parse(stringArray[2])),

            new Vector3(float.Parse(stringArray[3]),
            float.Parse(stringArray[4]),
            float.Parse(stringArray[5])),

            new Vector3(float.Parse(stringArray[6]),
            float.Parse(stringArray[7]),
            float.Parse(stringArray[8])),

            new Vector3(float.Parse(stringArray[9]),
            float.Parse(stringArray[10]),
            float.Parse(stringArray[11]))
        };
        return result;
    }

    void Update()
    {
        Vector3 A = Points[0];
        Vector3 B = Points[1];
        Vector3 C = Points[2];
        Vector3 D = Points[3];

        Vector3 E = new Vector3((C.x + D.x) / 2.0f, (C.y + D.y) / 2.0f, (C.z + D.z) / 2.0f);
    }
}