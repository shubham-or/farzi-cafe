using TMPro;
using UnityEngine;

public class FirebaseDBHandler : MonoBehaviour
{
    public TextMeshProUGUI uidText;
    public TMP_InputField nameInputField;
    public TMP_InputField pathInputField;
    public TMP_InputField valueInputField;

    public TMP_InputField amountInputField;

    public TextMeshProUGUI outputText;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
    }

    public void GetJSON() =>
        FirebaseDBLibrary.GetJSON(pathInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void PostJSON() =>
        FirebaseDBLibrary.PostJSON(pathInputField.text, valueInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void PushJSON()
    {
        FirebaseDBLibrary.PushJSON(pathInputField.text, valueInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public void UpdateJSON()
    {
        if (!string.IsNullOrWhiteSpace(nameInputField.text))
        {
            string _path = "";
            FirebaseDBLibrary.UpdateUserName(_path, "key", nameInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");
        }
        else
            DisplayErrorObject("Name is Empty");
    }

    public void DeleteJSON() =>
        FirebaseDBLibrary.DeleteJSON(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForValueChanged() =>
        FirebaseDBLibrary.ListenForValueChanged(pathInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForValueChanged() =>
        FirebaseDBLibrary.StopListeningForValueChanged(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForChildAdded() =>
        FirebaseDBLibrary.ListenForChildAdded(pathInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForChildAdded() =>
        FirebaseDBLibrary.StopListeningForChildAdded(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForChildChanged() =>
        FirebaseDBLibrary.ListenForChildChanged(pathInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForChildChanged() =>
        FirebaseDBLibrary.StopListeningForChildChanged(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForChildRemoved() =>
        FirebaseDBLibrary.ListenForChildRemoved(pathInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForChildRemoved() =>
        FirebaseDBLibrary.StopListeningForChildRemoved(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ModifyNumberWithTransaction()
    {
        float.TryParse(amountInputField.text, out var amount);
        FirebaseDBLibrary.ModifyNumberWithTransaction(pathInputField.text, amount, gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public void ToggleBooleanWithTransaction() =>
        FirebaseDBLibrary.ToggleBooleanWithTransaction(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void DisplayData(string data)
    {
        outputText.color = Color.green;
        outputText.text = data;
        Debug.Log(data);
    }

    public void DisplayInfo(string info)
    {
        outputText.color = Color.white;
        outputText.text = info;
        Debug.Log(info);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        outputText.color = Color.red;
        outputText.text = error;
        Debug.LogError(error);
    }
}
