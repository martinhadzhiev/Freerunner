using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text allTimeBestText;
    [SerializeField]
    private TMP_Text todaysBestText;
    [SerializeField]
    private TMP_Text congratsText;
    [SerializeField]
    private TMP_Text newAllTimeText;
    [SerializeField]
    private TMP_Text newTodaysText;
    [SerializeField]
    private Transform player;
    private RecordDatabase database;

    void Start()
    {
        scoreText.text = "0 m";
        database = new RecordDatabase();
        database.LoadRecord();
    }

    void Update()
    {
        var score = (long)Mathf.Floor(player.position.z);
        scoreText.text = score + " m";

        if (PauseMenu.GameIsPaused)
        {
            if (score > database.AllTimeBest || score > database.TodaysBest)
                database.WriteRecord(score);

            congratsText.enabled = database.NewAllTimeBest || database.NewTodaysBest;
            newAllTimeText.enabled = database.NewAllTimeBest;
            newTodaysText.enabled = database.NewTodaysBest;

            allTimeBestText.text = database.AllTimeBest + " m";
            todaysBestText.text = database.TodaysBest + " m";
        }
    }
}
