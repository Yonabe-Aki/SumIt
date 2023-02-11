using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using block_script_namespace;


public class Eim_Script : MonoBehaviour
{
    static private GameObject eim;
    public int eim_i_index = 0;
    public int eim_j_index = 0;
    static private bool is_selecting = false;
    private GameObject Finded_Block;
    static private Stage_Script stage_script;
    static private Block_Script block_script;
    public GameObject circle_prefab;


    void Start()
    {
        eim = gameObject;
        stage_script = GameObject.Find("Game Manager").GetComponent<Stage_Script>();        
    }
    public GameObject Find_Eiming_Block()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        foreach(GameObject block in blocks)
        {
            Block_Script s = block.GetComponent<Block_Script>();
            if(s.i_index == eim_i_index && s.j_index == eim_j_index)
            {
                return block;
            }
        }
        return null;
    }
    public void MoveEim()
    {
        // 十字キーで移動させる
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.eim_i_index++;
            Finded_Block = Find_Eiming_Block();
            if (stage_script.CanEimMoveToBlock(eim_i_index, eim_j_index))
            {
                eim.transform.SetParent(Finded_Block.transform, false);
                if (is_selecting)
                {
                    Finded_Block.GetComponent<Block_Script>().flag = "destroy";
                }
            }
            else
            {

                this.eim_i_index--;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.eim_i_index--;
            Finded_Block = Find_Eiming_Block();

            if (stage_script.CanEimMoveToBlock(eim_i_index, eim_j_index))
            {
                eim.transform.SetParent(Finded_Block.transform, false);
                if (is_selecting)
                {
                    Finded_Block.GetComponent<Block_Script>().flag = "destroy";
                }
            }
            else
            {
                this.eim_i_index++;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.eim_j_index--;
            Finded_Block = Find_Eiming_Block();
            if (stage_script.CanEimMoveToBlock(eim_i_index, eim_j_index))
            {
                eim.transform.SetParent(Finded_Block.transform, false);
                if (is_selecting)
                {
                    Finded_Block.GetComponent<Block_Script>().flag = "destroy";
                }
            }
            else
            {
                this.eim_j_index++;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.eim_j_index++;
            Finded_Block = Find_Eiming_Block();
            if (stage_script.CanEimMoveToBlock(eim_i_index, eim_j_index))
            {
                eim.transform.SetParent(Finded_Block.transform, false);
                if (is_selecting)
                {
                    Finded_Block.GetComponent<Block_Script>().flag = "destroy";
                }
            }
            else
            {
                this.eim_j_index--;
            }
        }
    }
    public void Press_Space()
    {
        //何も選んでない状態でスペース押したらis_selectingがtrueになる
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if(!is_selecting){
                Find_Eiming_Block().GetComponent<Block_Script>().flag = "destroy";
                is_selecting = true;
            }
            else if(is_selecting)
            {
                Find_Eiming_Block().GetComponent<Block_Script>().flag = "sum";
                is_selecting = false;
                stage_script.Sum_Dest_Inst();
                stage_script.Data_Fall();
                //Find_Eiming_Blockは検索にeim_j_indexを使う。間違ったeim_j_indexを修正するために間違ってるeim_j_indexを使うのでうまく行くはずもない
                //this.eim_j_index = Find_Eiming_Block().GetComponent<Block_Script>().j_index;
                //普通に親オブジェクトのコンポーネントからj_indexを取得するのが良さそう
                this.eim_j_index = transform.parent.gameObject.GetComponent<Block_Script>().j_index;
                GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
                foreach (GameObject block in blocks)
                {
                    block.GetComponent<Block_Script>().Block_Fall();
                }
                if(stage_script.IsClear())
                {
                    Instantiate(circle_prefab);
                }
            }

        }   

    }
}

