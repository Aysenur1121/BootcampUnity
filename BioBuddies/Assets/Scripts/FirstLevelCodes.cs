using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLevelCodes : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float MoveSpeed = 10;
    [SerializeField] float Jump = 5;
    public float boostsecond = 4.0f;
    public bool hizlandi = false;
    public bool geldi = false;
    //spawn icin
    private Vector3 initialPosition;  // Ýlk pozisyon
    private bool isReturning;  // Geri dönme durumu


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //spawn
        initialPosition = transform.position;  // Ýlk pozisyonu kaydet
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
        }

        //spawn için
        if (other.tag == "buz")
        {
            // Buz tag'li objeyegeldiðinde konum kaydet
            initialPosition = transform.position;
        }
        else if (other.CompareTag("deniz") && !isReturning)
        {
            // denize deðersen geri dön
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

    //spawn için
    private System.Collections.IEnumerator ReturnToInitialPosition()
    {
        float duration = 0f;  // Geri dönme süresi
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            // Pozisyonu kaydedilen interpolazyon 
            transform.position = Vector3.Lerp(startPosition, initialPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;  // Son konumu güncelle
        isReturning = false;  // Geri dönme durumunu sýfýrla

    }
}
