using UnityEngine;

public class TwoPlayerCamera : MonoBehaviour
{
    // --- 在 Inspector 中设置 ---
    [Tooltip("1st player to be followed")]
    public Transform player1;

    [Tooltip("2nd player to be followed")]
    public Transform player2;

    [Tooltip("The smooth speed of the camera movement")]
    public float smoothSpeed = 0.1f;
    
    [Tooltip("The minimum view size")]
    public float minSize = 1.0f;

    [Tooltip("The miximum view size")]
    public float maxSize = 10f;

    [Tooltip("The offset to the player in z axis")]
    public float zOffset = -10f;


    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // use LateUpdate to prevent shaking
    void LateUpdate()
    {
        // ensure that the 2 players have been assigned
        if (player1 == null || player2 == null)
        {
            return; 
        }

        // --- 1. calculate the position of the camera ---
        // Find the central point of 2 players
        Vector3 centerPoint = (player1.position + player2.position) / 2f;
        // 设置相机的目标位置（中心点的X,Y，再加上固定的Z轴偏移）
        Vector3 targetPosition = new Vector3(centerPoint.x, centerPoint.y, zOffset);
        // 平滑地移动相机到目标位置
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);


        // --- 2. 计算相机缩放 (Orthographic Size) ---
        // 计算两个玩家在水平和垂直方向上的距离
        float horizontalDistance = Mathf.Abs(player1.position.x - player2.position.x);
        float verticalDistance = Mathf.Abs(player1.position.y - player2.position.y);

        // 根据水平距离和屏幕宽高比，计算所需的相机大小
        float requiredSizeByWidth = horizontalDistance * Screen.height / Screen.width * 0.5f;
        // 根据垂直距离计算所需的相机大小
        float requiredSizeByHeight = verticalDistance * 0.5f;

        // 取两者中较大的一个，确保水平和垂直方向都能完整显示
        float requiredSize = Mathf.Max(requiredSizeByWidth, requiredSizeByHeight);

        // 确保相机大小在我们设定的最小和最大值之间
        float targetSize = Mathf.Clamp(requiredSize, minSize, maxSize);

        // 平滑地调整相机的大小
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, smoothSpeed);
    }
}