using UnityEngine;
using System.Collections;

public class ObjectsControl : MonoBehaviour {

	// Primitives
	public int primitiveNum = 20;
	public GameObject primitive;
	private GameObject[] primitives;
	private GameObject objectsParent;

	// NGUI
	private UISlider posXSlider; private UILabel posXLabel;
	private UISlider posYSlider; private UILabel posYLabel;
	private UISlider posZSlider; private UILabel posZLabel;

	private UISlider rotateXSlider; private UILabel rotateXLabel;
	private UISlider rotateYSlider; private UILabel rotateYLabel;
	private UISlider rotateZSlider; private UILabel rotateZLabel;

	private UISlider scaleXSlider; private UILabel scaleXLabel;
	private UISlider scaleYSlider; private UILabel scaleYLabel;
	private UISlider scaleZSlider; private UILabel scaleZLabel;

	private UIToggle toggleGlobalRotate;
	private UISlider threshSlider;

	private UISlider colorRSlider; private UILabel colorRLabel;
	private UISlider colorGSlider; private UILabel colorGLabel;
	private UISlider colorBSlider; private UILabel colorBLabel;
	private UISlider colorASlider; private UILabel colorALabel;

	//private UIToggle toggleFill;
	//private UIToggle toggleReverse;
	private UIToggle toggleRandom;

	public float posMin = 0.0f;
	public float posMax = 1.0f;
	public float rotateMin = 0.0f;
	public float rotateMax = 360.0f;
	public float scaleMin = 0.0f;
	public float scaleMax = 5.0f;
	public float colorMin = 0.0f;
	public float colorMax = 1.0f;

	private Vector3 pos;
	private Vector3 rotate;
	private Vector3 scale;
	private Color col;	

	// Audio Input
	private MicrophoneInput mic;

	// for Randomise
	private bool bang;
	private Slide globalRotate = new Slide();

	// Use this for initialization

	void Start () {
		
		// Primitives
		objectsParent = GameObject.Find("ObjectsParent");
		primitives = new GameObject[primitiveNum];
		for( int i = 0; i < primitiveNum; i++) {
			primitives[i] = Instantiate( primitive, new Vector3(), Quaternion.identity) as GameObject;
			primitives[i].layer = 0;
			primitives[i].transform.parent = objectsParent.transform;
		}

		// NGUI
		posXSlider = GameObject.Find( "posX" ).gameObject.GetComponent<UISlider>();
		posXLabel = GameObject.Find( "posX" ).gameObject.transform.FindChild( "Thumb" ).gameObject.transform.FindChild( "Value" ).gameObject.GetComponent<UILabel>();
		posYSlider = GameObject.Find( "posY" ).gameObject.GetComponent<UISlider>();
		posYLabel = GameObject.Find( "posY" ).gameObject.transform.FindChild( "Thumb" ).gameObject.transform.FindChild( "Value" ).gameObject.GetComponent<UILabel>();
		posZSlider = GameObject.Find( "posZ" ).gameObject.GetComponent<UISlider>();
		posZLabel = GameObject.Find( "posZ" ).gameObject.transform.FindChild( "Thumb" ).gameObject.transform.FindChild( "Value" ).gameObject.GetComponent<UILabel>();

		rotateXSlider = GameObject.Find("rotateX").gameObject.GetComponent<UISlider>();
		rotateXLabel = GameObject.Find("rotateX").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		rotateYSlider = GameObject.Find("rotateY").gameObject.GetComponent<UISlider>();
		rotateYLabel = GameObject.Find("rotateY").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		rotateZSlider = GameObject.Find("rotateZ").gameObject.GetComponent<UISlider>();
		rotateZLabel = GameObject.Find("rotateZ").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();

		scaleXSlider = GameObject.Find("scaleX").gameObject.GetComponent<UISlider>();
		scaleXLabel = GameObject.Find("scaleX").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		scaleYSlider = GameObject.Find("scaleY").gameObject.GetComponent<UISlider>();
		scaleYLabel = GameObject.Find("scaleY").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		scaleZSlider = GameObject.Find("scaleZ").gameObject.GetComponent<UISlider>();
		scaleZLabel = GameObject.Find("scaleZ").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();

		GameObject _toggleGlobalRotate = GameObject.Find("bGlobalRotate").gameObject;
		toggleGlobalRotate = _toggleGlobalRotate.GetComponent<UIToggle>();

		threshSlider = GameObject.Find("thresh").gameObject.GetComponent<UISlider>();

		colorRSlider = GameObject.Find("colorR").gameObject.GetComponent<UISlider>();
		colorRLabel = GameObject.Find("colorR").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		colorGSlider = GameObject.Find("colorG").gameObject.GetComponent<UISlider>();
		colorGLabel = GameObject.Find("colorG").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		colorBSlider = GameObject.Find("colorB").gameObject.GetComponent<UISlider>();
		colorBLabel = GameObject.Find("colorB").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();
		colorASlider = GameObject.Find("colorA").gameObject.GetComponent<UISlider>();
		colorALabel = GameObject.Find("colorA").gameObject.transform.FindChild("Thumb").gameObject.transform.FindChild("Value").gameObject.GetComponent<UILabel>();


		//GameObject _toggleFill = GameObject.Find("bFill").gameObject;
		//toggleFill = _toggleFill.GetComponent<UIToggle>();
		//GameObject _toggleReverse = GameObject.Find("bReverseTog").gameObject;
		//toggleReverse = _toggleReverse.GetComponent<UIToggle>();
		GameObject _toggleRandom = GameObject.Find("bRandom").gameObject;
		toggleRandom = _toggleRandom.GetComponent<UIToggle>();

		mic = GameObject.Find("AudioInput").gameObject.GetComponent<MicrophoneInput>();
	}
	
