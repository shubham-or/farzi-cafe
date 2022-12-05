using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseLoginResponse
{
    public User user;
    public Credential credential;
    public AdditionalUserInfo additionalUserInfo;
    public string operationType;

    public class AdditionalUserInfo
    {
        public string providerId;
        public bool isNewUser;
        public Profile profile;
    }

    public class Credential
    {
        public string providerId;
        public string signInMethod;
        public string oauthIdToken;
        public string oauthAccessToken;
    }

    public class MultiFactor
    {
        public List<object> enrolledFactors;
    }

    public class Profile
    {
        public string name;
        public string granted_scopes;
        public string id;
        public bool verified_email;
        public string given_name;
        public string locale;
        public string hd;
        public string family_name;
        public string email;
        public string picture;
    }

    public class ProviderDatum
    {
        public string uid;
        public string displayName;
        public string photoURL;
        public string email;
        public object phoneNumber;
        public string providerId;
    }

    public class StsTokenManager
    {
        public string apiKey;
        public string refreshToken;
        public string accessToken;
        public long expirationTime;
    }

    public class User
    {
        public string uid;
        public string displayName;
        public string photoURL;
        public string email;
        public bool emailVerified;
        public object phoneNumber;
        public bool isAnonymous;
        public object tenantId;
        public List<ProviderDatum> providerData;
        public string apiKey;
        public string appName;
        public string authDomain;
        public StsTokenManager stsTokenManager;
        public object redirectEventId;
        public string lastLoginAt;
        public string createdAt;
        public MultiFactor multiFactor;
    }



}
