using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takip : MonoBehaviour
{
    [SerializeField] Transform Player;

    void Start()
    {
        
    }


    void Update()
    {
        transform.position = Player.position;
    }
}
