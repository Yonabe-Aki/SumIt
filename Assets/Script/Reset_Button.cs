using System.Collections;
using System.Collections.Generic;
using block_script_namespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset_Button : MonoBehaviour
{
    // Start is called before the first frame update
    public void Reset()
    {
        //GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        //foreach (GameObject block in blocks)
        //{
        //        Destroy(block);            
        //}
        //Application.LoadLevel("SelectStageScene");
        SceneManager.LoadScene(2);
    }
}
