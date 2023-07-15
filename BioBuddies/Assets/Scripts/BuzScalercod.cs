using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzScalercod : MonoBehaviour
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

        // Box collider'ýn boyutlarýný güncelle
        Vector3 colliderSize = boxCollider.size;
        colliderSize.x *= scaleAmount;
        colliderSize.z *= scaleAmount;
        boxCollider.size = colliderSize;

        // Nesnenin yeri sabit kalsýn
        transform.position = initialPosition;
    }
}
