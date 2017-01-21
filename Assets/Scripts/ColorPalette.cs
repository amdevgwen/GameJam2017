using UnityEngine;
using System.Collections;

public class ColorPalette : MonoBehaviour
{

	/**Singleton-like reference to the ColorPalette component.
	 */
	public static ColorPalette cp;

	//Change these as needed by your palette
	/**Higher numbers are lighter.
	 * 0s are black replacements.
	 * 1s are the most saturated.
	 * 2s are the base colors.
	 * 3s are bright!
	 * 4s are pale (great for text). */
	public Color primary0, primary1, primary2, primary3, primary4, green0, green1, green2, green3, green4, blue0, blue1, blue2, blue3, blue4, complementary0, complementary1, complementary2, complementary3, complementary4;

	public static string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	public static string ColorText(Color32 color, string text)
	{
		return "<color=#" + ColorToHex(color) + ">" + text + "</color>";
	}

	void Awake()
	{
		//Set the CP
		if (cp == null)
		{
			cp = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (cp != this)
		{
			Destroy(gameObject);
		}
	}

}
