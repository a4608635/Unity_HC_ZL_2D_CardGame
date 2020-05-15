using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    [Header("牌組")]
    public List<CardData> deck = new List<CardData>();
    [Header("牌組物件：有新增與刪除功能的物件")]
    public List<GameObject> deckObject = new List<GameObject>();

    [Header("牌組卡牌")]
    public GameObject deckCardObject;
    [Header("牌組內容")]
    public Transform deckContent;

    private Text textDeckCount;
    private Button btnStart;

    private void Start()
    {
        instance = this;

        textDeckCount = GameObject.Find("卡牌數量").GetComponent<Text>();
        
        // 取得開始按鈕並取消互動、添加點擊後開始遊戲
        btnStart = GameObject.Find("開始遊戲").GetComponent<Button>();
        btnStart.interactable = false;
        btnStart.onClick.AddListener(StartGame);
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void StartGame()
    {
        SceneManager.LoadScene("戰鬥場景");
    }

    /// <summary>
    /// 新增卡牌
    /// </summary>
    /// <param name="index">要新增的卡牌編號</param>
    public void AddCard(int index)
    {
        if (deck.Count < 30)                                                                                // 如果 卡牌數量 低於 30 才能新增
        {
            List<CardData> sameCard = deck.FindAll(c => c.Equals(GetCard.instance.cards[index]));           // 搜尋相同卡牌的數量

            if (sameCard.Count < 2)                                                                         // 相同卡牌的數量 低於 2 才能新增卡牌 (相同卡牌最多 2 張)
            {
                deck.Add(GetCard.instance.cards[index]);                                                    // 牌組 加入 目前選取的卡牌資料

                textDeckCount.text = "卡牌數量：" + deck.Count + " / 30";                                    // 更新卡牌數量

                Transform temp;                                                                             // 要存放牌組物件的欄位

                if (deckObject[index])                                                                      // 如果牌組物件已經存在
                {
                    temp = deckObject[index].transform;                                                     // 牌組物件 = 目前選取的牌組物件
                }
                else
                {
                    temp = Instantiate(deckCardObject, deckContent).transform;                              // 否則 (還沒有選過) - 生成 牌組物件
                    temp.gameObject.AddComponent<DeckObject>().index = index;                               // 牌組物件 添加牌組物件腳本 - 儲存編號
                    deckObject[index] = temp.gameObject;                                                    //生成的牌組物件存入清單內
                }

                CardData card = GetCard.instance.cards[index];                                              // 取得卡牌資料

                temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();                         // 更新 消耗、名稱與數量
                temp.Find("名稱").GetComponent<Text>().text = card.name;
                temp.Find("數量").GetComponent<Text>().text = (sameCard.Count + 1).ToString();
            }
        }

        if (deck.Count == 30) btnStart.interactable = true;                                                 // 檢查卡牌等於 30 張 啟動按鈕
    }

    /// <summary>
    /// 刪除卡牌
    /// </summary>
    /// <param name="index">要刪除的卡牌編號</param>
    public void RemoveCard(int index)
    {
        List<CardData> sameCard = deck.FindAll(c => c.Equals(GetCard.instance.cards[index]));               // 搜尋相同卡牌的數量

        Transform temp = deckObject[index].transform;                                                       // 存放牌組物件

        if (sameCard.Count > 1)                                                                             // 如果此卡牌數量 大於 1 (兩張)
        {
            temp.Find("數量").GetComponent<Text>().text = (sameCard.Count - 1).ToString();                   // 數量 - 1 (更新為 1 張)
        }
        else
        {
            Destroy(temp.gameObject);                                                                       // 否則 (刪除剩 0 張) - 刪除牌組物件
        }

        deck.Remove(GetCard.instance.cards[index]);                                                         // 刪除卡牌
        textDeckCount.text = "卡牌數量：" + deck.Count + " / 30";                                            // 更新卡牌數量

        if (deck.Count < 30) btnStart.interactable = false;                                                 // 檢查卡牌低於 30 張 取消按鈕
    }
}
