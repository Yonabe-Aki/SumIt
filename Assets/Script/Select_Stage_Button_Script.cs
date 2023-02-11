using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select_Stage_Button_Script : MonoBehaviour
{
    public void Select_Stage()
    {
        GameObject text = transform.GetChild(0).gameObject;
        Stage_Script.stage_index = int.Parse(text.GetComponent<Text>().text);
        SceneManager.LoadScene("PlayScene");
    }
}
