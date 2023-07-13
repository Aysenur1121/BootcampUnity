using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuzScalercod : NetworkBehaviour
{
    private BoxCollider boxCollider;
    private Vector3 initialPosition;
    private float timer = 0f;
    [SerializeField] float interval = 3f;
    [SerializeField] float scaleAmount = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            ShrinkObject();
        }

    }

    private void ShrinkObject()
    {
        Vector3 scale = transform.localScale;
        scale.x *= scaleAmount;
        scale.z *= scaleAmount;
        transform.localScale = scale;

        // Box collider'?n boyutlar?n? g?ncelle
        Vector3 colliderSize = boxCollider.size;
        colliderSize.x *= scaleAmount;
        colliderSize.z *= scaleAmount;
        boxCollider.size = colliderSize;

        // Nesnenin yeri sabit kals?n
        transform.position = initialPosition;
    }
}
