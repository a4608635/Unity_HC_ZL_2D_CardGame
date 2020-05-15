using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour
{
    /// <summary>
    /// 卡牌編號
    /// </summary>
    public int index;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChooseCard);
    }

    /// <summary>
    /// 選擇卡牌：牌組加入目前卡牌
    /// </summary>
    private void ChooseCard()
    {
        DeckManager.instance.AddCard(index - 1);
    }
}
