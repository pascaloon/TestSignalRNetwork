using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;


public class Network : MonoBehaviour
{
    private HubConnection _connection;
    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }
    async void Connect()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:44344/ChatHub", options =>
            {
                options.AccessTokenProvider = async () => "my-unity-token";
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
            .Build();
        
        _connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Debug.Log($"{user}: {message}");
        });

        await _connection.StartAsync();
    }
    
    

    private void OnDestroy()
    {
        Disconnect();
    }

    async void Disconnect()
    {
        if (_connection.State == HubConnectionState.Connected || _connection.State == HubConnectionState.Connecting)
        {
            await _connection.StopAsync();
        }
    }

    
}
