using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTItem : MonoBehaviour
{
    public NFTClasses.NFTResponse.Result nft;

    [SerializeField] private Button redeem;
    [SerializeField] private Button withdraw;

    [SerializeField] private RawImage dishImage;
    [SerializeField] private TextMeshProUGUI nftname;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI timing;



    void Start()
    {

    }

    public void SetDetails(NFTClasses.NFTResponse.Result _data)
    {
        nft = _data;
        SetDishImageURL(5, _url => StartCoroutine(SetNFTImage(_url)));

        nftname.text = _data.dishName.Replace('_', ' ').Trim();
        rank.text = _data.rank.ToString();
        timing.text = Timer.GetTimeInMinAndSec(_data.bestTime);

        switch (_data.status.Trim().ToUpper())
        {
            case "AVAILABLE":
                redeem.GetComponentInChildren<TextMeshProUGUI>().text = "Redeem";
                withdraw.gameObject.SetActive(true);
                break;
            case "PENDING":
                redeem.GetComponentInChildren<TextMeshProUGUI>().text = "Pending...";
                redeem.interactable = false;
                withdraw.gameObject.SetActive(false);
                break;
            case "REDEEMED":
                redeem.GetComponentInChildren<TextMeshProUGUI>().text = $"Redeemed    {_data.redeemedDate}";
                redeem.interactable = false;
                withdraw.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }


    private IEnumerator SetDishImageURL(int _id, Action<string> _callback)
    {
        yield return StartCoroutine(UnityWebRequestHandler.GetCouponDetails(_id, _response =>
        {
            _callback?.Invoke(_response);
        }));
    }


    private IEnumerator SetNFTImage(string _url)
    {
        yield return StartCoroutine(UnityWebRequestHandler.GetTexture(_url, _texture =>
        {
            dishImage.texture = _texture;
        }));
    }


    public void OnClick_Redeem()
    {
        NFTScreen.Instance.Redeem(nft, flag =>
        {
            if (flag)
            {
                redeem.GetComponentInChildren<TextMeshProUGUI>().text = "Pending...";
                redeem.interactable = false;
                withdraw.gameObject.SetActive(false);
            }
        });


    }

    public void OnClick_Withdraw()
    {
        NFTScreen.Instance.Withdraw(nft, flag =>
        {
            if (flag)
            {
                redeem.gameObject.SetActive(false);
                withdraw.gameObject.SetActive(false);
            }
        });

    }

}
