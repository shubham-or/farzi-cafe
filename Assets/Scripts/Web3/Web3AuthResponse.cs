using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Web3AuthResponse
{
    [System.Serializable]
    public class LoginResponse
    {
        public bool status;
        public string address =  GameManager.Instance.GetUserData().userDataServer.email.Replace('@', '_').Replace('.','_');
        public Data data;


        [System.Serializable]
        public class Data
        {
            public string email;
            public string name;
            public string profileImage;
            public string aggregateVerifier;
            public string verifier;
            public string verifierId;
            public string typeOfLogin;
            public string dappShare;
            public string idToken;
            public string oAuthIdToken;
            public string oAuthAccessToken;


        }
    }

}
