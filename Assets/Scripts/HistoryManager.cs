using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistoryManager : MonoBehaviour
{
    //Datas
    [SerializeField] private List<HistoryData> historyDatas;
    //User Interface
    [SerializeField] private TextMeshProUGUI genDayText;
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI descText;


    // Start is called before the first frame update
    void Start()
    {
        GenerationData.Generation currGen = PlayerStats.playerStatsInstance.currentGeneration.generation;
        int currDay = PlayerStats.playerStatsInstance.dayCounter;

        //check for the correct description to use
        foreach (HistoryData data in  historyDatas)
        {
            //check if same generation and day
            if (data.gen == currGen && data.day == currDay)
            {
                //assign data
                genDayText.text = "Current Gen: " + (data.gen == GenerationData.Generation.Tutorial ? "Tutorial(0) " : (data.gen == GenerationData.Generation.Origins ? "Origin(1) " : (data.gen == GenerationData.Generation.Renaissance ? "Renaissance(2) " : "Modern(3) ")))
                    + "| Day: " + data.day;

                yearText.text = "Current Year: " + data.currentYear;

                descText.text = data.desc;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
