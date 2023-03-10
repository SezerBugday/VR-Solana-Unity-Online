using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Solana.Unity.Wallet;
using UnityEngine.SceneManagement;
// ReSharper disable once CheckNamespace

namespace Solana.Unity.SDK.Example
{
    public class LoginScreen : SimpleScreen
    {
        [SerializeField]
        private TMP_InputField passwordInputField;
        [SerializeField]
        private TextMeshProUGUI passwordText;
        [SerializeField]
        private Button loginBtn;
        [SerializeField]
        private Button loginBtnGoogle;
        [SerializeField]
        private Button loginBtnTwitter;
        [SerializeField]
        private Button loginBtnPhantom;
        [SerializeField]
        private Button loginBtnXNFT;
        [SerializeField]
        private TextMeshProUGUI messageTxt;
        [SerializeField]
        private TMP_Dropdown dropdownRpcCluster;
       
        private void OnEnable()
        {
            dropdownRpcCluster.interactable = true;
            passwordInputField.text = string.Empty;
        }



        private void Start()
        {
            passwordText.text = "";

            passwordInputField.onSubmit.AddListener(delegate { LoginChecker(); });

            loginBtn.onClick.AddListener(LoginChecker);
            loginBtnGoogle.onClick.AddListener(delegate{LoginCheckerWeb3Auth(Provider.GOOGLE);});
            loginBtnTwitter.onClick.AddListener(delegate{LoginCheckerWeb3Auth(Provider.TWITTER);});
            loginBtnPhantom.onClick.AddListener(LoginCheckerPhantom);
            loginBtnXNFT.onClick.AddListener(LoginCheckerXnft);

            if (Application.platform != RuntimePlatform.Android && 
                Application.platform != RuntimePlatform.IPhonePlayer
                && Application.platform != RuntimePlatform.WindowsPlayer
                && Application.platform != RuntimePlatform.WindowsEditor
                && Application.platform != RuntimePlatform.LinuxPlayer
                && Application.platform != RuntimePlatform.LinuxEditor
                && Application.platform != RuntimePlatform.OSXPlayer
                && Application.platform != RuntimePlatform.OSXEditor)
            {
                loginBtnGoogle.gameObject.SetActive(false);
                loginBtnTwitter.gameObject.SetActive(false);
            }
            loginBtnXNFT.gameObject.SetActive(false);
           

           
      
               
         

            if (messageTxt != null)
                messageTxt.gameObject.SetActive(false);
        }
        
        private async void LoginChecker()
        {
            var password = passwordInputField.text;
            var account = await Web3.Instance.LoginInGameWallet(password);
            CheckAccount(account);
        }
        
        private async void LoginCheckerPhantom()
        {
            var account = await Web3.Instance.LoginPhantom();
            CheckAccount(account);
        }
        
        private async void LoginCheckerWeb3Auth(Provider provider)
        {
            var account = await Web3.Instance.LoginInWeb3Auth(provider);
            CheckAccount(account);
        }

        public void TryLoginBackPack()
        {
            LoginCheckerXnft();
        }

        private async void LoginCheckerXnft()
        {
            if(Web3.Instance == null) return;
            var account = await Web3.Instance.LoginXNFT();
            messageTxt.text = "";
            CheckAccount(account);
        }


        private void CheckAccount(Account account)
        {
            if (account != null)
            {
                dropdownRpcCluster.interactable = false;
                manager.ShowScreen(this, "wallet_screen");
                messageTxt.gameObject.SetActive(false);
                gameObject.SetActive(false);
                Debug.Log("Girildi");
               


            }
            else
            {
                passwordInputField.text = string.Empty;
                messageTxt.gameObject.SetActive(true);
            }
        }

        public void OnClose()
        {
            var wallet = GameObject.Find("wallet");
            wallet.SetActive(false);
        }
    }
}

