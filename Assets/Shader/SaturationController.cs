using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SaturationController : MonoBehaviour
{
    public float Saturation = 0f;
    private Material _material;

    void Start()
    {
        // 创建一个新材质实例，以避免在所有使用此Shader的对象上应用更改
        _material = new Material(Shader.Find("Custom/HSVShader"));
        //GetComponent<SpriteRenderer>().material = _material;
    }

    void Update()
    {
        // 更新材质的饱和度值
        //_material.SetFloat("_Saturation", Saturation);
    }

    public void SetSaturation(float sat)
    {
        Saturation = sat;
        if(_material != null)
        {
            _material.SetFloat("_Saturation", Saturation);
        }
        
    }

    private void OnDestroy()
    {
        // 销毁新创建的材质实例，以防止内存泄漏
        Destroy(_material);
    }
}