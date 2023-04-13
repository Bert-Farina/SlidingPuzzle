using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    
    public int seconds;
    public int minutes;

    void Start()
    {
        AddASecond();
    }

    private void AddASecond()
    {
        seconds++;
        if (seconds > 59)
        {
            minutes++;
            seconds = 0;
        }
        
        //TimeFormat
        timeText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        Invoke(nameof(AddASecond), 1);
    }

    public void StopTimer()
    {
        CancelInvoke(nameof(AddASecond));
        timeText.gameObject.SetActive(false);
    }
}
