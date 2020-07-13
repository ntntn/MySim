using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEffect : MonoBehaviour
{


    private Vector3 Offset = new Vector3(-0.15f, -0.15f);
    private Material material;
    private GameObject _shadow;
    // Start is called before the first frame update
    void Start()
    {
        material = Resources.Load<Material>("Shadow Effect/ShadowMat");

        _shadow = new GameObject("Shadow");
        _shadow.transform.parent = transform;

        _shadow.transform.localPosition = Offset;
        _shadow.transform.localRotation = Quaternion.identity;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = _shadow.AddComponent<SpriteRenderer>();
        sr.sprite = renderer.sprite;
        sr.material = material;

        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        _shadow.transform.localPosition = Offset;
    }

}
