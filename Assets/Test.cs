using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public HyphenationJpn hypJpn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10,10,200,200));
		
//		RectTransform rectTrans = hypJpn.GetComponent<RectTransform>();
//		Rect rect = rectTrans.rect;
//		rect.width = GUILayout.HorizontalSlider(rect.width, 100, 600);
//		rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);

		GUILayout.Label("width");
		hypJpn.textWidth = GUILayout.HorizontalSlider(hypJpn.textWidth, 100, 600);
		GUILayout.Label("fontSize");
		hypJpn.fontSize = (int)GUILayout.HorizontalSlider(hypJpn.fontSize, 10, 40);

		GUILayout.Space(20);

		if(GUILayout.Button("ChangeText")){
			string sampleText = "Unityマニュアルガイドは特定のプラットフォームにのみ適用されるセクションを含みます。\n自ら参照したいセクションを選択して下さい。プロットフォーム特有の情報は各ページの三角形の記号で示されるボタンにより展開して参照することが出来ます。";
			hypJpn.SetText(sampleText);
		}

		GUILayout.EndArea();
	}
}
