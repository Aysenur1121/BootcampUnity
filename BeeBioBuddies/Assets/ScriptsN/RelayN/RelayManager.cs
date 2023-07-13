using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    // Start is called before the first frame update
    private string PlayerID; // bize atanacak olan id
    public TextMeshPro IdText;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialize");
        SignIn();
    }

    async void SignIn()
    {
        Debug.Log("Signing in");
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        PlayerID = AuthenticationService.Instance.PlayerId;
        Debug.Log("Signed in: " + PlayerID);
        IdText.text = PlayerID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
