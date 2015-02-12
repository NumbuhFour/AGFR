﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour {
	
	public Text topTextBox;
	public Text botTextBox;
	public int typeDelay = 10;
	
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
		messages[1] = lastMsg;
		messages[0] = head;
		
		lastMsg = head + msg;
		typingMsg = msg;
		typingIndex = 0;
		lastTime = (long)(Time.time*1000);
	}
	
	private void AddNextLetter(){
		string add = "" + typingMsg[typingIndex];
		typingIndex ++;
		
		if(add == "<"){
			while(typingMsg[typingIndex] != '>'){
				add += "" + typingMsg[typingIndex];
				typingIndex++;
			}
			if(typingIndex < typingMsg.Length -1){ // Add one more for next visible letter
				add += "" + typingMsg[typingIndex];
				typingIndex++;
			}
		}
		messages[0] += add;
		if(typingIndex > typingMsg.Length -1)
			typingMsg = "";
			
		UpdateTextBoxes();
	}
	
	private void UpdateTextBoxes(){
		botTextBox.text = messages[0];
		topTextBox.text = messages[1];
	}
}