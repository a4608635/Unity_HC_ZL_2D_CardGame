using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < DeckManager.instance.deck.Count; i++)
        {
            print(DeckManager.instance.deck[i].name);
        }
    }
}
