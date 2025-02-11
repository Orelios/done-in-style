using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private EndScreen endScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        endScreen.Toggle(true);
        endScreen.EndScreenText("You finished the level!");
    }
}
