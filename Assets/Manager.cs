using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{


    public GameObject MarketPanel,MapPanel;
    public Button marketAc, marketKapa;
    void Start()
    {
        marketAc.onClick.AddListener(() => OpenMarket());
        marketKapa.onClick.AddListener(() => CloseMarket());
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
}
