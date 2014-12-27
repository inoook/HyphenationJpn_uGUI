using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class HyphenationJpn : MonoBehaviour
{
	// http://answers.unity3d.com/questions/424874/showing-a-textarea-field-for-a-string-variable-in.html
	[TextArea(3,10)]
	public string text;

	private float spaceSize;

	public bool updateEditorOnly = true;

	void Update()
	{
		if (updateEditorOnly && Application.isPlaying){ return; } // run only editor

		UpdateText(text);

	}

	void UpdateText(string str)
	{
		// update Text
		Text textComp = this.gameObject.GetComponent<Text>();
		textComp.text = SetText(textComp, str);
	}
	
	public void SetText(string str)
	{
		text = str;
		UpdateText(text);
	}


	string SetText(Text textComp, string msg)
	{
		if(msg == "" || msg == null){
			return "";
		}
		
		float w = textComp.GetComponent<RectTransform>().rect.width;
		
		// get space width
		textComp.text = "m m";
		float tmp0 = textComp.preferredWidth;
		textComp.text = "mm";
		float tmp1 = textComp.preferredWidth;
		spaceSize = (tmp0 - tmp1);
		
		// override
		textComp.horizontalOverflow = HorizontalWrapMode.Overflow;


		// work
		string str = "";

		List<string> wordList = GetWordList(msg);
		
		float lineW = 0;
		for(int i = 0; i < wordList.Count; i++){

			textComp.text = wordList[i];
			lineW += textComp.preferredWidth;

			if(wordList[i] == NEWLINE_CHARA){
				lineW = 0;
			}else{
				if(wordList[i] == " "){
					lineW += spaceSize;
				}
				if(lineW > w){
					str += NEWLINE_CHARA;
					textComp.text = wordList[i];
					lineW = textComp.preferredWidth;
				}
			}
			str += wordList[i];
		}
		
		return str;
	}

	private List<string> GetWordList(string tmpText)
	{
		List<string> words = new List<string>();
		
		string word = "";
		for(int j = 0; j < tmpText.Length; j ++){

			string str = tmpText[j].ToString();//single Charactor
			string nextStr = "";
			if(j < tmpText.Length-1){
				nextStr = tmpText[j+1].ToString();
			}
			string preStr = "";
			if(j > 0){
				preStr = tmpText[j-1].ToString();
			}

			if( IsLatin(str) && !IsLatin(preStr) ){
				words.Add(word);
				word = "";
			}

			word += str;
			
			if( !IsLatin(str) && CHECK_HYP_BACK(preStr) ){
				words.Add(word);
				word = "";
			}

			if( (!IsLatin(nextStr) && !CHECK_HYP_FRONT(nextStr) && !CHECK_HYP_BACK(str)) ){
				words.Add(word);
				word = "";
			}

			if(j == tmpText.Length - 1){
				// end
				words.Add(word);
			}
		}
		return words;
	}

	// helper
	public float textWidth{
		set{
			RectTransform rectTrans = this.GetComponent<RectTransform>();
			rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
		}
		get{
			RectTransform rectTrans = this.GetComponent<RectTransform>();
			return rectTrans.rect.width;
		}
	}
	public int fontSize
	{
		set{
			Text textComp = this.gameObject.GetComponent<Text>();
			textComp.fontSize = value;
		}
		get{
			Text textComp = this.gameObject.GetComponent<Text>();
			return textComp.fontSize;
		}
	}

	// static

	private static string NEWLINE_CHARA = "\n";

	// 禁則処理 http://ja.wikipedia.org/wiki/%E7%A6%81%E5%89%87%E5%87%A6%E7%90%86
	// 行頭禁則文字
	private static string HYP_FRONT = ",)]｝、。）〕〉》」』】〙〗〟’”｠»" +// 終わり括弧類 簡易版
		"ァィゥェォッャュョヮヵヶっぁぃぅぇぉっゃゅょゎ" +//行頭禁則和字 
			"‐゠–〜ー" +//ハイフン類
			"?!‼⁇⁈⁉" +//区切り約物
			"・:;" +//中点類
			"。.";//句点類
	
	private static bool CHECK_HYP_FRONT(string str)
	{
		return HYP_FRONT.Contains(str);
	}
	
	private static string HYP_BACK = "([｛〔〈《「『【〘〖〝‘“｟«";//始め括弧類

	private static bool CHECK_HYP_BACK(string str)
	{
		return HYP_BACK.Contains(str);
	}

	private static bool IsLatin(string s)
	{
		return System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z0-9<>().,]+$");
	}
}