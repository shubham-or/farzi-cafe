using MoralisUnity;
using MoralisUnity.Kits.AuthenticationKit;
using MoralisUnity.Platform.Objects;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    public AuthenticationKit authenticationKit;
    public string dappUrl;
    public string dappId;

    public TMPro.TMP_InputField usernameIF;
    public TMPro.TMP_InputField passwordIF;
    public TMPro.TMP_InputField emailIF;
    public Button createUserBtn;

    void Start()
    {
        createUserBtn.onClick.AddListener(CreateMoralisUser);
    }

    public async void Moralis_UserLogin()
    {
        var user = await Moralis.LogInAsync("shraddha_Patel", "mypass");
    }


    [ContextMenu("CreateMoralisUser")]
    public async void CreateMoralisUser()
    {
        print("CreateMoralisUser");
        MoralisUser user = Moralis.Create<MoralisUser>();
        user.username = usernameIF.text.Trim();
        user.password = passwordIF.text.Trim();
        user.email = emailIF.text.Trim(); // optional
        try
        {
            // this signs the user up and doesnt log them in
            await user.SignUpAsync();
            // Hooray! Let them login the app now.
        }
        catch (System.Exception error)
        {
            // Show the error message somewhere and let the user try again.
            Debug.Log("Error :" + error);
        }
    }
}
