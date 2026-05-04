using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;

    [SerializeField] GameObject button;
    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;
    [SerializeField] Animator animator;
    [SerializeField] EventSystem eventSystem;

    LinkedList<Dialogue> dialogueQueue;

    [HideInInspector] public bool endDialogue = true;

    string currentDialogue;

    IEnumerator typeCoroutine;

    bool dialogueOn = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start () {
        if(instance != this)
            return;
        eventSystem = FindObjectOfType<EventSystem>();
        dialogueQueue = new LinkedList<Dialogue>();
        InputManager.OnPressA += PressContinue;
	}

    public void StartDialogue(Dialogue[] dialogues)
    {
        //button.SetActive(true);        
        endDialogue = false;
        animator.SetBool("IsOpen", true);
        dialogueOn = true;

        dialogueQueue.Clear();

        foreach (Dialogue dialogue in dialogues)
        {
            dialogueQueue.AddLast(dialogue);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        eventSystem.SetSelectedGameObject(button);
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        Dialogue dialogue = dialogueQueue.First();
        dialogueQueue.RemoveFirst();
        nameText.text = dialogue.name; // -> Set name field
        dialogueText.text = dialogue.sentence; // -> Set dialogue text

        // Lets break current sentence into pieces that fit the text box!
        string[] sentenceThatFit = TextSplitter(dialogue.sentence);
        //StartCoroutine(TextSplitter(dialogue.sentence));
        if(!string.IsNullOrEmpty(sentenceThatFit[1]))
            dialogueQueue.AddFirst(new Dialogue(dialogue.name, sentenceThatFit[1]));

        currentDialogue = sentenceThatFit[0];
        
        if(typeCoroutine != null) // -> If still typing, complete this sentence before continue;
        {
            StopCoroutine(typeCoroutine);
        }
        else
        {
            //typeCoroutine = TypeSentence(dialogue.sentence);
            typeCoroutine = TypeSentence(sentenceThatFit[0]);
            StartCoroutine(typeCoroutine);
        }

    }

    IEnumerator TypeSentence (string sentence)
    {
        

        dialogueText.text = "";
    
        yield return new WaitForSeconds(0.2f);

        //var punctuation = sentence.Where(Char.IsPunctuation).Distinct().ToArray();
        var words = sentence.Split().Select(x => x.Trim(' '));
        bool addSpace = false;
        bool firstInt = true;

        foreach(string word in words)
        {

            int thisIndex = dialogueText.text.Length;
            //if(addSpace)
            //if(thisIndex != 0)
            dialogueText.text += " ";
            dialogueText.text += word;
            dialogueText.CalculateLayoutInputHorizontal();
            int splitIndex = dialogueText.text.LastIndexOf(' ');
            if(splitIndex != -1)
            {
                if(dialogueText.preferredWidth > dialogueText.rectTransform.rect.width)
                {
                    dialogueText.text = dialogueText.text.Substring(0, splitIndex);
                    dialogueText.text += Environment.NewLine;
                    addSpace = false;
                }
                else
                {
                    dialogueText.text = dialogueText.text.Substring(0, splitIndex);
                    addSpace = true;
                }
            }
            if(addSpace && !firstInt)
                dialogueText.text += " ";
            foreach (char letter in word.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.02f);
            }
            firstInt = false;
        }
        typeCoroutine = null;
    }

    public void EndDialogue()
    {
        button.SetActive(false);
        animator.SetBool("IsOpen", false);
        dialogueOn = false;
        endDialogue = true;
    }


    private string[] TextSplitter(string input)
    {
        //List<string> brokenSentence = new List<string>();
        string[] output = new string[2];
        dialogueText.text = input;
        dialogueText.horizontalOverflow = HorizontalWrapMode.Wrap;
        dialogueText.verticalOverflow = VerticalWrapMode.Truncate;
        dialogueText.CalculateLayoutInputVertical();
        //print(dialogueText.preferredHeight);
        string nextSentence = "";
        int loop = 0;
        while(dialogueText.preferredHeight > dialogueText.rectTransform.rect.height)
        {
            print(loop);
            int splitIndex = dialogueText.text.LastIndexOf(' ');
            nextSentence = nextSentence.Insert(0, dialogueText.text.Substring(splitIndex));
            dialogueText.text = dialogueText.text.Substring(0, splitIndex);
            dialogueText.CalculateLayoutInputVertical();
            loop ++;
            print(nextSentence);
            print(dialogueText.preferredHeight > dialogueText.rectTransform.rect.height);
            //yield return null;
        }
        output[0] = dialogueText.text;
        nextSentence = nextSentence.TrimStart(' ');
        output[1] = nextSentence;

        return output;
    }

    private void PressContinue()
    {
        if(!dialogueOn)
            return;
        if(typeCoroutine != null) // -> If still typing, complete this sentence before continue.
        {
            StopCoroutine(typeCoroutine);
            typeCoroutine = null;
            dialogueText.text = currentDialogue;
        }
        else // -> Else will pick next sentence.
        {
            DisplayNextSentence();
        }
    }

}
