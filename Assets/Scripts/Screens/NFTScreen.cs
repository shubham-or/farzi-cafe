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

    [SerializeField] private GameObject popupsGo;
    [Header("---Redeem---")]
    [SerializeField] private GameObject redeemCongratsGo;
    [SerializeField] private TMP_InputField redeemCodeInput;

    [Header("---Withdraw---")]
    [SerializeField] private GameObject withdrawGO;
    [SerializeField] private GameObject processingGO;
    [SerializeField] private GameObject congratsGO;
    [SerializeField] private GameObject failedGO;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private TextMeshProUGUI error;

    public static NFTScreen Instance;
    private void Awake() => Instance = this;

    void Start()
    {
        addressInput.interactable = false;
    }

    private void OnEnable()
    {
        ResetValues();
        StartCoroutine(GetMyNFTs());
    }

    private IEnumerator GetMyNFTs()
    {
        yield return UnityWebRequestHandler.GetMyNFTs(GameManager.Instance.GetUserUID(), _response =>
        {
            APIDataClasses.NFTResponse _data = Newtonsoft.Json.JsonConvert.DeserializeObject<APIDataClasses.NFTResponse>(_response);
            if (_data.data.Count > 0 && UnityWebRequestHandler.IsSuccess(_data.status))
            {
                warningNoNFT.gameObject.SetActive(false);
                foreach (var winner in _data.data)
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

    public void ResetValues()
    {
        popupsGo.SetActive(false);
        redeemCongratsGo.SetActive(false);
        withdrawGO.SetActive(true);
        processingGO.SetActive(false);
        congratsGO.SetActive(false);
        failedGO.SetActive(false);
    }

    public void CopyToClipboard() => GUIUtility.systemCopyBuffer = address.text.Trim();




    #region Redeem and Withdraw
    public APIDataClasses.NFTResponse.Result selectedNFTItem;
    Action<bool> _withdrawCallback;
    Action<bool, string> _redeemCallback;
    public void ShowRedeemCode(string _code)
    {
        withdrawGO.SetActive(false);
        processingGO.SetActive(false);
        failedGO.SetActive(false);

        popupsGo.SetActive(true);
        redeemCongratsGo.SetActive(true);
        redeemCodeInput.text = _code;
    }
    public void ShowRedeem(APIDataClasses.NFTResponse.Result _NFT, Action<bool, string> _callback)
    {
        selectedNFTItem = _NFT;
        _redeemCallback = _callback;

        withdrawGO.SetActive(false);
        processingGO.SetActive(false);
        failedGO.SetActive(false);

        popupsGo.SetActive(true);
        processingGO.SetActive(true);
        StartCoroutine(RedeemNFT(_NFT.dishName, GameManager.Instance.GetUserUID(), _redeemCallback));
    }


    private IEnumerator RedeemNFT(string _dishName, string _walletAddress, Action<bool, string> _callback)
    {
        yield return StartCoroutine(UnityWebRequestHandler.RedeemRequest(_dishName, _walletAddress, _response =>
        {
            processingGO.SetActive(false);
            APIDataClasses.GenericResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<APIDataClasses.GenericResponse>(_response);

            if (UnityWebRequestHandler.IsSuccess(response.status))
            {
                redeemCodeInput.text = response.data;
                redeemCongratsGo.SetActive(true);
                _callback?.Invoke(true, response.data);
            }
            else
            {
                failedGO.SetActive(true);
                _callback?.Invoke(false, null);
            }

            selectedNFTItem = null;
            _redeemCallback = null;
        }));
    }





    public void ShowWithdraw(APIDataClasses.NFTResponse.Result _NFT, Action<bool> _callback)
    {
        selectedNFTItem = _NFT;
        _withdrawCallback = _callback;
        processingGO.SetActive(false);
        failedGO.SetActive(false);
        redeemCongratsGo.SetActive(false);

        popupsGo.SetActive(true);
        withdrawGO.SetActive(true);
        addressInput.text = GameManager.Instance.GetUserUID();
    }


    public void OnClick_Withdraw() => Withdraw(selectedNFTItem, _withdrawCallback);

    private void Withdraw(APIDataClasses.NFTResponse.Result _NFT, Action<bool> _callback)
    {
        withdrawGO.SetActive(false);
        processingGO.SetActive(true);
        StartCoroutine(WithdrawNFT(_NFT.dishName, GameManager.Instance.GetUserUID(), _callback));
    }

    private IEnumerator WithdrawNFT(string _dishName, string _walletAddress, Action<bool> _callback)
    {
        yield return StartCoroutine(UnityWebRequestHandler.WithdrawRequest(_dishName, _walletAddress, _response =>
        {
            processingGO.SetActive(false);
            APIDataClasses.GenericResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<APIDataClasses.GenericResponse>(_response);

            if (UnityWebRequestHandler.IsSuccess(response.status))
            {
                congratsGO.SetActive(true);
                _callback?.Invoke(true);
            }
            else
            {
                failedGO.SetActive(true);
                _callback?.Invoke(false);
            }

            selectedNFTItem = null;
            _withdrawCallback = null;
        }));
    }
    #endregion
}



