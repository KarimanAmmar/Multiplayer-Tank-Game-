using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace NGOTanks
{
    public enum Team
    {
        Red=0,
        Orange = 1,
        blue = 2
    }
    public enum PlayerClass
    {
        Tank = 0,
        DPS = 1
    }
    public struct PlayerData : INetworkSerializable
    {
        public FixedString64Bytes playerName;
        public PlayerClass playerClass;
        public Team TeamID;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsWriter)
            {
                FastBufferWriter writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(playerName);
                writer.WriteValueSafe(playerClass);
                writer.WriteValueSafe(TeamID);
            }
            else if(serializer.IsReader) 
            {
                FastBufferReader reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out playerName);
                reader.ReadValueSafe(out playerClass);
                reader.ReadValueSafe(out TeamID);
            }
        }
        public override string ToString()
        {
            return $"PlayerName: {playerName}, TeamID: {TeamID}, class: {playerClass}";
        }
    }
}
