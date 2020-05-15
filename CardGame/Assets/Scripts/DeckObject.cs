using UnityEngine;
using UnityEngine.UI;

public class DeckObject : MonoBehaviour
{
    /// <summary>
    /// 牌組物件編號
    /// </summary>
    public int index;

    private void Start()
    {
        transform.Find("增加").GetComponent<Button>().onClick.AddListener(Add);
        transform.Find("減少").GetComponent<Button>().onClick.AddListener(Remove);
    }
    
    /// <summary>
    /// 牌組物件內的增加卡牌功能
    /// </summary>
    private void Add()
    {
        DeckManager.instance.AddCard(index);
    }

    /// <summary>
    /// 牌組物件內的減少卡牌功能
    /// </summary>
    private void Remove()
    {
        DeckManager.instance.RemoveCard(index);
    }
}
