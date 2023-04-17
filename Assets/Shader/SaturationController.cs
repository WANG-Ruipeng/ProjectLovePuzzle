using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SaturationController : MonoBehaviour
{
    public float Saturation = 0f;
    private Material _originalMaterial;
    private Material _hsvMaterial;
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = spriteRenderer.material;

        // 创建一个新材质实例，以避免在所有使用此Shader的对象上应用更改
        _hsvMaterial = new Material(Shader.Find("Custom/HSVShader"));
        spriteRenderer.material = _hsvMaterial;

        // 将原始材质的纹理分配给新的HSV材质
        _hsvMaterial.SetTexture("_MainTex", _originalMaterial.mainTexture);
    }

    void Update()
    {
        // 更新材质的饱和度值
        //_material.SetFloat("_Saturation", Saturation);
    }

    public void SetSaturation(float saturation)
    {
        Saturation = saturation;
        if (_hsvMaterial != null)
            _hsvMaterial.SetFloat("Desaturation", Saturation);
    }

    private void OnDestroy()
    {
        // 销毁新创建的材质实例，以防止内存泄漏
        Destroy(_hsvMaterial);
    }
}
