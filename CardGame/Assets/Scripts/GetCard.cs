using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;   // 引用 網路連線 API
using System.Collections;

public class GetCard : MonoBehaviour
{
    public CardData[] cards;

    public GameObject cardObject;
    public Transform cardPanel;

    private CanvasGroup loadingPanel;
    private Image loading;

    private IEnumerator GetCardData()
    {
        loadingPanel.alpha = 1;
        loadingPanel.blocksRaycasts = true;

        // 引用 (網路要求 www = 網路要求.Post("網址", ""))
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbw5tzw4smBcz6qqilN7odl11FelAK52gfzenMWBCubCWgLozV0/exec", ""))
        {
            // 等待 網路要求時間

            //yield return www.SendWebRequest();

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
                print(www.downloadHandler.text);

                cards = JsonHelper.FromJson<CardData>(www.downloadHandler.text);

                CreateCard();
            }
        }

        yield return new WaitForSeconds(0.5f);
        loadingPanel.alpha = 0;
        loadingPanel.blocksRaycasts = false;
    }

    private void CreateCard()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Transform temp = Instantiate(cardObject, cardPanel).transform;
            temp.Find("消耗").GetComponent<Text>().text = cards[i].cost.ToString();
            temp.Find("攻擊").GetComponent<Text>().text = cards[i].attack.ToString();
            temp.Find("血量").GetComponent<Text>().text = cards[i].hp.ToString();
            temp.Find("名稱").GetComponent<Text>().text = cards[i]._name;
            temp.Find("描述").GetComponent<Text>().text = cards[i].description;
        }
    }

    private void Start()
    {
        loadingPanel = GameObject.Find("載入畫面").GetComponent<CanvasGroup>();
        loading = GameObject.Find("進度條").GetComponent<Image>();
        StartCoroutine(GetCardData());
    }
}

[System.Serializable]
public class CardData
{
    public int index;
    public string _name;
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