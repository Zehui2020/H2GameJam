using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instruction;
    private bool isClicked = false;
    private bool canClick = false;

    private IEnumerator Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            instruction.text = "Press Anywhere To Begin!";
        else
            instruction.text = "Click Anywhere To Begin!";

        yield return new WaitForSeconds(1f);

        canClick = true;
    }

    private void Update()
    {
        if (isClicked || !canClick)
            return;

        if (Input.GetMouseButtonDown(0) || Input.touches.Count() > 0)
        {
            SceneLoader.Instance.LoadScene("MarketPlace");
            isClicked = true;
        }
    }
}