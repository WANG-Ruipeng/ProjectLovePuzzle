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

        // ����һ���²���ʵ�����Ա���������ʹ�ô�Shader�Ķ�����Ӧ�ø���
        _hsvMaterial = new Material(Shader.Find("Custom/HSVShader"));
        spriteRenderer.material = _hsvMaterial;

        // ��ԭʼ���ʵ����������µ�HSV����
        _hsvMaterial.SetTexture("_MainTex", _originalMaterial.mainTexture);
    }

    void Update()
    {
        // ���²��ʵı��Ͷ�ֵ
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
        // �����´����Ĳ���ʵ�����Է�ֹ�ڴ�й©
        Destroy(_hsvMaterial);
    }
}
