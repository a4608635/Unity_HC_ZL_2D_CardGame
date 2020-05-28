using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 對戰管理器實體物件
    /// </summary>
    public static BattleManager instance;

    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;

    /// <summary>
    /// 先後攻
    /// true 先
    /// false 後
    /// </summary>
    private bool firstAttack;

    /// <summary>
    /// 對戰用牌組：手牌
    /// </summary>
    public List<CardData> battleDeck = new List<CardData>();
    public List<GameObject> battleDeckCard = new List<GameObject>();

    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartBattle()
    {
        gameView.SetActive(true);   // 顯示遊戲畫面

        ThrowCoin();
    }

    /// <summary>
    /// 擲金幣
    /// </summary>
    private void ThrowCoin()
    {
        coin.AddForce(0, Random.Range(300, 500), 0);            // 推力
        coin.AddTorque(Random.Range(200, 500), 0, 0);           // 旋轉

        Invoke("CheckCoin", 3);                                 // 延遲呼叫檢查方法
    }

    /// <summary>
    /// 檢查金幣正反面
    /// rotation.x 為 -1 - 背面
    /// rotation.x 為 0  - 正面
    /// </summary>
    private void CheckCoin()
    {
        // 三元運算子
        // 先後攻 = 布林運算 ? 成立 : 不成立
        firstAttack = coin.transform.eulerAngles.x >= 0.3f ? true : false;

        StartCoroutine(GetCard(3));
    }

    /// <summary>
    /// 抽牌組卡排到手上牌組
    /// </summary>
    private IEnumerator GetCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 抽牌組第一張 放到 手牌 第一張
            battleDeck.Add(DeckManager.instance.deck[0]);
            // 刪除 牌組第一張
            DeckManager.instance.deck.RemoveAt(0);

            battleDeckCard.Add(DeckManager.instance.deckCard[0]);

            DeckManager.instance.deckCard.RemoveAt(0);

            yield return StartCoroutine(MoveCard());
        }
    }

    public Transform canvas;
    public Transform handCard;

    /// <summary>
    /// 移動卡片：顯示出來再放到手排
    /// </summary>
    private IEnumerator MoveCard()
    {
        RectTransform card = battleDeckCard[battleDeckCard.Count - 1].GetComponent<RectTransform>();

        card.anchorMin = Vector2.one * 0.5f;
        card.anchorMax = Vector2.one * 0.5f;

        card.SetParent(canvas);

        while (card.anchoredPosition.x > 501)
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, 0), 0.5f * Time.deltaTime * 35);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        card.localScale = Vector3.one * 0.5f;

        while (card.anchoredPosition.y > -274)
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -275), 0.5f * Time.deltaTime * 35);
            yield return null;
        }

        card.SetParent(handCard);
        card.gameObject.AddComponent<HandCard>();
    }
}
