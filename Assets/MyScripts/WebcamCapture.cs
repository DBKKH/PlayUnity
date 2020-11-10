//http://unitygeek.hatenablog.com/entry/2017/04/18/093734

using UnityEngine;

public class WebcamCapture : MonoBehaviour
{
	WebCamTexture webcamTexture;
	public Renderer _renderer;

	Quaternion baseRotation;

	void Start()
	{
		baseRotation = transform.rotation;

		this.Init();

		StartWebcam();
	}


	//void Update()
	//{
	//	this.transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
	//}
	// https://nil-one.com/blog/article/2018/01/30/imageanalysisusingunity03/

	void Init()
	{
		webcamTexture = new WebCamTexture();

		if (_renderer == null)
			_renderer = GetComponent<Renderer>();
	}

	public void StartWebcam()
	{
		_renderer.material.mainTexture = webcamTexture;    //mainTextureにWebCamTextureを指定
		webcamTexture.Play();  //ウェブカムを作動させる
	}

}