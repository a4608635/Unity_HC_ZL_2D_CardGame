using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;   // 引用 網路連線 API
using System.Collections;

public class GetCard : MonoBehaviour
{
    [Header("卡牌資料")]
    public CardData[] cards;
    [Header("卡牌")]
    public GameObject cardObject;
    [Header("卡牌內容")]
    public Transform cardPanel;

    private CanvasGroup loadingPanel;
    private Image loading;

    public static GetCard instance;

    /// <summary>
    /// 載入卡牌資料
    /// </summary>
    private IEnumerator GetCardData()
    {
        loadingPanel.alpha = 1;
        loadingPanel.blocksRaycasts = true;

        // 引用 (網路要求 www = 網路要求.Post("網址", ""))
        UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbw5tzw4smBcz6qqilN7odl11FelAK52gfzenMWBCubCWgLozV0/exec", "");

        www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
            loading.fillAmount = www.downloadProgress;
        }

        if (www.isHttpError || www.isNetworkError)
        {
            print("連線錯誤：" + www.error);
        }
        else
        {
            cards = JsonHelper.FromJson<CardData>(www.downloadHandler.text);

            CreateCard();
        }

        yield return new WaitForSeconds(0.5f);
        loadingPanel.alpha = 0;
        loadingPanel.blocksRaycasts = false;
    }

    /// <summary>
    /// 生成卡牌
    /// </summary>
    private void CreateCard()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Transform temp = Instantiate(cardObject, cardPanel).transform;

            CardData card = cards[i];

            temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
            temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
            temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
            temp.Find("名稱").GetComponent<Text>().text = card.name;
            temp.Find("描述").GetComponent<Text>().text = card.description;

            temp.Find("遮色片").Find("圖片").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);

            temp.gameObject.AddComponent<CardDeck>().index = card.index;
        }
    }

    private void Start()
    {
        instance = this;

        loadingPanel = GameObject.Find("載入畫面").GetComponent<CanvasGroup>();
        loading = GameObject.Find("進度條").GetComponent<Image>();
        StartCoroutine(GetCardData());
    }
}

[System.Serializable]
public class CardData
{
    public int index;
    public string name;
    public string description;
    public int cost;
    public float attack;
    public float hp;
    public string file;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}