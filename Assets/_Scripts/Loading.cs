using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public TextMeshProUGUI login_warningText;
    public TextMeshProUGUI signUp_warningText;
    public TMP_InputField login_usernameInputField;
    public TMP_InputField login_paswordInputField;
    public TMP_InputField signUp_usernameInputField;
    public TMP_InputField signUp_1stPaswordInputField;
    public TMP_InputField signUp_2ndPaswordInputField;
    public GameObject loadingProgress;

    private async void Awake()
    {
        loadingProgress.Enable();

        try
        {
            await UnityServices.InitializeAsync();

            await Task.Delay(500);

            print(AuthenticationService.Instance.AccessToken);

            if (AuthenticationService.Instance.SessionTokenExists)
            {
                SignInOptions options = new()
                {
                    CreateAccount = false
                };

                await AuthenticationService.Instance.SignInAnonymouslyAsync(options);

                SignedIn();
            }
            else
            {
                loadingProgress.Disable();
            }
        }
        catch
        {
            if (AuthenticationService.Instance.AccessToken == null ||
                !AuthenticationService.Instance.SessionTokenExists)
            {
                login_warningText.text = "Try Login or Signup";
                loadingProgress.Disable();
            }
        }
    }

    private async void SignedIn()
    {
        loadingProgress.Enable();

        while (!DataManager.DataLoaded)
        {
            await Task.Delay(1000);
        }

        GameStats.LoadDirectScene(GameStats.SceneMainMenu);
    }

    public async void B_Login()
    {
        string username = login_usernameInputField.text;
        string password1 = login_paswordInputField.text;

        loadingProgress.Enable();

        try
        {
            await SignInWithUsernamePasswordAsync(username, password1);
        }
        catch
        {

        }
    }

    public async void B_SignUp()
    {
        string username = signUp_usernameInputField.text;
        string password1 = signUp_1stPaswordInputField.text;
        string password2 = signUp_2ndPaswordInputField.text;

        if (string.IsNullOrEmpty(username))
        {
            signUp_warningText.text = "Email is empty!";
        }
        else if (!username.Contains("@gmail.com") && !username.Contains("@hotmail.com"))
        {
            signUp_warningText.text = "Email must contain @gmail.com or @hotmail.com";
        }
        else
        {
            int minLength = username.Contains("@gmail.com") ? 11 : 13;

            if (username.Length < minLength)
            {
                signUp_warningText.text = "Email length is not correct for the domain";
            }
            else if (password1.Length < 8)
            {
                signUp_warningText.text = "Password length must be at least 8 characters";
            }
            else if (password1 != password2)
            {
                signUp_warningText.text = "Passwords do not match";
            }
            else
            {
                loadingProgress.Enable();

                try
                {
                    await SignUpWithUsernamePasswordAsync(username, password1);
                }
                catch
                {

                }
            }
        }
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
            SignedIn();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message

            signUp_warningText.text = ex.Message;
            Debug.LogWarning(ex);
            loadingProgress.Disable();
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            signUp_warningText.text = ex.Message;
            Debug.LogWarning(ex);
            loadingProgress.Disable();
        }
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
            SignedIn();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            login_warningText.text = ex.Message;
            Debug.LogWarning(ex);
            loadingProgress.Disable();
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            login_warningText.text = ex.Message;
            Debug.LogWarning(ex);
            loadingProgress.Disable();
        }
    }
}
