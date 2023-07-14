using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

   

    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) // Move komutunun bütün playerlar üzerinde çalışmaması için
        {
            Move(); 
        }
    }

    void Update()
    {
        transform.position = Position.Value;
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        else  // server değilsek çalışacak
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams  rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(x: Random.Range(-3f, 3f), y: 1f, z: Random.Range(-3f, 3f));
    }
}
