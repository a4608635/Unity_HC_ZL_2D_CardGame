using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 牌組管理器
/// </summary>
public class DeckManager : MonoBehaviour
{
    // 清單<要存放的類型> 清單名稱 = 新增 清單物件
    public List<CardData> deck = new List<CardData>();

    [Header("牌組物件")]
    public GameObject deckObject;
    [Header("牌組內榮")]
    public Transform deckContent;
    [Header("牌組數量")]
    public Text textDeckCount;

    /// <summary>
    /// 牌組管理器實體物件
    /// </summary>
    public static DeckManager instancs;

    private void Awake()
    {
        // 牌組管理器實體物件 = 此腳本
        instancs = this;
    }

    /// <summary>
    /// 添加選取的圖鑑卡牌至牌組內
    /// </summary>
    /// <param name="index">選取的圖鑑卡牌編號</param>
    public void AddCard(int index)
    {
        if (deck.Count < 30)
        {
            // 選取的卡牌
            CardData card = GetCard.instance.cards[index - 1];

            // => 黏巴達 Lambda 符號 C# 7 版新符號
            // 牌組.尋找全部(卡牌 => 卡牌.等於(選取的卡牌))
            List<CardData> sameCard = deck.FindAll(c => c.Equals(card));

            if (sameCard.Count < 2)
            {
                // 牌組.添加卡牌(選取的卡牌)
                deck.Add(card);

                Transform temp;

                if (sameCard.Count < 1)
                {
                    temp = Instantiate(deckObject, deckContent).transform;
                    temp.name = "牌組：" + card.name;
                }
                else
                {
                    temp = GameObject.Find("牌組：" + card.name).transform;
                }

                temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
                temp.Find("名稱").GetComponent<Text>().text = card.name;
                temp.Find("數量").GetComponent<Text>().text = (sameCard.Count + 1).ToString();

                textDeckCount.text = "牌組數量：" + deck.Count + " / 30";
            }
        }
    }

    /// <summary>
    /// 從牌組內刪除選取得圖鑑卡牌
    /// </summary>
    /// <param name="index">要刪除的圖鑑卡牌編號</param>
    public void DeleteCard(int index)
    {

    }
}