	// Update is called once per frame
	void Update () {

		// UIの表示非表示
		if (Input.GetKeyDown ("h")) {
			GameObject uiCamera = GameObject.Find("UI Root").gameObject.transform.FindChild("Camera").gameObject;
			if( uiCamera.activeSelf == false ) uiCamera.SetActive (true);
			else uiCamera.SetActive (false);
		}

		float vol = mic.loudness;
		float thresh = threshSlider.value;

		if (bang == false && vol > thresh)
		{
			// 新しくbangが入ったら
			bang = true;
			if (toggleRandom.value) randomise();
			if (toggleGlobalRotate.value)
			{
				globalRotate.set(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			}
		}

		// オブジェクト全体の回転
		if (!toggleGlobalRotate.value) globalRotate.set(0, 0, 0);
		globalRotate.update();
		objectsParent.transform.eulerAngles = globalRotate.v;

		if (vol <= thresh) bang = false;

		// 値の同期
		UpdateValues();

		// 各Primitiveを、移動・回転・拡縮 する
		for(int i = 0; i < primitives.Length; i++)
		{
			float x = (i - primitives.Length * 0.5f) * vol * pos.x;
			float y = (i - primitives.Length * 0.5f) * vol * pos.y;
			float z = (i - primitives.Length * 0.5f) * vol * pos.z;
			primitives[i].transform.localPosition = new Vector3(x,y,z);

			x = i * vol * 0.1f * rotate.x;
			y = i * vol * 0.1f * rotate.y;
			z = i * vol * 0.1f * rotate.z;
			primitives[i].transform.localRotation = Quaternion.Euler(new Vector3(x, y, z));

			x = i * vol * scale.x + 1;
			y = i * vol * scale.y + 1;
			z = i * vol * scale.z + 1;
			primitives[i].transform.localScale = new Vector3( x, y, z );

			float r = col.r;
			float g = col.g;
			float b = col.b;
			float a = col.a;
			Color color = new Color(r, g, b, a);
			primitives[i].GetComponent<wireframeRenderer>().lineColor = color;
		}
	}

	void randomise()
	{
		// Position
		int rand = (int)(5.0f * Random.value);
		if (rand == 0)
		{
			pos.x = 0.33f * posMax; pos.y = 0.0f; pos.z = 0.0f;
		}
		else if (rand == 1)
		{
			pos.x = 0.0f; pos.y = 0.33f * posMax; pos.z = 0.0f;
		}
		else if (rand == 2)
		{
			pos.x = 0.0f; pos.y = 0.0f; pos.z = 0.33f * posMax;
		}
		else
		{
			pos.x = Random.Range( 0.33f, 0.66f ) * posMax; pos.y = Random.Range( 0.33f, 0.66f ) * posMax; pos.z = Random.Range( 0.33f, 0.66f ) * posMax;
		}

		// Rotate
		rand = (int)(5.0f * Random.value);
		if (rand == 0)
		{
			rotate.x = 1.0f * rotateMax; rotate.y = 0.0f; rotate.z = 0.0f;
		}
		else if (rand == 1)
		{
			rotate.x = 0.0f; rotate.y = 1.0f * rotateMax; rotate.z = 0.0f;
		}
		else if (rand == 2)
		{
			rotate.x = 0.0f; rotate.y = 0.0f; rotate.z = 1.0f * rotateMax;
		}
		else
		{
			rotate.x = Random.Range(0.33f, 0.66f) * rotateMax; rotate.y = Random.Range(0.33f, 0.66f) * rotateMax; rotate.z = Random.Range(0.33f, 0.66f) * rotateMax;
		}

		// Scale
		rand = (int)(5.0f * Random.value);
		if (rand == 0)
		{
			scale.x = Random.Range(0.33f, 0.66f) * scaleMax; scale.y = 0.0f; scale.z = 0.0f;
		}
		else if (rand == 1)
		{
			scale.x = 0.0f; scale.y = Random.Range(0.33f, 0.66f) * scaleMax; scale.z = 0.0f;
		}
		else if (rand == 2)
		{
			scale.x = 0.0f; scale.y = 0.0f; scale.z = Random.Range(0.33f, 0.66f) * scaleMax;
		}
		else
		{
			scale.x = Random.Range(0f, 1.0f) * scaleMax; scale.y = Random.Range(0f, 1.0f) * scaleMax; scale.z = Random.Range(0f, 1.0f) * scaleMax;
		}
	}

	private void UpdateValues()
	{
		if (toggleRandom.value)
		{
			posXSlider.value = Mathf.InverseLerp(posMin, posMax, pos.x);
			posYSlider.value = Mathf.InverseLerp(posMin, posMax, pos.y);
			posZSlider.value = Mathf.InverseLerp(posMin, posMax, pos.z);

			rotateXSlider.value = Mathf.InverseLerp(rotateMin, rotateMax, rotate.x);
			rotateYSlider.value = Mathf.InverseLerp(rotateMin, rotateMax, rotate.y);
			rotateZSlider.value = Mathf.InverseLerp(rotateMin, rotateMax, rotate.z);

			scaleXSlider.value = Mathf.InverseLerp(scaleMin, scaleMax, scale.x);
			scaleYSlider.value = Mathf.InverseLerp(scaleMin, scaleMax, scale.y);
			scaleZSlider.value = Mathf.InverseLerp(scaleMin, scaleMax, scale.z);
		}
		else
		{
			pos.x = Mathf.Lerp(posMin, posMax, posXSlider.value);
			pos.y = Mathf.Lerp(posMin, posMax, posYSlider.value);
			pos.z = Mathf.Lerp(posMin, posMax, posZSlider.value);

			rotate.x = Mathf.Lerp(rotateMin, rotateMax, rotateXSlider.value);
			rotate.y = Mathf.Lerp(rotateMin, rotateMax, rotateYSlider.value);
			rotate.z = Mathf.Lerp(rotateMin, rotateMax, rotateZSlider.value);

			scale.x = Mathf.Lerp(scaleMin, scaleMax, scaleXSlider.value);
			scale.y = Mathf.Lerp(scaleMin, scaleMax, scaleYSlider.value);
			scale.z = Mathf.Lerp(scaleMin, scaleMax, scaleZSlider.value);
		}

		col.r = Mathf.Lerp(colorMin, colorMax, colorRSlider.value);
		col.g = Mathf.Lerp(colorMin, colorMax, colorGSlider.value);
		col.b = Mathf.Lerp(colorMin, colorMax, colorBSlider.value);
		col.a = Mathf.Lerp(colorMin, colorMax, colorASlider.value);
		
		posXLabel.text = pos.x.ToString();
		posYLabel.text = pos.y.ToString();
		posZLabel.text = pos.z.ToString();

		rotateXLabel.text = rotate.x.ToString();
		rotateYLabel.text = rotate.y.ToString();
		rotateZLabel.text = rotate.z.ToString();

		scaleXLabel.text = scale.x.ToString();
		scaleYLabel.text = scale.y.ToString();
		scaleZLabel.text = scale.z.ToString();

		colorRLabel.text = col.r.ToString();
		colorGLabel.text = col.g.ToString();
		colorBLabel.text = col.b.ToString();
		colorALabel.text = col.a.ToString();
	}

	public class Slide {
		public Vector3 v;
		private Vector3 _v;
		private float speed;

		public Slide()
		{
			v = new Vector3(0, 0, 0);
			_v = new Vector3(0, 0, 0);
			speed = 0.1f;
		}

		public void set(float _px, float _py, float _pz)
		{
			_v.x = _px; _v.y = _py; _v.z = _pz;
		}

		public void update()
		{
			v.x += (_v.x - v.x) * speed;
			v.y += (_v.y - v.y) * speed;
			v.z += (_v.z - v.z) * speed;
		}
	}
}
