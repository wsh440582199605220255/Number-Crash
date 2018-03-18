using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace MainLogic.NetWork
{
    public class Socket_Client : MonoBehaviour
    {

        public enum NetworkState
        {
            idle,
            connect,
            recive,
            end,
        }

        private static Socket client;
        private static IPAddress ip;
        public bool local = false;
        public bool allIsReady = false;

        // Use this for initialization
        private void Start()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (local) ip = IPAddress.Parse("192.168.8.100");
            else ip = IPAddress.Parse("139.199.166.200");


            Thread thread = new Thread(ConnectTheServer);
            thread.IsBackground = true;
            thread.Start();
        }

        private void ConnectTheServer()
        {
            state = NetworkState.connect;
            //be careful!
            client.Connect(new IPEndPoint(ip, 9999));
            SendCmd(Command.Ready);         //send ready after connect successful
            state = NetworkState.recive;
            while (true)
            {
                switch (state)
                {
                    case NetworkState.recive: Recive(); break;
                    case NetworkState.end: return;
                }
            }
        }

        public NetworkState state = NetworkState.idle;

        //not in main thread
        private void Recive()
        {
            byte[] buffer = new byte[1024 * 1024 * 2];
            int effective = 0;
            try
            {
                effective = client.Receive(buffer);
            }
            catch
            {
                state = NetworkState.end;
                return;
            }

            if (effective == 0)
            {
                state = NetworkState.end;
                return;
            }
            var str = Encoding.UTF8.GetString(buffer, 0, effective);

            msgQ.Enqueue(str);
        }

        private static void SendMsg(string msg)
        {
            var buffter = Encoding.UTF8.GetBytes(msg);
            client.Send(buffter);
        }

        public static void SendCmd(Command cmd)
        {
            SendMsg(new CmdData(cmd).Pack());
        }

        public static void SendChessAction(ChessAction act)
        {
            CmdData data = new CmdData(Command.ChessAction);
            data.act = act;
            SendMsg(data.Pack());

            Debug.Log("Send my chess action.");
        }

        public static void SendChessDatas(List<ChessData> cs)
        {
            CmdData data = new CmdData(Command.LayoutCompleted);
            data.datas = cs;
            SendMsg(data.Pack());
        }

        public Queue msgQ = new Queue();
        private void Update()
        {
            if (msgQ.Count > 0)
            {
                Analyze(msgQ.Dequeue() as string);
            }
        }

        //this function is not in main thread
        public static void Analyze(string str)
        {
            //Debug.Log(str);
            CmdData data = CmdData.FromJson(str);
            switch (data.cmd)
            {
                case Command.Message: break;
                case Command.allocateGroup:
                    {
                        if (data.isOk == false) Debug.Log("allocate failed.");
                        else
                        {
                            GameManagement.PUBLIC.playerGroup = data.gProperty;
                            Debug.Log("Get a group: " + data.gProperty.ToString());
                        }
                        break;
                    }
                case Command.allIsReady: GameManagement.PUBLIC.EnableLayout(); break;
                case Command.LayoutCompleted: GameManagement.PUBLIC.CompleteGroupLayout(data); break;
                case Command.GameStart: GameManagement.PUBLIC.GameStart(); break;
                case Command.ChessAction: GameManagement.PUBLIC.RunChessAction(data.act); break;
                case Command.Chat: GameManagement.PUBLIC.DisplayChat(data.text, data.sender); break;
            }
        }

        private void OnDestroy()
        {
            Debug.Log("Socket closed.");
            client.Close();
            client = null;
        }

        private void OnApplicationQuit()
        {
            if (client != null) client.Close();
        }
    }
}

/// 1)client发送ready
/// 2)server分配group并告知对应的client
/// 3)对应的client开始layout
/// 4)所有clients完成layout
/// 5)server发送gamestart
/// 6)client发送chess action
/// 7)server转发到其他的client
/// 
