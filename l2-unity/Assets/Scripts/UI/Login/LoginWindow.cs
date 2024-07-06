using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginWindow : L2Window
{
    private VisualElement _logo;
    private TextField _userInput;
    private TextField _passwordInput;

    private static LoginWindow _instance;
    public static LoginWindow Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(this);
        }
    }

    private void OnDestroy() {
        _instance = null;
    }

    protected override void LoadAssets() {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Login/LoginWindow");
    }

    protected override IEnumerator BuildWindow(VisualElement root) {
        InitWindow(root);

        yield return new WaitForEndOfFrame();

        Button loginButton = (Button) GetElementById("LoginButton");
        loginButton.AddManipulator(new ButtonClickSoundManipulator(loginButton));
        loginButton.RegisterCallback<ClickEvent>(evt => LoginButtonPressed());

        Button exitButton = (Button)GetElementById("ExitButton");
        exitButton.AddManipulator(new ButtonClickSoundManipulator(exitButton));
        exitButton.RegisterCallback<ClickEvent>(evt => ExitButtonPressed());

        VisualElement userInputBg = GetElementById("UserInputBg");
        _userInput = (TextField) GetElementById("UserInputField");
        _userInput.RegisterCallback<FocusEvent>((evt) => OnInputFocus(evt, _userInput));
        _userInput.RegisterCallback<BlurEvent>((evt) => OnInputBlur(evt, _userInput));
        _userInput.maxLength = 16;
        _userInput.RegisterValueChangedCallback(OnLoginInputChanged);

        _userInput.AddManipulator(new HighlightedInputFieldManipulator(_userInput, userInputBg, 20));
        _userInput.AddManipulator(new BlinkingCursorManipulator(_userInput));

        VisualElement passwordInputBg = GetElementById("PasswordInputBg");
        _passwordInput = (TextField)GetElementById("PasswordInputField");
        _passwordInput.RegisterCallback<FocusEvent>((evt) => OnInputFocus(evt, _passwordInput));
        _passwordInput.RegisterCallback<BlurEvent>((evt) => OnInputBlur(evt, _passwordInput));
        _passwordInput.maxLength = 16;

        _passwordInput.AddManipulator(new HighlightedInputFieldManipulator(_passwordInput, passwordInputBg, 20));
        _passwordInput.AddManipulator(new BlinkingCursorManipulator(_passwordInput));

        _logo = root.Q<VisualElement>("L2Logo");
    }

    private void OnLoginInputChanged(ChangeEvent<string> evt) {
        string username = evt.newValue;

        // Regex pattern: ^[a-zA-Z0-9]{1,16}$
        // This allows only alphanumeric characters, 1-16 characters long
        Regex regex = new Regex(@"^[a-zA-Z0-9]{0,16}$");

        if (!regex.IsMatch(username)) {
            // If the input doesn't match the regex, revert to the previous value
            _userInput.SetValueWithoutNotify(evt.previousValue);
        }
    }

    private void OnInputFocus(FocusEvent evt, VisualElement input) {
        if (!input.ClassListContains("highlighted")) {
            input.AddToClassList("highlighted");
        }
    }

    private void OnInputBlur(BlurEvent evt, VisualElement input) {
        if (input.ClassListContains("highlighted")) {
            input.RemoveFromClassList("highlighted");
        }
    }

    private void LoginButtonPressed() {
        if (_userInput.text.Length == 0 && _passwordInput.text.Length == 0) {
            _userInput.value = "Shnok";
            _passwordInput.value = "1234";
        } else if(_passwordInput.text.Length == 0) {
            return;
        }

        LoginClient.Instance.Account = _userInput.value.ToLower().Trim();
        LoginClient.Instance.Password = _passwordInput.value.Trim();
        LoginClient.Instance.Connect();
    }

    private void ExitButtonPressed() {
        Application.Quit();
    }
}