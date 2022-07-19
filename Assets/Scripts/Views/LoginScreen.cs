using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : BasePopup
{
    [SerializeField]
    private GameObject _btnLogin;

    [SerializeField]
    private InputField _inputUsername;

    [SerializeField]
    private InputField _inputPassword;

    public Action<string, string> LoginClicked;

    // Use this for initialization
    void Start()
    {
        _btnLogin.SetOnClick(OnLoginClicked);
        _inputUsername.text = "test0";
        _inputPassword.text = "test0";
    }

    public void OnLoginClicked(GameObject sender)
    {
        var username = _inputUsername.text;
        var password = _inputPassword.text;

        LoginClicked?.Invoke(username, password);
    }
}
