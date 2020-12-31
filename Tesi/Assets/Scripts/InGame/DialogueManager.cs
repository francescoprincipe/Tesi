using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Dialogue questDialogue;
	public Dialogue finalDialogue;

	private Dialogue activeDialogue;

	public GameObject dialogueCanvas;
	public GameObject dialogueTextArea;
	public GameObject buttonTextObject;

	private TextMeshProUGUI dialogueText;
	private TextMeshProUGUI buttonText;

	private Queue<string> sentences;

	private void Awake()
	{
		sentences = new Queue<string>();
		dialogueText = dialogueTextArea.GetComponent<TextMeshProUGUI>();
		buttonText = buttonTextObject.GetComponent<TextMeshProUGUI>();
		if (activeDialogue == null)
			activeDialogue = finalDialogue;
	}
	void OnEnable () {
		buttonText.text = "CONTINUA...";
		StartDialogue(activeDialogue);
	}

	public void StartDialogue (Dialogue dialogue)
	{

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if(sentences.Count == 1)
		{
			buttonText.text = "CONCLUDI";
		}

		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		int maxChar = sentence.Length;
		dialogueText.text = sentence;
		int counter = 0;

		while(counter <= maxChar)
		{
			dialogueText.maxVisibleCharacters = counter;
			counter++;
			yield return new WaitForSeconds(0.03f);
		}

	}

	void EndDialogue()
	{
		Debug.Log("End dialogue");
		dialogueCanvas.SetActive(false);
	}

	public void SetDialogue(bool questCompleted)
	{
		activeDialogue = questCompleted ? finalDialogue : questDialogue;
	}

}
