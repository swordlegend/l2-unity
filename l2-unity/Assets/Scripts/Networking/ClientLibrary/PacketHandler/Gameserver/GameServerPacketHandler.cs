﻿using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class GameServerPacketHandler : ServerPacketHandler
{
    public override void HandlePacket(byte[] data) {
        GameServerPacketType packetType = (GameServerPacketType)data[0];
        //if (GameClient.Instance.LogReceivedPackets && packetType != GameServerPacketType.Ping) {
            Debug.Log("[" + Thread.CurrentThread.ManagedThreadId + "] [GameServer] Received packet:" + packetType);
        //}
        switch (packetType) {
            case GameServerPacketType.Ping:
                OnPingReceive();
                break;
            case GameServerPacketType.Key:
                OnKeyReceive(data);
                break;
            case GameServerPacketType.LoginFail:
                OnLoginFail(data);
                break;
            case GameServerPacketType.CharSelectionInfo:
                OnCharSelectionInfoReceive(data);
                break;
            case GameServerPacketType.MessagePacket:
                OnMessageReceive(data);
                break;
            case GameServerPacketType.SystemMessage:
                OnSystemMessageReceive(data);
                break;
            case GameServerPacketType.PlayerInfo:
                OnPlayerInfoReceive(data);
                break;
            case GameServerPacketType.ObjectPosition:
                OnUpdatePosition(data);
                break;
            case GameServerPacketType.RemoveObject:
                OnRemoveObject(data);
                break;
            case GameServerPacketType.ObjectRotation:
                OnUpdateRotation(data);
                break;
            case GameServerPacketType.ObjectAnimation:
                OnUpdateAnimation(data);
                break;
            case GameServerPacketType.ApplyDamage:
                OnInflictDamage(data);
                break;
            case GameServerPacketType.NpcInfo:
                OnNpcInfoReceive(data);
                break;
            case GameServerPacketType.ObjectMoveTo:
                OnObjectMoveTo(data);
                break;
            case GameServerPacketType.UserInfo:
                OnUserInfoReceive(data);
                break;
            case GameServerPacketType.ObjectMoveDirection:
                OnUpdateMoveDirection(data);
                break;
            case GameServerPacketType.GameTime:
                OnUpdateGameTime(data);
                break;
            case GameServerPacketType.EntitySetTarget:
                OnEntitySetTarget(data);
                break;
            case GameServerPacketType.AutoAttackStart:
                OnEntityAutoAttackStart(data);
                break;
            case GameServerPacketType.AutoAttackStop:
                OnEntityAutoAttackStop(data);
                break;
            case GameServerPacketType.ActionFailed:
                OnActionFailed(data);
                break;
        }
    }

    protected override byte[] DecryptPacket(byte[] data) {
        Debug.Log("ENCRYPTED: " + StringUtils.ByteArrayToString(data));

        GameClient.Instance.GameCrypt.Decrypt(data);

        Debug.Log("DECRYPTED: " + StringUtils.ByteArrayToString(data));

        return data;
    }

    private void OnPingReceive() {
        long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        int ping = _timestamp != 0 ? (int)(now - _timestamp) : 0;
        //Debug.Log("Ping: " + ping + "ms");
        _client.Ping = ping;

        Task.Delay(1000).ContinueWith(t => {
            if(!_tokenSource.IsCancellationRequested) {
                ((GameClientPacketHandler) _clientPacketHandler).SendPing();
                _timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }

            Task.Delay(GameClient.Instance.ConnectionTimeoutMs).ContinueWith(t => {
                if(!_tokenSource.IsCancellationRequested) {
                    long now2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    if(now2 - _timestamp >= GameClient.Instance.ConnectionTimeoutMs) {
                        Debug.Log("Connection timed out");
                        _client.Disconnect();
                    }
                }
            }, _tokenSource.Token);
        }, _tokenSource.Token);
    }

    private void OnKeyReceive(byte[] data) {
        KeyPacket packet = new KeyPacket(data);

        if (!packet.AuthAllowed) {
            Debug.LogError("Gameserver connect not allowed.");
            EventProcessor.Instance.QueueEvent(() => GameClient.Instance.Disconnect());
            EventProcessor.Instance.QueueEvent(() => LoginClient.Instance.Disconnect());
            return;
        }

        GameClient.Instance.EnableCrypt(packet.BlowFishKey);

        _eventProcessor.QueueEvent(() => ((GameClientPacketHandler)_clientPacketHandler).SendAuth());
    }

    private void OnLoginFail(byte[] data) {
        PlayFailPacket packet = new PlayFailPacket(data);
        EventProcessor.Instance.QueueEvent(() => GameClient.Instance.Disconnect());
        EventProcessor.Instance.QueueEvent(() => LoginClient.Instance.Disconnect());

        Debug.LogError($"Gameserver login failed reason: " +
            $"{Enum.GetName(typeof(LoginFailPacket.LoginFailedReason), packet.FailedReason)}");
    }

    private void OnCharSelectionInfoReceive(byte[] data) {
        EventProcessor.Instance.QueueEvent(() => LoginClient.Instance.Disconnect());

        _eventProcessor.QueueEvent(() => GameClient.Instance.OnAuthAllowed());
    }

    private void OnMessageReceive(byte[] data) {
        ReceiveMessagePacket packet = new ReceiveMessagePacket(data);
        String sender = packet.Sender;
        String text = packet.Text;
        ChatMessage message = new ChatMessage(sender, text);
        _eventProcessor.QueueEvent(() => ChatWindow.Instance.ReceiveChatMessage(message));
    }

    private void OnSystemMessageReceive(byte[] data) {
        SystemMessagePacket packet = new SystemMessagePacket(data);
        SystemMessage message = packet.Message;
        _eventProcessor.QueueEvent(() => ChatWindow.Instance.ReceiveChatMessage(message));
    }

    private void OnPlayerInfoReceive(byte[] data) {
        PlayerInfoPacket packet = new PlayerInfoPacket(data);
        _eventProcessor.QueueEvent(() => {
            World.Instance.SpawnPlayer(packet.Identity, packet.Status, packet.Stats, packet.Appearance);
            GameManager.Instance.OnPlayerInfoReceived();
        });
    }

    private void OnUserInfoReceive(byte[] data) {
        UserInfoPacket packet = new UserInfoPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.SpawnUser(packet.Identity, packet.Status, packet.Stats, packet.Appearance));
    }

    private void OnUpdatePosition(byte[] data) {
        UpdatePositionPacket packet = new UpdatePositionPacket(data);
        int id = packet.Id;
        Vector3 position = packet.Position;
        _eventProcessor.QueueEvent(() => World.Instance.UpdateObjectPosition(id, position));
    }

    private void OnRemoveObject(byte[] data) {
        RemoveObjectPacket packet = new RemoveObjectPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.RemoveObject(packet.Id));
    }

    private void OnUpdateRotation(byte[] data) {
        UpdateRotationPacket packet = new UpdateRotationPacket(data);
        int id = packet.Id;
        float angle = packet.Angle;
        _eventProcessor.QueueEvent(() => World.Instance.UpdateObjectRotation(id, angle));
    }

    private void OnUpdateAnimation(byte[] data) {
        UpdateAnimationPacket packet = new UpdateAnimationPacket(data);
        int id = packet.Id;
        int animId = packet.AnimId;
        float value = packet.Value;
        _eventProcessor.QueueEvent(() => World.Instance.UpdateObjectAnimation(id, animId, value));
    }

    private void OnInflictDamage(byte[] data) {
        InflictDamagePacket packet = new InflictDamagePacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.InflictDamageTo(packet.SenderId, packet.TargetId, packet.Value, packet.NewHp, packet.CriticalHit)); 
    }

    private void OnNpcInfoReceive(byte[] data) {
        NpcInfoPacket packet = new NpcInfoPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.SpawnNpc(packet.Identity, packet.Status, packet.Stats));
    }

    private void OnObjectMoveTo(byte[] data) {
        ObjectMoveToPacket packet = new ObjectMoveToPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.UpdateObjectDestination(packet.Id, packet.Pos, packet.Speed, packet.Walking));

    }

    private void OnUpdateMoveDirection(byte[] data) {
        UpdateMoveDirectionPacket packet = new UpdateMoveDirectionPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.UpdateObjectMoveDirection(packet.Id, packet.Speed, packet.Direction));
    }

    private void OnUpdateGameTime(byte[] data) {
        GameTimePacket packet = new GameTimePacket(data);
        _eventProcessor.QueueEvent(() => WorldClock.Instance.SynchronizeClock(packet.GameTicks, packet.TickDurationMs, packet.DayDurationMins));
    }

    private void OnEntitySetTarget(byte[] data) {
        EntitySetTargetPacket packet = new EntitySetTargetPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.UpdateEntityTarget(packet.EntityId, packet.TargetId));
    }

    private void OnEntityAutoAttackStart(byte[] data) {
        Debug.Log("OnEntityAutoAttackStart");
        AutoAttackStartPacket packet = new AutoAttackStartPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.EntityStartAutoAttacking(packet.EntityId));
    }

    private void OnEntityAutoAttackStop(byte[] data) {
        Debug.Log("OnEntityAutoAttackStop");
        AutoAttackStopPacket packet = new AutoAttackStopPacket(data);
        _eventProcessor.QueueEvent(() => World.Instance.EntityStopAutoAttacking(packet.EntityId));
    }

    private void OnActionFailed(byte[] data) {
        ActionFailedPacket packet = new ActionFailedPacket(data);
        _eventProcessor.QueueEvent(() => PlayerEntity.Instance.OnActionFailed(packet.PlayerAction));
    }
}
