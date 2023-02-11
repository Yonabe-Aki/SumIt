using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using block_script_namespace;
using System.Linq;


public class Stage_Script : MonoBehaviour
{
    // ステージを作る関数
    // ステージ・・・ブロック・eim・ひとつ戻るボタン・exitボタン
    public GameObject block_prefab;
    public Text number_text_prefab;
    private static GameObject canvas;
    public GameObject eim_prefab;
    public GameObject empty_prefab;
    GameObject block;
    GameObject[] blocks;
    static private GameObject eim;
    static private Eim_Script eim_script;
    static public int size;
    private static int[,] placement;
    static public int[,] ans_placement;
    float depth;
    public static int stage_index;
    public static float block_size;
    static float screen_width;
    static int num_vertical_block;
    static int num_horizon_block;
    LineRenderer liner;
    private GameObject line_empty;


    public static int[,] Placement { get => placement; set => placement = value; }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        depth = Math.Abs(Camera.main.transform.position.z);
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
        float screen_height = rightTop.y - leftBottom.y;
        screen_width = rightTop.x - leftBottom.x;
        // screen_heightが実際のスクリーンの大きさに対して小さい
        float stage_size = screen_height * 0.6f;
        switch (stage_index)
        {
            case 1:
                Placement = new int[,]
                {
                    {1,2},
                    {3,3}
                };
                ans_placement = new int[,]
                {
                    {0,0},
                    {6,3}
                };
                break;
            case 2:
                // 横がi、縦がj,左上が原点
                // ジャグ配列で書き換えた方がいいかも
                //実際の図とは横と縦が逆
                Placement = new int[,]
                {
                    {3,2,1},
                    {1,5,1},
                    {2,6,2}
                };

                ans_placement = new int[,]
                {
                    {0,3,2},
                    {0,0,7},
                    {3,6,2}
                };
                break;
            case 3:
                Placement = new int[,]
                {
                    {1,4,7},
                    {2,5,8},
                    {3,6,9}
                };

                ans_placement = new int[,]
                {
                    { 0, 0, 12},
                    { 0, 0, 15},
                    { 0, 0, 18},
                };
                break;
            case 4:
                Placement = new int[,]
                {
                    {2,5,2},
                    {3,6,7},
                    {3,1,9}
                };

                ans_placement = new int[,]
                {
                    { 0, 0, 15},
                    { 0, 0, 13},
                    { 0, 0, 10}
                };
                break;
            case 5:
                Placement = new int[,]
                {
                    {10, 4, 2},
                    {11, 6, 7},
                    { 7, 5, 3}
                };

                ans_placement = new int[,]
                {
                    { 0, 0, 22},
                    { 0, 0, 0},
                    { 0, 0, 33}
                };
                break;
            case 6:
                Placement = new int[,]
                {
                    { 3, 2,10},
                    { 2, 7, 3},
                    { 1, 2, 1}
                };

                ans_placement = new int[,]
                {
                    { 0,11,20},
                    { 0, 0, 0},
                    { 0, 0, 0}
                };
                break;
            case 7:
                Placement = new int[,]
                {
                    {1,1,1,1},
                    {1,1,1,1},
                    {1,1,1,1},
                    {1,1,1,1}
                };
                ans_placement = new int[,]
                {
                    {0,0,1,1},
                    {0,0,0,3},
                    {0,0,0,0},
                    {1,3,1,6}
                };
                break;
            case 8:
                Placement = new int[,]
                {
                    { 1, 1, 4},
                    { 5, 8, 5},
                    { 3, 4,21}
                };

                ans_placement = new int[,]
                {
                    { 0, 0, 0},
                    { 0,22,30},
                    { 0, 0, 0}
                };
                break;
            case 9:
                Placement = new int[,]
                {
                    { 6, 7, 7},
                    { 3, 4, 3},
                    { 0, 3, 5}
                };

                ans_placement = new int[,]
                {
                    { 0,13,10},
                    { 0, 0, 0},
                    { 0, 6, 9}
                };
                break;
            case 10:
                Placement = new int[,]
                {
                    {1,3,2,2},
                    {2,3,2,1},
                    {2,1,4,3},
                    {1,5,1,4}

                };
                ans_placement = new int[,]
                {
                    {0,0,1, 9,},
                    {0,0,0, 0,},
                    {0,2,7,13,},
                    {0,0,1, 4,},

                };
                break;
            case 11:
                Placement = new int[,]
                {
                    { 3, 3, 2},
                    { 8, 5, 3},
                    { 9, 2, 3}
                };

                ans_placement = new int[,]
                {
                    { 0, 0,25},
                    { 0, 0,13},
                    { 0, 0, 0}
                };
                break;

        }
        size = Placement.GetLength(0);
        num_vertical_block = Placement.GetLength(0);
        num_horizon_block = Placement.GetLength(1);
        block_size = stage_size / (Math.Max(num_horizon_block, num_vertical_block));
        Create_Stage();
        eim = GameObject.Find("eim_prefab(Clone)");
        eim_script = eim.GetComponent<Eim_Script>();
    }
    void Create_Block(int i,int j)
    {
        float x = (i - (num_horizon_block - 1) / 2.0f) * (block_size) + screen_width * 0.25f;
        float y = -(j - (num_vertical_block - 1) / 2.0f) * (block_size);
        GameObject block = Instantiate(block_prefab, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
        block.transform.localScale = new Vector3(block_size, block_size, 1.0f);
        block.transform.SetParent(canvas.transform, true);
        block.tag = "block";
        Block_Script s = block.GetComponent<Block_Script>(); //コンポーネントを取得
        s.number = Placement[i, j];
        s.i_index = i;
        s.j_index = j;
        Text number_text = Instantiate(number_text_prefab) as Text;
        number_text.transform.SetParent(block.transform, false);
        number_text.text = Placement[i, j].ToString();
    }

    void Create_Block_Stage()
    {
        for (int i = 0; i < num_vertical_block; i++)
        {
            for (int j = 0; j < num_horizon_block; j++)
            {
                // placementに従ってブロックを生成
                Create_Block(i, j);
                // ans_placementに従ってブロックを生成
                if (ans_placement[i, j] != 0)
                {
                    float x = (i-(num_horizon_block-1) / 2.0f) * (block_size) - screen_width * 0.25f;
                    float y = -(j - (num_vertical_block - 1) / 2.0f) * (block_size);
                    GameObject ans_block = Instantiate(block_prefab, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                    ans_block.transform.localScale = new Vector3(block_size, block_size, 1.0f);
                    ans_block.transform.SetParent(canvas.transform, true);
                    Text ans_number_text = Instantiate(number_text_prefab) as Text;
                    ans_number_text.transform.SetParent(ans_block.transform, false);
                    ans_number_text.text = ans_placement[i, j].ToString();
                }
            }
        }    
    }
    void Create_Eim()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        // すべてのブロックについてコンポーネントを取得してインデックスが00のやつを親にする これが結論 foreach使えそう
        foreach (GameObject block in blocks)
        {
            Block_Script s = block.GetComponent<Block_Script>(); 
            if (s.i_index == 0 && s.j_index == 0)
            {
                GameObject eim = Instantiate(eim_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                eim.transform.SetParent(block.transform, false);
                break;
            }
        }
    }

    //縦線と横線をそれぞれ問題側と回答がわで全4回分
    void Create_frame()
    {
        for (int i = 0; i < num_vertical_block + 1; i++)
        {            
            GameObject line_empty = Instantiate(empty_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            liner = line_empty.GetComponent<LineRenderer>();
            liner.sortingLayerName = "eim";
            liner.useWorldSpace = false;
            liner.startColor = Color.black;
            liner.endColor = Color.black;
            float x = (i - num_horizon_block / 2.0f) * (block_size) + screen_width * 0.25f;
            float y_top = block_size * num_vertical_block / 2.0f;
            float y_bottom = -1.0f * block_size * num_vertical_block / 2.0f;
            Vector3 pos1 = new Vector3(x, y_top, 0f);
            Vector3 pos2 = new Vector3(x, y_bottom, 0f);
            liner.SetPosition(0, pos1);
            liner.SetPosition(1, pos2);
        }
        for (int i = 0; i < num_vertical_block + 1; i++)
        {
            GameObject line_empty = Instantiate(empty_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            liner = line_empty.GetComponent<LineRenderer>();
            liner.sortingLayerName = "eim";
            liner.useWorldSpace = false;
            liner.startColor = Color.black;
            liner.endColor = Color.black;
            float y = (i - num_vertical_block / 2.0f) * (block_size);
            float x_right = block_size * num_horizon_block / 2.0f + screen_width * 0.25f;
            float x_left = -1.0f * block_size * num_horizon_block / 2.0f + screen_width * 0.25f;
            Vector3 pos1 = new Vector3(x_right, y, 0f);
            Vector3 pos2 = new Vector3(x_left, y, 0f);
            liner.SetPosition(0, pos1);
            liner.SetPosition(1, pos2);
        }
        for (int i = 0; i < num_vertical_block + 1; i++)
        {
            GameObject line_empty = Instantiate(empty_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            liner = line_empty.GetComponent<LineRenderer>();
            liner.sortingLayerName = "eim";
            liner.useWorldSpace = false;
            liner.startColor = Color.black;
            liner.endColor = Color.black;
            float x = (i - num_horizon_block / 2.0f) * (block_size) - screen_width * 0.25f;
            float y_top = block_size * num_vertical_block / 2.0f;
            float y_bottom = -1.0f * block_size * num_vertical_block / 2.0f;
            Vector3 pos1 = new Vector3(x, y_top, 0f);
            Vector3 pos2 = new Vector3(x, y_bottom, 0f);
            liner.SetPosition(0, pos1);
            liner.SetPosition(1, pos2);
        }
        for (int i = 0; i < num_vertical_block + 1; i++)
        {
            GameObject line_empty = Instantiate(empty_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            liner = line_empty.GetComponent<LineRenderer>();
            liner.sortingLayerName = "eim";
            liner.useWorldSpace = false;
            liner.startColor = Color.black;
            liner.endColor = Color.black;
            float y = (i - num_vertical_block / 2.0f) * (block_size);
            float x_right = block_size * num_horizon_block / 2.0f - screen_width * 0.25f;
            float x_left = -1.0f * block_size * num_horizon_block / 2.0f - screen_width * 0.25f;
            Vector3 pos1 = new Vector3(x_right, y, 0f);
            Vector3 pos2 = new Vector3(x_left, y, 0f);
            liner.SetPosition(0, pos1);
            liner.SetPosition(1, pos2);
        }
    }

    void Create_Stage()
    {
        Create_Block_Stage();
        Create_Eim();
        Create_frame();
    }

    public void Data_Fall()
    {
        // まず列ごとの0の数を数え保存する。次に0以外の要素ごとに下にある0の個数を数え保存する。0以外の要素を下に移動させる。頭にcount_0の個数だけ0をつける。
        int[] i_placement_col = new int[size];
        int count_0 = 0;
        int count_fallnum = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                i_placement_col[j] = placement[i, j];
                // jより下(jは含まない)の列をij_lowert_arrayに格納する
            }
            int[] fallnum_save_array = new int[size];
            for (int j = 0; j < size; j++)
            {
                int[] ij_lower_array = new int[size - j - 1];
                if (placement[i, j] == 0)
                {
                    count_0++;
                }
                else if (j < size - 1)
                {
                    for (int j2 = 0; j2 < size - j - 1; j2++)
                    {
                        ij_lower_array[j2] = i_placement_col[j + j2 + 1];
                    }
                    count_fallnum = ij_lower_array.Count(n => n == 0);
                    // [i,j]のブロックを見つけてfall_numを設定する
                    GameObject[] blocks2 = GameObject.FindGameObjectsWithTag("block");
                    foreach (GameObject block in blocks2)
                    {
                        Block_Script s = block.GetComponent<Block_Script>();
                        if (s.i_index == i && s.j_index == j)
                        {
                            s.fall_num = count_fallnum;
                            s.j_add += count_fallnum;
                        }
                    }

                }
                //ここでjのブロックのfall_numを記録する        
                fallnum_save_array[j] = count_fallnum;
                count_fallnum = 0;
            }           
            //５がいくら落ちるか計算する前に先に１がデータ上で落ちてしまっているからバグってる
            //落とす処理はここで一列まとめてやる
            //i_placement_col[j]が0なら無視する。
            //0のブロックのところはfallnumが正確ではないがどうせ無視するのでこれでよし =>怪しくなってきた
            //int[] save_i_placement_col = new int[size];



      

            TestClass.BringBeforeZero bring_before_zero = new TestClass.BringBeforeZero();
            // i_placement_colをplacementのi列にする。
            for (int j = 0; j < size; j++)
            {
                //placement[i, j] = i_placement_col[j];
                placement[i, j] = bring_before_zero.BringBeforeZeroFunc(i_placement_col, count_0)[j];

            }
            count_0 = 0;
            //i_placement_col[j + count_fallnum] = i_placement_col[j];
            //ブロックのjインデックスを下げる
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
            foreach (GameObject block in blocks)
            {
                Block_Script s = block.GetComponent<Block_Script>();
                if (s.i_index == i)
                {
                    s.j_index += s.j_add;
                    s.j_add = 0;
                }
            }
        }
    }
    public bool CanEimMoveToBlock(int i, int j)
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject block in blocks)
        {
            Block_Script s3 = block.GetComponent<Block_Script>();
            if (s3.i_index == i && s3.j_index == j)
            {
                if (s3.flag == "destroy")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
    private int Sum_Selected_Block()
    {
        int sum_number = 0;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject block in blocks)
        {
            Block_Script s = block.GetComponent<Block_Script>();
            if (s.flag == "destroy" || s.flag == "sum")
            {
                sum_number += s.number;
            }
        }
        return sum_number;
    }
    public void Sum_Dest_Inst()
    {
        int sum_number = Sum_Selected_Block();
        int i = eim_script.eim_i_index;
        int j = eim_script.eim_j_index;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject destroyed_block in blocks)
        {
            Block_Script s = destroyed_block.GetComponent<Block_Script>();
            // des,sumブロックの場所のplacementを0にする
            if (s.flag == "destroy" || s.flag == "sum")
            {
                Placement[s.i_index, s.j_index] = 0;
                Destroy(destroyed_block);
            }
        }
        Placement[i, j] = sum_number;
        float x = (i - (num_horizon_block - 1) / 2.0f) * (block_size) + screen_width * 0.25f;
        float y = -(j - (num_vertical_block - 1) / 2.0f) * (block_size);
        GameObject block = Instantiate(block_prefab, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
        block.transform.localScale = new Vector3(block_size, block_size, 1.0f);
        block.tag = "block";
        block.transform.SetParent(canvas.transform, true);
        //eimを生成したブロックの子にする
        //この時のeim_j_indexに注意
        eim.transform.SetParent(block.transform, false);
        Block_Script s2 = block.GetComponent<Block_Script>();
        s2.number = sum_number;
        s2.i_index = i;
        s2.j_index = j;
        Text number_text = Instantiate(number_text_prefab) as Text;
        number_text.transform.SetParent(block.transform, false);
        number_text.text = sum_number.ToString();
        sum_number = 0;
    }

    private void Update()
    {
        eim_script.MoveEim();
        eim_script.Press_Space();
    }
    //eimでブロック消した時に正解か判定させたいのでEim_Scriptで呼び出し
    public bool IsClear()
    {
        for (int i = 0; i < num_vertical_block; i++)
        {
            for (int j = 0; j < num_horizon_block; j++)
            {
                if (placement[i, j] != ans_placement[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }

}

namespace TestClass
{
    public class CalcAdd
    {
        public int AddInt(int a, int b)
        {
            return a + b;
        }
    }
    public class BringBeforeZero
    {
        public static int[] Remove0(int[] arr)
        {
            List<int> list = new List<int>(arr);
            list.RemoveAll(num => num == 0);
            return list.ToArray();
        }

        public int[] Main(int[] arr)
        {
            int[] newArr = Remove0(arr);
            return newArr;
        }

        //count_0の数だけ配列の頭に０を追加する
        public int[] Add0ToHead(int[] input,int count_0)
        {            
            if(count_0 == 0)
            {
                return input;
            }
            else
            {
                int[] result = new int[input.Length + count_0];
                for (int i=0; i<count_0;i++)
                {
                    result[i] = 0;
                }
                Array.Copy(input, 0, result, count_0, input.Length);
                return result;
            }
        }
        public int[] BringBeforeZeroFunc(int[] input, int count_0)
        {
            return Add0ToHead(Main(input), count_0);
        }

    }

}