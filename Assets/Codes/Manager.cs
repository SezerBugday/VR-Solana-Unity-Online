using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using codebase.utility;
using Cysharp.Threading.Tasks;
using Solana.Unity.Extensions;
using Solana.Unity.Rpc.Types;

// ReSharper disable once CheckNamespace

namespace Solana.Unity.SDK.Example
{
    public class Manager : SimpleScreen
    {


        public GameObject MarketPanel, MapPanel, HerseyPanel, TransferPanel,Deneme;
        public Button marketAc, marketKapa, HerseyKapa;



        void Start()
        {
            marketAc.onClick.AddListener(() => OpenMarket());
            marketKapa.onClick.AddListener(() => CloseMarket());
            HerseyKapa.onClick.AddListener(() => HerKapa());
           

        }
        public void OpenMarket()
        {
            MarketPanel.SetActive(true);
            MapPanel.SetActive(false);
        }

        public void CloseMarket()
        {
            MarketPanel.SetActive(false);
            MapPanel.SetActive(true);
        }
        public void HerKapa()
        {
            HerseyPanel.SetActive(false);

        }

        public void TransferAc()
        {
            TransferPanel.transform.parent = Deneme.transform;
            TransferPanel.SetActive(true);
           
        }
   

    }
}