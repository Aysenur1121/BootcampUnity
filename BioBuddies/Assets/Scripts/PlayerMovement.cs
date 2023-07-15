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
    private Camera mainCamera;       //degiþen
    private Vector3 moveDirection;   // degiþen
    public float boostsecond = 4.0f;
    public bool hizlandi = false;
    public bool geldi = false;
    //spawn icin
    private Vector3 initialPosition;  // Ýlk pozisyon
    private bool isReturning;  // Geri dönme durumu
    //---------------------------2.BOLUM-------------------------------------
    public int puan = 0;
    public int trashes = 31;
    public bool magnet = false;
    public float magnetsecond = 3.0f;
    //-------------------------3.BOLUM-------------------------------------
    public bool organic = false;
    public bool plastic = false;
    public bool glass = false;
    public bool empty = false;
    public bool dolu = false;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main; //degisen
        //spawn
        initialPosition = transform.position;  // Ýlk pozisyonu kaydet
        Debug.Log("ilk pozisyon tmm");
        //-----------------------------2.BOLUM-------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Kamera yönüne göre hareket vektörünü hesapla
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        moveDirection = (vertical * cameraForward + horizontal * mainCamera.transform.right).normalized;

        if (moveDirection.magnitude > 0)
        {
            isMoving = true;
            transform.Translate(moveDirection * MoveSpeed * Time.deltaTime, Space.World);

            // Karakterin yönünü kamera yönüne hizala
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                // Hareket durduðunda ek iþlemler yapabilirsiniz
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
            // 5 saniyeliðine artacak           
            GetComponent<CapsuleCollider>().enabled = false; //normal collider devre dýþý           
            GetComponent<SphereCollider>().enabled = true;    //magnet collider etkin                                                             
            if (magnetsecond <= 0f)
            {
                magnet = false;
                GetComponent<CapsuleCollider>().enabled = true;  //normal collider etkin
                GetComponent<SphereCollider>().enabled = false;    // magnet collider etkin deðil            
                magnetsecond = 3.0f;   // yine 3 saniyeliðine eþitledik, sýfýrda kalmasýn
            }
        } 
        
        //---------------------------------------------------3.BOLUM----------------------------------------------

        if (organic == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("Organik alýndý");
            empty = true;
        }

        if (plastic == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("plastik alýndý");
            empty = true;
        }

        if (glass == true && Input.GetKeyDown(KeyCode.E) && empty == false)
        {
            Debug.Log("Cam alýndý");
            empty = true;
        }

        if (empty == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                
                Debug.Log("Býeaktýn");
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
            organic = true;
        }

        if (other.tag == "plastik")
        {
            plastic = true;
        }

        if (other.tag == "cam")
        {
            glass = true;
        }  
        
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Engel")
        {
            MoveSpeed = 10;
        }

        //------------------------------------------3.BOLUM---------------------------------
        
        if (empty == true && organic == true && plastic == false && glass == false)
        {
            organictut();
            StartCoroutine(YapisPlayer(other.gameObject));
        }
        if (empty == true && organic == false && plastic == true && glass == false)
        {
            StartCoroutine(YapisPlayer(other.gameObject));
        }
        if (empty == true && organic == false && plastic == false && glass == true)
        {
            StartCoroutine(YapisPlayer(other.gameObject));
        }       
    }

    public void organictut()
    {
        organic = false;
        plastic = false;
        glass = false;
        empty = false;
    }

    

    IEnumerator FollowPlayer(GameObject trashObject)
    {
        float followSpeed = 2f; // Takip hýzý
        float minDistance = 1f; // Min takip mesafesi

        Rigidbody trashRigidbody = trashObject.GetComponent<Rigidbody>();
        trashRigidbody.isKinematic = false;

        while (trashObject != null) // Objeyi kontrol et
        {
            if (!trashObject.activeInHierarchy) 
                break;

            Vector3 playerPosition = transform.position;
            Vector3 targetPosition = playerPosition + (trashObject.transform.position - playerPosition).normalized * minDistance;

            // Hedefe doðru hareket lerp
            trashObject.transform.position = Vector3.Lerp(trashObject.transform.position, targetPosition, Time.deltaTime * followSpeed);

            // player merkeze geldiðinde kontrol et
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

    //---------------------------3.BOLUM------------------------------------

    private void sifirla()
    {
        organic = false;
        plastic = false;
        glass = false;
        empty = false;
    }


    IEnumerator YapisPlayer(GameObject trashObject)
    {
        float followSpeed = 99f; // Takip hýzý
        float minDistance = 4f; // Min takip mesafesi

        Rigidbody trashRigidbody = trashObject.GetComponent<Rigidbody>();
        trashRigidbody.isKinematic = false;

        while (trashObject != null) // Objeyi kontrol et
        {
            if (!trashObject.activeInHierarchy)
                break;

            Vector3 playerPosition = transform.position;
            Vector3 targetPosition = playerPosition + (trashObject.transform.position - playerPosition).normalized * minDistance;

            // Hedefe doðru hareket lerp
            trashObject.transform.position = Vector3.Lerp(trashObject.transform.position, targetPosition, Time.deltaTime * followSpeed);

           

            yield return null;
        }
    }



}
