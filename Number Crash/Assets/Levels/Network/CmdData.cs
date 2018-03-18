using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MainLogic
{
    [Serializable]
    public struct ChessAction
    {
        public Vector2Int chessPos;
        public Vector2Int targetNodePos;

        public ChessAction(Vector2Int chessPos, Vector2Int targetNodePos)
        {
            this.chessPos = chessPos;
            this.targetNodePos = targetNodePos;
        }

        public bool IsNULL() { return (chessPos == Vector2Int.zero && targetNodePos == Vector2Int.zero); }
        public static ChessAction NULL = new ChessAction(Vector2Int.zero, Vector2Int.zero);
    }
}

namespace MainLogic.NetWork
{
    public enum Command
    {
        Ready,              //send by client,the server will boardcast it to all clients
        allocateGroup,      //send by the server  
        allIsReady,         //send by the server
        LayoutCompleted,    //send by client,the server will boardcast it to all clients
        GameStart,          //send by the server if all clients are layout completed
        ChessAction,        //send by client,the server will boardcast it to all clients   
        EndGame,
        Chat,
        Message,            //undefined simple message
    } 

    [Serializable]
    public struct ChessData
    {
        public CType type;
        public Vector2Int pos;

        public ChessData(CType type, Vector2Int pos)
        {
            this.type = type;
            this.pos = pos;
        }
    }

    [Serializable]
    public class CmdData
    {
        public CmdData(Command cmd, bool isOk = true)
        {
            this.cmd = cmd;
            this.sender = GameManagement.PUBLIC.playerGroup;
            this.isOk = isOk;
        }

        public static CmdData FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CmdData>(json);
        }

        public Command cmd;
        public Group sender;
        public bool isOk;

        #region Undefined
        public List<ChessData> datas;
        public ChessAction act;
        public Group gProperty;
        public string text;
        #endregion
        public string Pack() { return JsonConvert.SerializeObject(this); }
    }
}