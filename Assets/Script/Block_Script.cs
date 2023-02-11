using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace block_script_namespace{
    public class Block_Script : MonoBehaviour
    {
        public int number;
        public string flag;
        public int i_index;
        public int j_index;
        public int j_add;
        public int fall_num = 0;
        public Rigidbody rb;
        bool is_fall =false;
        float start_y_position;

        public void Block_Fall(){
            // fall_num分だけブロックを一定時間移動させたい。位置が最終予想位置より上なら落とし続ける。
            this.is_fall = true;
            this.start_y_position = this.transform.position.y;
        }
        void Update()
        {
            if (this.flag == "destroy")
            {
                GetComponent<SpriteRenderer>().color = Color.gray;
            }
            if (this.is_fall)
            {

                if (rb.position.y >= start_y_position - fall_num * Stage_Script.block_size)
                {
                    Vector3 now = rb.position;
                    now += new Vector3(0.0f, -0.2f, 0.0f);
                    rb.position = now;
                }
                else
                {
                    // kamaseは最終地点。
                    Vector3 kamase = rb.position;
                    kamase.y = start_y_position - fall_num * Stage_Script.block_size;
                    rb.position = kamase;
                    this.fall_num = 0;
                    this.is_fall = false;
                }
            }
        }
        public void PointerEnter_Push(){
            if (Input.GetMouseButton(0)) {
                this.flag = "destroy";
            }
        }
        public void Drop(){
            this.flag = "sum";
        }
    }
}


