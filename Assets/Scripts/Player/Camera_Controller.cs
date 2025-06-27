using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Camera_Controller:MonoSingleton<Camera_Controller>
{
    private Transform mTransform;
    [SerializeField] Transform target;// 跟随目标
    [SerializeField] Vector3 offset;  // 跟随偏移量
    [SerializeField] float moveSpeed; // 跟随速度
    // 初始化相机跟踪的边界
    [SerializeField] Vector2 positionXScope; // X的范围
    [SerializeField] Vector2 positionZScope; // Z的范围

    public void Init(float mapSizeOnWorld)
    {
        mTransform = transform;
    }

   

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            targetPosition.x = Mathf.Clamp(targetPosition.x, positionXScope.x, positionXScope.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, positionZScope.x, positionZScope.y);
            mTransform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }
}
