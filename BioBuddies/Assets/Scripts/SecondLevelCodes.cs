using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLevelCodes : MonoBehaviour
{
    public int puan = 0;
    public int trashes = 31;
    public bool magnet = false;
    public float magnetsecond = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "trash")
        {
            if (magnet == true)
            {
                StartCoroutine(FollowPlayer(other.gameObject));
            }
            else if (magnet == false)
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
}
