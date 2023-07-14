using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        if(IsOwner && IsServer)
        {
            transform.RotateAround(GetComponentInParent<Transform>().position, Vector3.up, 100 * Time.deltaTime);
        }  // playera eklenen bir chil ın senkronize hareketi için // sadece playerda network obj yeterli
    }
}
