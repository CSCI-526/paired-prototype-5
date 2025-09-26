using UnityEngine;

// 确保游戏对象上有一个 Rigidbody2D 组件
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerArrowController : MonoBehaviour
{
    // 公开变量，用于在 Inspector 窗口中设置移动速度
    public float speed = 5.0f;

    public float jumpForce = 10.0f;

    //二段跳代码
    public int maxJumps = 2; // 新增：最大跳跃次数（设为2就是二段跳）
    private int jumpCount; // 新增：当前剩余的跳跃次数

    // 私有变量，用于引用 Rigidbody2D 组件
    private Rigidbody2D rb;


    // Start is called before the first frame update
    private void Start()
    {
        // 获取附加到此游戏对象的 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps; // 游戏开始时，充满跳跃次数
    }
    

    // Update is called once per frame
    void Update()
    {
        // 检查玩家是否按下了空格键
        // 并且剩余跳跃次数 > 0
        if (Input.GetKeyDown(KeyCode.W) && jumpCount > 0)
        {
            // 1. 在施加力之前，先把当前的垂直速度清零
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            // 2. 施加一个向上的瞬间冲力
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            // 3. 消耗一次跳跃次数
            jumpCount--;
        }
    }

    // Handle physics-based movement.
    private void FixedUpdate()
    {
        // 1. 获取输入
        // 创建一个浮点数来存储水平移动方向 (-1 for left, 1 for right, 0 for none)
        float moveHorizontal = 0f;

        // 检测是否按下了右箭头键
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
        }
        // 如果没有按右箭头，再检测是否按下了左箭头键
        else if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
        }

        // 2. 应用移动
        // 修改刚体的速度来实现移动
        // X 轴速度 = 方向 * 速度
        // Y 轴速度 = 保持当前的垂直速度 (这样重力才能正常工作！)
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    //跳跃次数重置
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 当接触到地面时，重置跳跃次数
            jumpCount = maxJumps;
        }
    }
}