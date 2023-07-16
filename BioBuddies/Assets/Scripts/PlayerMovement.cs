using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] float MoveSpeed = 10;
    [SerializeField] float Jump = 5;
    private bool isMoving = false;
    private Camera mainCamera;       //degi?en
    private Vector3 moveDirection;   // degi?en
    public float boostsecond = 4.0f;
    public bool hizlandi = false;
    public bool geldi = false;
    //spawn icin
    private Vector3 initialPosition;  // ?lk pozisyon
    private bool isReturning;  // Geri d?nme durumu
    //---------------------------2.BOLUM-------------------------------------
    public int puan = 0;
    public int trashes = 31;
    public bool magnet = false;
    public float magnetsecond = 3.0f;
    //-------------------------3.BOLUM-------------------------------------
    public bool metal = false;
    public bool plastic = false;
    public bool glass = false;
    public bool kagit = false;
    public bool empty = false;
    public bool dolu = false;
   


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main; //degisen
        //spawn
        initialPosition = transform.position;  // ?lk pozisyonu kaydet
        Debug.Log("ilk pozisyon tmm");
        //-----------------------------2.BOLUM-------------------------------
    }


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Kamera y?n?ne g?re hareket vekt?r?n? hesapla
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        moveDirection = (vertical * cameraForward + horizontal * mainCamera.transform.right).normalized;

        if (moveDirection.magnitude > 0)
        {
            isMoving = true;
            transform.Translate(moveDirection * MoveSpeed * Time.deltaTime, Space.World);

            // Karakterin y?n?n? kamera y?n?ne hizala
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                // Hareket durdu?unda ek i?lemler yapabilirsiniz
            }
        }

        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0))
            rb.velocity = new Vector3(rb.velocity.x, Jump, rb.velocity.z);

        //transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);

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

        // --------------------------------2.BOLUM------------------------------

        if (magnet == true)
        {
            magnetsecond -= Time.deltaTime;           
            // 5 saniyeli?ine artacak           
            GetComponent<CapsuleCollider>().enabled = false; //normal collider devre d???           
            GetComponent<SphereCollider>().enabled = true;    //magnet collider etkin                                                             
            if (magnetsecond <= 0f)
            {
                magnet = false;
                GetComponent<CapsuleCollider>().enabled = true;  //normal collider etkin
                GetComponent<SphereCollider>().enabled = false;    // magnet collider etkin de?il            
                magnetsecond = 3.0f;   // yine 3 saniyeli?ine e?itledik, s?f?rda kalmas?n
            }
        } 
        
        //---------------------------------------------------3.BOLUM----------------------------------------------

        if (metal == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("Metal al?nd?");
            empty = true;
        }

        if (plastic == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("plastik al?nd?");
            empty = true;
        }

        if (glass == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("Cam al?nd?");
            empty = true;
        }

        if (kagit == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("Kagit al?nd?");
            empty = true;
        }


        if (empty == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                
                Debug.Log("B?eakt?n");
                sifirla();
            }
        }        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Engel")
        {
            MoveSpeed = 2;
        }


       if(other.tag == "Collect")
        {
            hizlandi = true;   
        }

        //spawn i?in
        if (other.tag == "buz")
        {
            // Buz tag'li objeyegeldi?inde konum kaydet
            initialPosition = transform.position;           
        }
        else if (other.CompareTag("deniz") && !isReturning)
        {
            // denize de?ersen geri d?n
            isReturning = true;           
            StartCoroutine(ReturnToInitialPosition());
        }

        if (other.tag == "pengus")
        {
            //4 saniye animasyon
            SceneManager.LoadScene("scene2");
        }

        // ---------------------------------------2.BOLUM---------------------------------

        if (other.tag == "trash")
        {
            if(magnet == true)
            {
                StartCoroutine(FollowPlayer(other.gameObject));               
            }
            else if(magnet == false)
            {
                Destroy(other.gameObject);
                puan++;
                Debug.Log(puan);
                trashes--;
                Debug.Log(trashes);
            }                                                            
        }

        if (other.tag == "magnet")
        {
            magnet = true;
        }

        // ----------------------------- 3. BOLUM -------------------------------------------

        if (other.tag == "organik")
        {           
            metal = true;
        }

        if (other.tag == "plastik")
        {
            plastic = true;
        }

        if (other.tag == "cam")
        {
            glass = true;
        }

        if (other.tag == "kagit")
        {
            kagit = true;
        }
        
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Engel")
        {
            MoveSpeed = 10;
        }

        //------------------------------------------3.BOLUM---------------------------------
        
        if (empty == true && metal == true && plastic == false && glass == false)
        {
            organictut();
            StartCoroutine(YapisPlayer(other.gameObject));
        }
        if (empty == true && metal == false && plastic == true && glass == false)
        {
            StartCoroutine(YapisPlayer(other.gameObject));
        }
        if (empty == true && metal == false && plastic == false && glass == true)
        {
            StartCoroutine(YapisPlayer(other.gameObject));
        }       
    }

    public void organictut()
    {
        metal = false;
        plastic = false;
        glass = false;
        empty = false;
    }

  

    IEnumerator FollowPlayer(GameObject trashObject)
    {
        float followSpeed = 2f; // Takip h?z?
        float minDistance = 1f; // Min takip mesafesi

        Rigidbody trashRigidbody = trashObject.GetComponent<Rigidbody>();
        trashRigidbody.isKinematic = false;

        while (trashObject != null) // Objeyi kontrol et
        {
            if (!trashObject.activeInHierarchy) 
                break;

            Vector3 playerPosition = transform.position;
            Vector3 targetPosition = playerPosition + (trashObject.transform.position - playerPosition).normalized * minDistance;

            // Hedefe do?ru hareket lerp
            trashObject.transform.position = Vector3.Lerp(trashObject.transform.position, targetPosition, Time.deltaTime * followSpeed);

            // player merkeze geldi?inde kontrol et
            if (Vector3.Distance(trashObject.transform.position, playerPosition) < 0.1f)
            {
                Destroy(trashObject);
                puan += 1;
                Debug.Log(puan);
                trashes -= 1;
                Debug.Log(trashes);
                break;
            }

            yield return null;
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

    //---------------------------3.BOLUM------------------------------------

    private void sifirla()
    {
        metal = false;
        plastic = false;
        glass = false;
        kagit = false;
        empty = false;
    }


    IEnumerator YapisPlayer(GameObject trashObject)
    {
        float followSpeed = 99f; // Takip h?z?
        float minDistance = 4f; // Min takip mesafesi

        Rigidbody trashRigidbody = trashObject.GetComponent<Rigidbody>();
        trashRigidbody.isKinematic = false;

        while (trashObject != null) // Objeyi kontrol et
        {
            if (!trashObject.activeInHierarchy)
                break;

            Vector3 playerPosition = transform.position;
            Vector3 targetPosition = playerPosition + (trashObject.transform.position - playerPosition).normalized * minDistance;

            // Hedefe do?ru hareket lerp
            trashObject.transform.position = Vector3.Lerp(trashObject.transform.position, targetPosition, Time.deltaTime * followSpeed);

           

            yield return null;
        }
    }



}
