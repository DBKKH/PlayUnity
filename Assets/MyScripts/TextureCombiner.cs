using UnityEngine;

public class TextureCombiner : MonoBehaviour
{
	public Renderer baseTexture1, baseTexture2, textureResult;

	public void Merge(/*Texture2D texture1, Texture2D texture2*/)
	{
		//baseTexture1 = texture1;
		//baseTexture2 = texture2;

		this.textureResult.material.mainTexture = AddWatermark(ToTexture2D(baseTexture1.material.mainTexture), ToTexture2D(baseTexture2.material.mainTexture));
	}

	public Texture2D AddWatermark(Texture2D background, Texture2D watermark)
	{
		int startX = 0;
		int startY = background.height - watermark.height;

		for (int x = startX; x < background.width; x++)
		{

			for (int y = startY; y < background.height; y++)
			{
				Color bgColor = background.GetPixel(x, y);
				Color wmColor = watermark.GetPixel(x - startX, y - startY);

				Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

				background.SetPixel(x, y, final_color);
			}
		}

		background.Apply();
		return background;
	}

	public Texture2D CombineTexutes(Texture2D _textureA, Texture2D _textureB)
	{
		//Create new textures
		Texture2D textureResult = new Texture2D(_textureA.width, _textureA.height);
		//create clone form texture
		textureResult.SetPixels(_textureA.GetPixels());
		//Now copy texture B in texutre A
		for (int x = 0; x < _textureB.width; x++)
		{
			for (int y = 0; y < _textureB.height; y++)
			{
				Color c = _textureB.GetPixel(x, y);
				if (c.a > 0.0f) //Is not transparent
				{
					//Copy pixel colot in TexturaA
					textureResult.SetPixel(x, y, c);
				}
			}
		}
		//Apply colors
		textureResult.Apply();
		return textureResult;
	}

	public Texture2D ToTexture2D(Texture self)
	{
		var sw = self.width;
		var sh = self.height;
		var format = TextureFormat.RGBA32;
		var result = new Texture2D(sw, sh, format, false);
		var currentRT = RenderTexture.active;
		var rt = new RenderTexture(sw, sh, 32);
		Graphics.Blit(self, rt);
		RenderTexture.active = rt;
		var source = new Rect(0, 0, rt.width, rt.height);
		result.ReadPixels(source, 0, 0);
		result.Apply();
		RenderTexture.active = currentRT;
		return result;
	}
}
