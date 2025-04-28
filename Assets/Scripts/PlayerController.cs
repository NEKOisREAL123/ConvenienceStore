using UnityEngine;

namespace ConvenienceStore
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        
        private BoxCollider boxCollider;
        private Vector3 moveDirection;

        private void Start()
        {
            // 获取或添加BoxCollider
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }
            
            // 设置BoxCollider属性
            boxCollider.center = new Vector3(0, 0.01f, 0);
            boxCollider.size = new Vector3(0.01f, 0.02f, 0.01f);
            
            // 将BoxCollider设置为触发器，这样不会被物理碰撞反弹
            boxCollider.isTrigger = true;
        }

        private void Update()
        {
            // 处理输入
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            
            // 移动玩家（使用Transform.position而不是Rigidbody，避免物理碰撞）
            if (moveDirection.magnitude >= 0.1f)
            {
                // 使用Translate方法移动，而不是直接修改position
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
                
                // 旋转角色面向移动方向
                if (moveDirection != Vector3.zero)
                {
                    transform.forward = Vector3.Slerp(transform.forward, moveDirection, 10 * Time.deltaTime);
                }
            }
        }

        // 触发器碰撞检测，可以手动处理碰撞响应
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Player触碰到: " + other.name);
        }

        private void OnTriggerStay(Collider other)
        {
            // 如果需要，可以添加更复杂的碰撞响应
            // 例如，检测墙壁并阻止玩家穿过
            if (other.CompareTag("Wall"))
            {
                // 获取碰撞点最近的点
                Vector3 closestPoint = other.ClosestPoint(transform.position);
                Vector3 direction = (transform.position - closestPoint).normalized;
                
                // 仅沿水平方向移动
                direction.y = 0;
                
                // 轻微推开玩家，避免穿墙
                if (direction != Vector3.zero)
                {
                    transform.position += direction * 0.05f;
                }
            }
        }
    }
}
