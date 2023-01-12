using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class UnityWebRequestHandler
{
    public static readonly string baseURL = "https://restaurant-backend.onerare.io/";
    public static readonly string topWinnersGET = "getTopWinners";
    public static readonly string addWinnerPOST = "addWinner";
    public static readonly string redeemPOST = "redeem";
    public static readonly string withdrawPOST = "withdraw";
    public static readonly string myNFTsGET = "getAllCoupons";
    public static readonly string couponDetailsGET = "getCouponDetails";



    public static IEnumerator AddWinnerRequest(UserData _userData, Action<string> _callback)
    {
        string _url = baseURL + addWinnerPOST;
        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new NFTClasses.WinnindRequestBody
        {
            dishId = 0,
            userAddress = _userData.userDataServer.uid,
            userEmail = _userData.userDataServer.email,
            time = int.Parse(_userData.userDataServer.time)
        });
        Debug.Log($"URL -> {_url} | Data -> {_postData}");
        UnityWebRequest req = UnityWebRequest.Post(_url, _postData);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Request RESPONSE: " + req.downloadHandler.text);
            _callback?.Invoke(req.downloadHandler.text);
        }
    }


    public static IEnumerator GetTopWinnersRequest(string _restaurantName, Action<string> _callback)
    {
        string _url = baseURL + topWinnersGET;

        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new NFTClasses.TopWinnersRequestBody
        {
            restaurantName = _restaurantName
        });
        Debug.Log($"URL -> {_url} | Data -> {_postData}");

        UnityWebRequest req = UnityWebRequest.Get(_url);
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("accept", "text/plain");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(_postData));
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Request RESPONSE: " + req.downloadHandler.text);
            _callback?.Invoke(req.downloadHandler.text);
        }

    }


    public static IEnumerator RedeemRequest(string _dishName, string _walletAddress, Action<string> _callback)
    {
        string _url = baseURL + redeemPOST;
        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new  NFTClasses.RedeemRequestBody
        {
            dishName = _dishName,
            userAddress = _walletAddress
        });

        Debug.Log($"URL -> {_url} | Data -> {_postData}");
        UnityWebRequest req = UnityWebRequest.Post(_url, _postData);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Request RESPONSE: " + req.downloadHandler.text);
            _callback?.Invoke(req.downloadHandler.text);
        }
    }


    public static IEnumerator WithdrawRequest(string _dishName, string _walletAddress, Action<string> _callback)
    {
        string _url = baseURL + withdrawPOST;
        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new NFTClasses.WithdrawRequestBody
        {
            dishName = _dishName,
            userAddress = _walletAddress,
            toAddress = _walletAddress
        });

        Debug.Log($"URL -> {_url} | Data -> {_postData}");
        UnityWebRequest req = UnityWebRequest.Post(_url, _postData);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Request RESPONSE: " + req.downloadHandler.text);
            _callback?.Invoke(req.downloadHandler.text);
        }
    }


    public static IEnumerator GetMyNFTs(string _walletAddress, Action<string> _callback)
    {
        string _url = baseURL + myNFTsGET + $"/{_walletAddress}";
        Debug.Log($"URL -> {_url}");

        UnityWebRequest req = UnityWebRequest.Get(_url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Request RESPONSE: " + req.downloadHandler.text);
            _callback?.Invoke(req.downloadHandler.text);
        }
    }


    public static IEnumerator GetCouponDetails(int _dishId, Action<string> _callback)
    {
        string _url = baseURL + couponDetailsGET + $"/{_dishId}";
        Debug.Log($"URL -> {_url}");

        UnityWebRequest req = UnityWebRequest.Get(_url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Request RESPONSE: " + req.downloadHandler.text);
            _callback?.Invoke(req.downloadHandler.text);
        }
    }

    public static IEnumerator GetTexture(string _url, Action<Texture2D> _callback)
    {
        Debug.Log($"URL -> {_url}");
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(_url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Request ERROR: " + req.error);
            _callback?.Invoke(null);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)req.downloadHandler).texture;
            _callback?.Invoke(myTexture);
        }
    }


}





