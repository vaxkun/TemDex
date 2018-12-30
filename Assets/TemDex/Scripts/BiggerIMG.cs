using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiggerIMG : MonoBehaviour {

	public GameObject BiggerPanel;
	public Image Biggerimage;
	public Image myImage;

	public void MakeBigger()
	{
		BiggerPanel.SetActive(true);
		Biggerimage.sprite = myImage.sprite;
	}
}
