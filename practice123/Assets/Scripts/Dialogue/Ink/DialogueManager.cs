using System.Collections;
using System.Collections.Generic;
//using Ink.Parsed;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;

    [Header("Choices UI")]
    public GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public Story currentStory { get; private set; }
    public bool dialogueIsPlaying { get; private set; }
    private static DialogueManager instance;

    public UnityEvent onDialogueEnd;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    //初始關掉所有對話框相關物件
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //get all of choices
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }



    }

    private void Update()
    {
        //return right away if dialogue is not playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        //handle nextline when submit button is pressed
        if (Input.GetButtonDown("Submit"))
        {
            continueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {

        //Debug.Log("EnterDialogueMode");
        if (playerController != null)
        {
            playerController.enabled = false;  // 進入對話時停用玩家移動
        }
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //更新變數
        InkVariableUpdater inkUpdater = FindObjectOfType<InkVariableUpdater>();
        if (inkUpdater != null)
        {
            //Debug.Log("進入對話時更新暫存變數");
            inkUpdater.SetCurrentStory(currentStory);
            inkUpdater.ApplyTempVariables();
        }


        continueStory();

    }
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        playerController.enabled = true;
    }

    private void continueStory()
    {

        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            dialogueText.text = currentStory.Continue();
            // display the choices
            DisplayChoices();
            //handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
            onDialogueEnd?.Invoke(); // 觸發對話結束事件
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        //loop for each tag 
        foreach (string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(":");
            //check parsing is correct
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // Debug 檢查是否解析正確
            //Debug.Log($"Tag detected - Key:{tagKey}, Value:{tagValue}");

            // handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    //Debug.Log("portrait = " + tagValue);
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    //layoutAnimator.Play(tagValue);
                    break;
                //case AUDIO_TAG: 
                //SetCurrentAudioInfo(tagValue);
                //break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // 這段檢查選項數量是否超過UI可支持的選項數
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More Choices were given than the UI can support. Number of choices given"
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {

            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;

            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        //StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        continueStory(); // 讓對話繼續
    }
}
