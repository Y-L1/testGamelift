using System;
using System.Collections.Generic;
using Amazon;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;


public class GameLiftClient : MonoBehaviour
{
    public Button Create1, Create2;
    
    public string ProfileName = "Default";
    
    private Amazon.GameLift.Model.PlayerSession psession = null;
    public AmazonGameLiftClient aglc = null;

    public void CreateGameLiftClient()
    {
        //Access Amazon GameLift service by setting up a configuration. 
        //The default configuration specifies a location. 
        var config = new AmazonGameLiftConfig();
        config.RegionEndpoint = Amazon.RegionEndpoint.USEast2;
   
        CredentialProfile profile = null;
        var nscf = new SharedCredentialsFile();
        
        nscf.TryGetProfile(ProfileName, out profile);
        Debug.Log("Getting profile: " + ProfileName);
        
        AWSCredentials credentials = profile.GetAWSCredentials(null);
        Debug.Log("Creating client.");
        
        //Initialize GameLift Client with default client configuration.
        aglc = new AmazonGameLiftClient(credentials, config);
        
    }
    
    public void Initialize(RegionEndpoint endPoint, Action<string> onError = null)
    {
        if (aglc != null)
        {
            Debug.Log("GameLift has already initialized.");
            onError?.Invoke("Initialized");
            return;
        }

        Debug.Log("Initializing GameLift.");
            
        // Access Amazon GameLift service by setting up a configuration. 
        // The default configuration specifies a location. 
        var config = new AmazonGameLiftConfig
        {
            RegionEndpoint = endPoint
        };

        try
        {
            Debug.Log("Getting profile: " + ProfileName);
            var sharedCredentialsFile = new SharedCredentialsFile();
            sharedCredentialsFile.TryGetProfile(ProfileName, out var credProfile);
            var credentials = credProfile.GetAWSCredentials(null);
            
            AWSCredentials credentials2 = new BasicAWSCredentials("AKIAW2JEMCHBXVNPDRS5", "N5fTNuPtcp7YG88w65oZ5HVVVRSsr4fI1nmqu+BM");

            
            Debug.Log("Creating client.");
            //Initialize GameLift Client with default client configuration.
            aglc = new AmazonGameLiftClient(credentials2, config);
        }
        catch (Exception exception)
        {
            Debug.Log("Initializing GameLift failed, error: " + exception.Message);
            onError?.Invoke(exception.Message);
        }
            
        Debug.Log("GameLift initialized.");
        onError?.Invoke("");
    }

    private void Awake()
    {
        Create1.onClick.AddListener(() =>
        {
            Initialize(RegionEndpoint.USEast2);
        });
        Create2.onClick.AddListener(CreateGameLiftClient);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Initialize(RegionEndpoint.USEast2);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            CreateGameLiftClient();
        }
        
    }
}
