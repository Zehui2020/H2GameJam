using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] private Sprite gen1End;
    [SerializeField] private Sprite gen2End;
    [SerializeField] private Sprite gen3End;

    [SerializeField] private SpriteRenderer endSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerStats.playerStatsInstance)
            return;

        switch (PlayerStats.playerStatsInstance.currentGeneration.generation)
        {
            case GenerationData.Generation.Origins:
                endSprite.sprite = gen1End;
                break;
            case GenerationData.Generation.Renaissance:
                endSprite.sprite = gen2End;
                break;
            case GenerationData.Generation.Morden:
                endSprite.sprite = gen3End;
                break;
            default:
                endSprite.sprite = gen1End;
                break;
        }
        
    }
}
