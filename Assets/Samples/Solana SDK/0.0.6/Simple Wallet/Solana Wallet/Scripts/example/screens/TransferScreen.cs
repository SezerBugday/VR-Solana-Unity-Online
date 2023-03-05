using System;
using Solana.Unity.Extensions.TokenMint;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace

namespace Solana.Unity.SDK.Example
{
    public class TransferScreen : SimpleScreen
    {
        public TextMeshProUGUI ownedAmountTxt;
        public TextMeshProUGUI nftTitleTxt;
        public TextMeshProUGUI errorTxt;
        private string toPublicTxt;
        private float amountTxt;

        public Button transferBtn;
        public RawImage nftImage;
        public Button closeBtn;

        private TokenAccount _transferTokenAccount;
        private Nft.Nft _nft;
        private double _ownedSolAmount;
        
        private const long SolLamports = 1000000000;

        private void Start()
        {
            toPublicTxt = "CXppym8LxUBEuqC9GV7gXio8gwupkFkXpLA5CBK7kdzf";
            amountTxt = 0.1f;
            transferBtn.onClick.AddListener(TryTransfer);

            closeBtn.onClick.AddListener(() =>
            {
                manager.ShowScreen(this, "wallet_screen");
            });
        }

        public void TryTransfer()
        {
            Debug.Log("TryTranfer çalisti");
            if (_nft != null)
            {
                TransferNft();
            }
            else if (_transferTokenAccount == null)
            {
                if (CheckInput())
                    TransferSol();
            }
            else
            {
                if (CheckInput())
                    TransferToken();
            }
        }

        private async void TransferSol()
        {
            RequestResult<string> result = await Web3.Instance.Wallet.Transfer(
                new PublicKey(toPublicTxt),
                Convert.ToUInt64(float.Parse(amountTxt.ToString())*SolLamports));
            HandleResponse(result);
        }

        private async void TransferNft()
        {
            RequestResult<string> result = await Web3.Instance.Wallet.Transfer(
                new PublicKey(toPublicTxt),
                new PublicKey(_nft.metaplexData.mint),
                1);
            HandleResponse(result);
        }

        bool CheckInput()
        {
            if (string.IsNullOrEmpty(amountTxt.ToString()))
            {
                errorTxt.text = "Please input transfer amount";
                return false;
            }

            if (string.IsNullOrEmpty(toPublicTxt))
            {
                errorTxt.text = "Please enter receiver public key";
                return false;
            }

            if (_transferTokenAccount == null)
            {
                if (float.Parse(amountTxt.ToString()) > _ownedSolAmount)
                {
                    errorTxt.text = "Not enough funds for transaction.";
                    return false;
                }
            }
            else
            {
                if (long.Parse(amountTxt.ToString()) > long.Parse(ownedAmountTxt.text))
                {
                    errorTxt.text = "Not enough funds for transaction.";
                    return false;
                }
            }
            errorTxt.text = "";
            return true;
        }

        private async void TransferToken()
        {
            RequestResult<string> result = await Web3.Instance.Wallet.Transfer(
                new PublicKey(toPublicTxt),
                new PublicKey(_transferTokenAccount.Account.Data.Parsed.Info.Mint),
                ulong.Parse(amountTxt.ToString()));
            HandleResponse(result);
        }

        private void HandleResponse(RequestResult<string> result)
        {
            errorTxt.text = result.Result == null ? result.Reason : "";
            if (result.Result != null)
            {
                manager.ShowScreen(this, "wallet_screen");
            }
        }

        public override async void ShowScreen(object data = null)
        {
            base.ShowScreen();

            ResetInputFields();
            await PopulateInfoFields(data);
            gameObject.SetActive(true);
        }
        
        public void OnClose()
        {
            var wallet = GameObject.Find("wallet");
            wallet.SetActive(false);
        }

        private async System.Threading.Tasks.Task PopulateInfoFields(object data)
        {
            nftImage.gameObject.SetActive(false);
            nftTitleTxt.gameObject.SetActive(false);
            ownedAmountTxt.gameObject.SetActive(false);
            if (data != null && data.GetType() == typeof(Tuple<TokenAccount, TokenDef, Texture2D>))
            {
                var (tokenAccount, tokenDef, texture) = (Tuple<TokenAccount, TokenDef, Texture2D>)data;
                ownedAmountTxt.text = $"{tokenAccount.Account.Data.Parsed.Info.TokenAmount.Amount}";
                nftTitleTxt.gameObject.SetActive(true);
                nftImage.gameObject.SetActive(true);
                nftTitleTxt.text = $"{tokenDef.Symbol}";
                nftImage.texture = texture;
                nftImage.color = Color.white;
            }
            else if (data != null && data.GetType() == typeof(Nft.Nft))
            {
                nftTitleTxt.gameObject.SetActive(true);
                nftImage.gameObject.SetActive(true);
                _nft = (Nft.Nft)data;
                nftTitleTxt.text = $"{_nft.metaplexData.data.name}";
                nftImage.texture = _nft.metaplexData?.nftImage?.file;
                nftImage.color = Color.white;
                amountTxt = 1;
                //amountTxt.interactable = false;
            }
            else
            {
                _ownedSolAmount = await Web3.Instance.Wallet.GetBalance();
                //ownedAmountTxt.text = $"{_ownedSolAmount}";
            }
        }

        private void ResetInputFields()
        {
            errorTxt.text = "";
            amountTxt = 0;
            toPublicTxt = "";
           // amountTxt.interactable = true;
        }

        public override void HideScreen()
        {
            base.HideScreen();
            _transferTokenAccount = null;
            gameObject.SetActive(false);
        }
    }

}