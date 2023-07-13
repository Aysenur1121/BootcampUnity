using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FirstLevelCodes : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] float MoveSpeed = 10;
    [SerializeField] float Jump = 5;
    public float boostsecond = 4.0f;
    public bool hizlandi = false;
    public bool geldi = false;
    //spawn icin
    private Vector3 initialPosition;  // ?lk pozisyon
    private bool isReturning;  // Geri d?nme durumu


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //spawn
        initialPosition = transform.position;  // ?lk pozisyonu kaydet
        Debug.Log("ilk pozisyon tmm");
    }

    // Update is called once per frame
    void Update()
    {
        float horInput = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        float verInput = Input.GetAxisRaw("Vertical") * MoveSpeed;

        rb.velocity = new Vector3(horInput, rb.velocity.y, verInput);

        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0))
            rb.velocity = new Vector3(rb.velocity.x, Jump, rb.velocity.z);

        transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
    }
    private void FixedUpdate()
    {
        if (hizlandi == true)
        {
            MoveSpeed = 20;
            boostsecond -= Time.deltaTime;
            if (boostsecond <= 0f)
            {
                hizlandi = false;
                MoveSpeed = 10;
                boostsecond = 4.0f;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Engel")
        {
            MoveSpeed = 2;
        }


        if (other.tag == "Collect")
        {
            hizlandi = true;
            Destroy(other.gameObject);
        }

        //spawn i?in
        if (other.tag == "buz")
        {
            // Buz tag'li objeyegeldi?inde konum kaydet
            initialPosition = transform.position;
        }
        else if (other.CompareTag("deniz") && !isReturning)
        {
            Debug.Log("Öldü mü?");
            // denize de?ersen geri d?n
            isReturning = true;
            StartCoroutine(ReturnToInitialPosition());
            
        }

        if (other.tag == "pengus")
        {
            //4 saniye animasyon
            SceneManager.LoadScene("scene2");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Engel")
        {
            MoveSpeed = 10;
        }
    }

    //spawn i?in
    private System.Collections.IEnumerator ReturnToInitialPosition()
    {
        float duration = 0f;  // Geri d?nme s?resi
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            // Pozisyonu kaydedilen interpolazyon 
            transform.position = Vector3.Lerp(startPosition, initialPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;  // Son konumu g?ncelle
        isReturning = false;  // Geri d?nme durumunu s?f?rla

    }
}
