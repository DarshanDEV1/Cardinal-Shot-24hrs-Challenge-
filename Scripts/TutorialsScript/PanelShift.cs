using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PanelShift : MonoBehaviour
{
    /* NOTES:::: ->
     *  0 -> Left
     *  1 -> Right
     *  2 -> Up
     *  3 -> Down
     *  4 -> Fire
     */

    [SerializeField] private GameObject[] list = new GameObject[5];
    public bool[] bools = new bool[5];
    [SerializeField] private TuTGameManager gameManager;

    public void ActivateDeactivatePanels(bool config, int panel)
    {
        if (config)
        {
            //Activate only the panel number that is passed to the method as argument 
            //and rest all has to be Deactivated.
            for (int i = 0; i < list.Length; i++)
            {
                if (i != panel)
                {
                    //list[i].SetActive(false);
                    list[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    //list[i].SetActive(true);
                    list[i].GetComponent<Button>().interactable = true;
                    if(gameManager.buttonTutorial)
                        StartCoroutine(ButtonView(list[i]));
                }
            }
        }
        else
        {
            for (int i = 0; i < list.Length; i++)
            {
                //list[i].SetActive(true);
                list[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    private IEnumerator ButtonView(GameObject btn)
    {
        btn.GetComponent<Animator>().Play("Zoom");
        yield return new WaitForSeconds(1f);
        btn.GetComponent<Animator>().Play("Shrink");
        StopCoroutine(ButtonView(btn));
    }
}
