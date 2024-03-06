using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;

    bool isDataReceived;

    public bool IsDataReceived()
    {
        return isDataReceived;
    }

    List<Vector3> Points = new List<Vector3>
    {
        Vector3.zero,
        Vector3.zero,
        Vector3.zero,
        Vector3.zero
    };


    void Start()
    {
        isDataReceived = false;

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

            isDataReceived = true;
        }
        else
        {
            isDataReceived = false;
        }

    }

    public static List<Vector3> ParseData(string dataString)
    {
        string[] stringArray = dataString.Split(',');

        List<Vector3> result = new List<Vector3>();

        try
        {
            result.Add(new Vector3(float.Parse(stringArray[0]),
            float.Parse(stringArray[1]),
            float.Parse(stringArray[2])));

            result.Add(new Vector3(float.Parse(stringArray[3]),
            float.Parse(stringArray[4]),
            float.Parse(stringArray[5])));

            result.Add(new Vector3(float.Parse(stringArray[6]),
            float.Parse(stringArray[7]),
            float.Parse(stringArray[8])));

            result.Add(new Vector3(float.Parse(stringArray[9]),
            float.Parse(stringArray[10]),
            float.Parse(stringArray[11])));
        }
        catch
        {
            result.Add(new Vector3(float.Parse(stringArray[0].Replace('.', ',')), 
            float.Parse(stringArray[1].Replace('.', ',')), 
            float.Parse(stringArray[2].Replace('.', ','))));

            result.Add(new Vector3(float.Parse(stringArray[3].Replace('.', ',')), 
            float.Parse(stringArray[4].Replace('.', ',')), 
            float.Parse(stringArray[5].Replace('.', ','))));

            result.Add(new Vector3(float.Parse(stringArray[6].Replace('.', ',')), 
            float.Parse(stringArray[7].Replace('.', ',')), 
            float.Parse(stringArray[8].Replace('.', ','))));

            result.Add(new Vector3(float.Parse(stringArray[9].Replace('.', ',')), 
            float.Parse(stringArray[10].Replace('.', ',')), 
            float.Parse(stringArray[11].Replace('.', ','))));
        }


        return result;
    }

    Vector3 CalculateOpticalAxisDirection(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 E = (C + D) / 2f;
        Vector3 AB = B - A;
        Vector3 AE = E - A;

        float a1 = AB.x;
        float a2 = AB.y;
        float a3 = AB.z;

        float b1 = AE.x;
        float b2 = AE.y;
        float b3 = AE.z;

        float c1 = a2 * b3 - a3 * b2;
        float c2 = a3 * b1 - a1 * b3;
        float c3 = a1 * b2 - a2 - b1;

        return new Vector3(-c1, c2, c3); //change coordinate system
    }

    Vector3 CalculateFocalLength(Vector3 A, Vector3 B)
    {
        return (A + B) / 2f;
    }

    void Update()
    {
        Vector3 A = Points[0];
        Vector3 B = Points[1];
        Vector3 C = Points[2];
        Vector3 D = Points[3];

        //calculate focal length
        Vector3 focalLength = CalculateFocalLength(A, B);

        //calculate direction of the optical axis
        Vector3 opticalAxisDirection = CalculateOpticalAxisDirection(A, B, C, D);

        //set rotation
        if(opticalAxisDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(opticalAxisDirection);
            transform.rotation = rotation;
        }

        //set position
        Vector3 position = focalLength;
        //scaling
        position.x *= 10;
        position.y *= 10;
        position.z *= 100;
        transform.position = position;
    }
}