using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[AddComponentMenu("Scripts/UI/Chat Manager")]
public class ChatManager : MonoBehaviour {
	public Text[] boxes;
	public int typeDelay = 10;
	public int maxLength = 35;
	
	private string lastMsg = "";
	private string[] messages = {"",""};
	private string typingMsg = "";
	private int typingIndex = 0;
	
	private long lastTime;
	// Use this for initialization
	void Start () {
		lastTime = (long)(Time.time*1000);
		UpdateTextBoxes();
	}
	
	// Update is called once per frame
	void Update () {
		long time = (long)(Time.time*1000);
		if(time - lastTime >= typeDelay){
			lastTime = time;
			if(typingMsg != "") AddNextLetter();
		}
	}
	
	public void PushText(string head, string msg){
		while (typingIndex < typingMsg.Length-1){ //Finish typing
			AddNextLetter();
		}
		messages[1] = messages[0];
		messages[0] = head;
		
		lastMsg = head + msg;
		typingMsg = msg;
		typingIndex = 0;
		lastTime = (long)(Time.time*1000);
	}
	
	private void AddNextLetter(){
		string add = "" + typingMsg[typingIndex];
		typingIndex ++;
		
		/*if(add == "<"){
			while(typingMsg[typingIndex] != '>'){
				add += "" + typingMsg[typingIndex];
				typingIndex++;
			}
			if(typingIndex < typingMsg.Length -1){ // Add one more for next visible letter
				add += "" + typingMsg[typingIndex];
				typingIndex++;
			}
		}*/
		messages[0] += add;
		if(typingIndex >= maxLength && typingIndex != typingMsg.Length-1){
			messages[1] = messages[0];
			messages[0] = "";
			typingMsg = typingMsg.Substring(typingIndex);
			typingIndex = 0;
		}
		if(typingIndex > typingMsg.Length -1)
			typingMsg = "";
			
		UpdateTextBoxes();
	}
	
	private void UpdateTextBoxes(){
		for(int i = 0; i < boxes.Length && i < messages.Length; i++){
			boxes[i].text = messages[i];
		}
	}
}
