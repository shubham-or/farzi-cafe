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
        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new APIDataClasses.WinnindRequestBody
        {
            restaurantName = GameManager.restaurantName,
            dishName = GameManager.ReplaceSpaceWithUnderscore(_userData.dishData.Dish_Name),
            userAddress = _userData.userDataServer.uid,
            userEmail = _userData.userDataServer.email,
            time = int.Parse(_userData.userDataServer.time)
        });
        Debug.Log($"AddWinnerRequest URL -> {_url} | Data -> {_postData}");
        UnityWebRequest req = UnityWebRequest.Put(_url, _postData);
        req.method = "POST";
        req.SetRequestHeader("Content-Type", "application/json");
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

        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new APIDataClasses.TopWinnersRequestBody
        {
            restaurantName = _restaurantName
        });
        Debug.Log($"GetTopWinnersRequest URL -> {_url} | Data -> {_postData}");

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
        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new APIDataClasses.RedeemRequestBody
        {
            dishName = _dishName,
            userAddress = _walletAddress
        });

        Debug.Log($"RedeemRequest URL -> {_url} | Data -> {_postData}");
        UnityWebRequest req = UnityWebRequest.Put(_url, _postData);
        req.method = "POST";
        req.SetRequestHeader("Content-Type", "application/json");
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
        string _postData = Newtonsoft.Json.JsonConvert.SerializeObject(new APIDataClasses.WithdrawRequestBody
        {
            dishName = _dishName,
            userAddress = _walletAddress,
            toAddress = _walletAddress
        });

        Debug.Log($"WithdrawRequest URL -> {_url} | Data -> {_postData}");
        UnityWebRequest req = UnityWebRequest.Put(_url, _postData);
        req.method = "POST";
        req.SetRequestHeader("Content-Type", "application/json");
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
        Debug.Log($"GetMyNFTs URL -> {_url}");

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
        Debug.Log($"GetCouponDetails URL -> {_url}");

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
        Debug.Log($"GetTexture URL -> {_url}");
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
            Debug.Log("Texture name: " + myTexture.name);
            _callback?.Invoke(myTexture);
        }
    }


    public static bool IsSuccess(string _status) => _status.Trim().ToLower().Equals("success");

}





