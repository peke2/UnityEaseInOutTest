using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ease : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CreateLineMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (lineMaterial) return;

        // Unity has a built-in shader that is useful for drawing
        // simple colored things.
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);
    }

    void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        List<Vector3> list;

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        list = createEase(easeIn);
        drawPoints(list, new Vector3(0,0,0), new Color(1, 0, 0, 1));

        list = createEase(easeOut);
        drawPoints(list, new Vector3(0, 0.5f, 0), new Color(0, 1, 0, 1));

        list = createEase(easeInOut);
        drawPoints(list, new Vector3(0, 1.0f, 0), new Color(0, 0, 1, 1));

        GL.PopMatrix();
    }

    void drawPoints(List<Vector3> list, Vector3 offset, Color color)
    {
        foreach (var v in list)
        {
            GL.PushMatrix();

            Matrix4x4 mtx = Matrix4x4.Translate(v + offset);

            GL.MultMatrix(mtx);

            // Draw lines
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex3(-0.1f, 0, 0);
            GL.Vertex3(0.1f, 0, 0);
            GL.Vertex3(0, -0.1f, 0);
            GL.Vertex3(0, 0.1f, 0);
            GL.End();

            GL.PopMatrix();
        }
    }

    delegate float EaseFunc(float t, float b, float c, float d);

    List<Vector3> createEase(EaseFunc func)
    {
        var list = new List<Vector3>();
        var t = 0.0f;
        for(int i=0; i<=10; i++)
        {
            var v = func(t, 0, 5, 1);
            list.Add(new Vector3(v, 0, 0));
            t += 0.1f;
        }
        return list;
    }


    float easeIn(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t + b;
    }

    float easeOut(float t, float b, float c, float d)
    {
        t /= d;
        return -c * t * (t-2) + b;
    }

    float easeInOut(float t, float b, float c, float d)
    {
        t /= d/2;

        if (t < 1)
        {
            return c / 2 * t * t + b;
        }

        t -= 1;

        return -c / 2 * (t * (t - 2)-1) + b;
    }

}
