using UnityEngine;
using System.Collections;

public class BG_Scroller : MonoBehaviour {

    public float scrollSpeed;

    Vector2 offset;
    float lastX;

	// Use this for initialization
	void Start ()
    {
        offset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
        lastX = offset.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float x = Mathf.Repeat(Time.deltaTime * scrollSpeed + lastX, 1);
        Vector2 newOffSet = new Vector2(x, offset.y);
        lastX = newOffSet.x;
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", newOffSet);
    }

    void OnDisable()
    {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
