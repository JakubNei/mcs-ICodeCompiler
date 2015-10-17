using UnityEngine;

// namespace doesnt matter, its here just for clarity
namespace RuntimeCSharpCompiler
{

    // it can even be the same as already existing type, what matters is its in different module
    public class TestLoadScript : MonoBehaviour
    {

        void Start()
        {
            Debug.Log("I live, Iam script loaded and parsed by mcs during Unity runtime. Yay this is too epic.");
        }


        float lastColorChageTime = 0;
        Color targetColor;
        void Update()
        {

            MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
            if (renderer)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime*1);
                if (lastColorChageTime + 1 < Time.realtimeSinceStartup)
                {
                    lastColorChageTime = Time.realtimeSinceStartup;
                    targetColor = new Color(Random.value, Random.value, Random.value);
                }
            }
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, 50f * Time.deltaTime, 0);
        }
    }

}