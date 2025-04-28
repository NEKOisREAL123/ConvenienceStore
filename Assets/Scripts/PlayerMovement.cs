using UnityEngine;

namespace ConvenienceStore
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        
        private CapsuleCollider playerCollider;
        
        void Start()
        {
            // 获取胶囊碰撞器
            playerCollider = GetComponent<CapsuleCollider>();
            if (playerCollider != null)
            {
                // 将碰撞器设置为触发器，这样就不会被墙体物理反弹
                playerCollider.isTrigger = true;
            }
        }
        
        void Update()
        {
            // 获取输入
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // 创建移动向量
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            
            // 使用Transform.Translate进行移动，而不是使用物理引擎的力
            if (moveDirection.magnitude >= 0.1f)
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
                
                // 让角色面向移动方向
                transform.forward = Vector3.Slerp(transform.forward, moveDirection, 10f * Time.deltaTime);
            }
        }
        
        // 处理与墙体的碰撞
        void OnTriggerStay(Collider other)
        {
            // 这里可以添加逻辑防止穿墙
            if (other.gameObject.CompareTag("Wall"))
            {
                // 计算玩家到墙体最近点的方向
                Vector3 directionFromWall = transform.position - other.ClosestPoint(transform.position);
                directionFromWall.y = 0; // 保持Y轴不变
                
                if (directionFromWall.magnitude > 0)
                {
                    // 将玩家推离墙体
                    transform.position += directionFromWall.normalized * 0.1f;
                }
            }
        }
    }
}
