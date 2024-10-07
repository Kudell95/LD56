using TMPro;
using UnityEngine;

public class LevelCounterManager : MonoBehaviour
{
    public TextMeshProUGUI CounterText;

    private void Awake() {
        GameManager.OnLevelCounterChanged += OnCounterChanged;
    }

    public void OnCounterChanged(int count)
    {
        CounterText.text = $"Level: {count + 1}";
    }


    private void OnDestroy() {
        GameManager.OnLevelCounterChanged -= OnCounterChanged;
    }
}
