using UnityEngine;

public class TextureCombiner : MonoBehaviour
{
	[SerializeField] bool useRunTime;
	[SerializeField] Renderer renderer1, renderer2, resultRenderer;

	private void Update()
	{
		if (!useRunTime) return;
		SetResultTexture();
	}

	public void StartMerge() { useRunTime = !useRunTime; }

	public void SetResultTexture()
	{
		var calc = new VariousCodes();
		Debug.Log(
		calc.CalcTime(() =>
		{

			//make texture into texture2D.
			var texture1 = ToTexture2D(renderer1.material.mainTexture);
			var texture2 = ToTexture2D(renderer2.material.mainTexture);

			//set result texture to resultRenderer.
			resultRenderer.material.mainTexture = CombineTexutes(texture1, texture2);
		})
		);
	}

	/// <summary>
	/// Combines textures from uperLeft.
	/// </summary>
	/// <param name="_textureA"></param>
	/// <param name="_textureB"></param>
	/// <returns></returns>
	public Texture2D CombineTexutes(Texture2D _textureA, Texture2D _textureB)
	{
		//create new textures
		Texture2D textureResult = new Texture2D(_textureA.width, _textureA.height);
		//create clone form texture
		textureResult.SetPixels(_textureA.GetPixels());
		//copy texture B in texutre A
		for (int x = 0; x < _textureB.width; x++)
		{
			for (int y = 0; y < _textureB.height; y++)
			{
				Color c = _textureB.GetPixel(x, y);
				if (c.a > 0.0f) //Is not transparent
				{
					//copy pixel colot in TexturaA
					textureResult.SetPixel(x, y, c);
				}
			}
		}

		//apply changes by SetPixel().
		textureResult.Apply();
		return textureResult;
	}

	/// <summary>
	/// makes texture into texture2D.
	/// </summary>
	/// <param name="my">is original texture.</param>
	/// <returns>result texture2D.</returns>
	public Texture2D ToTexture2D(Texture my)
	{
		var myWidth = my.width;
		var myHeight = my.height;
		var format = TextureFormat.RGBA32;
		var result = new Texture2D(myWidth, myHeight, format, false);
		var currentRT = RenderTexture.active;
		var rt = new RenderTexture(myWidth, myHeight, 32);
		Graphics.Blit(my, rt);
		RenderTexture.active = rt;
		var source = new Rect(0, 0, rt.width, rt.height);
		result.ReadPixels(source, 0, 0);
		result.Apply();
		RenderTexture.active = currentRT;
		return result;
	}
}
