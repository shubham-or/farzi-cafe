using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NFTScreen : MonoBehaviour
{
    [SerializeField] private GameObject nftItemPrefab;
    [SerializeField] private Transform content;

    [SerializeField] private TextMeshProUGUI address;
    [SerializeField] private TextMeshProUGUI warningNoNFT;

    [Header("---Withdraw---")]
    [SerializeField] private GameObject withdrawGo;
    [SerializeField] private GameObject inputsGO;
    [SerializeField] private GameObject processingGO;
    [SerializeField] private GameObject congratsGO;
    [SerializeField] private GameObject failedGO;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private TextMeshProUGUI error;

    public const string restaurantName = "FARZI_CAFE";
    public static string walletAddress = "0x6e2a98e14961c9619768d794b636cad486688754";

    public static NFTScreen Instance;
    private void Awake() => Instance = this;

    void Start()
    {
        StartCoroutine(GetMyNFTs());
    }

    public void SetDishName(string _dishName) => _dishName.Replace(' ', '_').Trim();

    private IEnumerator GetMyNFTs()
    {
        yield return UnityWebRequestHandler.GetMyNFTs(walletAddress, _response =>
        {
            NFTClasses.NFTResponse _data = Newtonsoft.Json.JsonConvert.DeserializeObject<NFTClasses.NFTResponse>(_response);
            if (_data.result.Count > 0)
            {
                warningNoNFT.gameObject.SetActive(false);
                foreach (var winner in _data.result)
                {
                    Instantiate(nftItemPrefab, content).GetComponent<NFTItem>().SetDetails(winner);
                }
            }
            else
                warningNoNFT.gameObject.SetActive(true);
        });
    }


    public void OnClick_Close(GameObject _go)
    {
        _go.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = address.text.Trim();
    }




    #region Withdraw
    public void Redeem(NFTClasses.NFTResponse.Result _NFT, Action<bool> _callback)
    {

        StartCoroutine(RedeemNFT(_NFT.dishName, walletAddress, _callback));
    }

    private IEnumerator RedeemNFT(string _dishName, string _walletAddress, Action<bool> _callback)
    {
        yield return StartCoroutine(UnityWebRequestHandler.RedeemRequest(_dishName, _walletAddress, _response =>
        {

        }));
    }


    public NFTClasses.NFTResponse.Result selectedNFTItem;
    Action<bool> _nftCallback;
    public void ShowWithdraw(NFTClasses.NFTResponse.Result _NFT, Action<bool> _callback)
    {
        selectedNFTItem = _NFT;
        withdrawGo.SetActive(true);
        inputsGO.SetActive(true);
    }

    public void Withdraw(NFTClasses.NFTResponse.Result _NFT, Action<bool> _callback)
    {
        StartCoroutine(WithdrawNFT(_NFT.dishName, walletAddress, _nftCallback));
    }

    private IEnumerator WithdrawNFT(string _dishName, string _walletAddress, Action<bool> _callback)
    {
        yield return StartCoroutine(UnityWebRequestHandler.WithdrawRequest(_dishName, _walletAddress, _response =>
        {

        }));
    }
    #endregion
}



