using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueMe : MonoBehaviour
{
    [Header("Conversation Tracker")]
    [SerializeField] int currentIndex = 0;

    [Header("Debug Monitor")]
    [SerializeField] bool playerInRange = false;
    [SerializeField] bool interactionTriggered = false;
    public bool ongoingDialogue = false;
    [SerializeField] bool allowContinuePress = false;

    [Header("Settings")]
    [SerializeField] bool freezeMouseViewDuringDialogue = false;

    DialogueData data;
    GameObject player;
    Image charImage;
    Sprite defaultSprite;
    string[] currentTextSet;
    string[] currentChoiceSet;
    string wholeText;

    float facingAngleTreshold = 24f;
    float letterShowInterval = 0.015f;
    float threeDotInterval = 0.2f;
    int bleepcounter = 0;
    int currentPage = 0;
    int pages = 0;

    /// UI references
    /// UI references

    GameObject dialogueCanvasObj;
    CanvasGroup dialogueCG;
    CanvasGroup charSpriteCG;
    CanvasGroup threedotsCG;
    CanvasGroup choicesCG;

    Button button0;
    Button button1;
    Button button2;
    Button button3;

    TextMeshProUGUI tmpName;
    TextMeshProUGUI tmpText;
    TextMeshProUGUI tmpThreeDots;
    TextMeshProUGUI tmp_b0;
    TextMeshProUGUI tmp_b1;
    TextMeshProUGUI tmp_b2;
    TextMeshProUGUI tmp_b3;

    GameObject choiceBG2;
    GameObject choiceBG3;
    GameObject choiceBG4;

    void ReferenceAcquire()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueCanvasObj = GameObject.FindGameObjectWithTag("DialogueCanvas");

        tmpName = dialogueCanvasObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        tmpText = dialogueCanvasObj.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        threedotsCG = dialogueCanvasObj.transform.GetChild(6).GetComponent<CanvasGroup>();
        tmpThreeDots = dialogueCanvasObj.transform.GetChild(6).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        choicesCG = dialogueCanvasObj.transform.GetChild(7).GetComponent<CanvasGroup>();

        charSpriteCG = dialogueCanvasObj.transform.GetChild(1).GetComponent<CanvasGroup>();
        charImage = dialogueCanvasObj.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        var obj = dialogueCanvasObj.transform.GetChild(7).gameObject;
        choiceBG2 = obj.transform.GetChild(0).gameObject;
        choiceBG3 = obj.transform.GetChild(1).gameObject;
        choiceBG4 = obj.transform.GetChild(2).gameObject;

        button0 = obj.transform.GetChild(3).GetComponent<Button>();
        tmp_b0 = obj.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        button1 = obj.transform.GetChild(4).GetComponent<Button>();
        tmp_b1 = obj.transform.GetChild(4).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        button2 = obj.transform.GetChild(5).GetComponent<Button>();
        tmp_b2 = obj.transform.GetChild(5).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        button3 = obj.transform.GetChild(6).GetComponent<Button>();
        tmp_b3 = obj.transform.GetChild(6).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        dialogueCG = dialogueCanvasObj.GetComponent<CanvasGroup>();
        data = GetComponent<DialogueData>();
    }

    void Start()
    {
        ReferenceAcquire();
        choicesCG.alpha = 0f;
        dialogueCG.alpha = 0f;
        charSpriteCG.alpha = 0f;
    }

    #region TriggerEnter/Exit
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerInRange = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerInRange == true)
        {
            playerInRange = false;
            if (interactionTriggered)
            {
                interactionTriggered = false;
                ForcedExitDialogue();
            }
        }
    }
    #endregion   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange && AngleCheck() && !interactionTriggered && !ongoingDialogue)
        {
            InitiateDialogue();
        }
        else if (Input.GetKeyDown(KeyCode.F) && playerInRange && interactionTriggered && ongoingDialogue)
        {
            if (allowContinuePress)
                DialogueContinue();
        }
    }

    #region Dialogue Hard Code
    void RenderTextField(string text)
    {
        wholeText = text;
        //dummyText = "";
        tmpText.text = "";

        StopCoroutine("RenderLetters");
        StartCoroutine("RenderLetters");
    }

    string[] AcquireDialogueTextSet()
    {
        return data.dialoguePackage[currentIndex].textSlide;
    }
    string[] AcquireAnswerSet()
    {
        if(data.dialoguePackage[currentIndex].repliesAtEnd == false)
        {
            return null;
        }
        else
        {
            return data.dialoguePackage[currentIndex].replyChoice;
        }        
    }
    Sprite AcquireDefaultSprite()
    {
        if(data.sprites[0] == null)
        {
            Debug.Log("Couldn't set default sprite because array is empty!");
            return null;
        }
        else
        {
            return data.sprites[0];
        }        
    }
    Sprite SetDialogueSprite()
    {
        if(data.dialoguePackage[currentIndex].spriteToShow.Length <= 0)
        {
            return defaultSprite;
        }

        var s = data.dialoguePackage[currentIndex].spriteToShow[0];        
        int i = 0;
        switch (s)
        {
            case DialogueData.CharSprites.unspecified:
                i = 0;
                break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
            //case DialogueData2.CharSprites.unspecified:
            //    i = 0;
            //    break;
        }
        return data.sprites[i];
    }

    void UpdateInteractionCode(int val)
    {
        currentIndex = val;
    }

    IEnumerator RenderLetters()
    {
        StopAllFinishBlink();

        bleepcounter = 0;
        foreach (char c in wholeText)
        {
            bleepcounter += 1;
            if (bleepcounter == data.vocalisationFrequency)
            {
                //audio for gibberish vocalisation goes here
                bleepcounter = 0;
            }
            tmpText.text += c;
            yield return new WaitForSeconds(letterShowInterval);
        }
        FinishedRenderingLetters();
    }
    void FinishedRenderingLetters()
    {
        if (currentPage < pages)
        {
            threedotsCG.alpha = 1f;
            StartCoroutine("Threedots"); ///////
            allowContinuePress = true;
        }
        else if (currentPage == pages && data.dialoguePackage[currentIndex].repliesAtEnd == false )
        {
            threedotsCG.alpha = 1f;
            StartCoroutine("Exitblink"); //////
            allowContinuePress = true;
        }
        else if (currentPage == pages && data.dialoguePackage[currentIndex].repliesAtEnd == true)
        {
            threedotsCG.alpha = 1f;
            StartCoroutine("AwaitResponse"); //////
            allowContinuePress = true;
        }
    }

    void InitiateDialogue()
    {
        if(freezeMouseViewDuringDialogue)
            player.GetComponent<SimplerMovement>().InDialogueCurrently(true);

        tmpName.text = data.characterName;
        tmpText.text = "";
        defaultSprite = AcquireDefaultSprite();

        ongoingDialogue = true;
        interactionTriggered = true;
        dialogueCG.alpha = 0f;
        charSpriteCG.alpha = 0f;
        threedotsCG.alpha = 0f;

        charImage.sprite = SetDialogueSprite();
        currentTextSet = AcquireDialogueTextSet();
        currentChoiceSet = AcquireAnswerSet();
        pages = currentTextSet.Length;
        currentPage = 1;

        RenderTextField(currentTextSet[0]);
        FadeIn();
    }

    void DialogueContinue()
    {
        if (currentPage < pages)
        {
            threedotsCG.alpha = 0f;
            allowContinuePress = false;
            StartCoroutine(CursorChange(false, 0f));
            currentPage += 1;
            SetDialogueSprite();

            //charImg.sprite = charSprite[0];
            RenderTextField(currentTextSet[currentPage - 1]);
        }
        else if (currentPage == pages && (currentChoiceSet == null || currentChoiceSet.Length <= 1))
        {
            ///dialogue concluded!
            threedotsCG.alpha = 0f;
            allowContinuePress = false;
            CursorChange(false, 0f);
            currentPage = 0;
            tmpText.text = "";
            FadeOut();
            ongoingDialogue = false;
            interactionTriggered = false;
            
            if(freezeMouseViewDuringDialogue)
                player.GetComponent<SimplerMovement>().InDialogueCurrently(false);

            if (data.dialoguePackage[currentIndex].circular && !data.dialoguePackage[currentIndex].repliesAtEnd)
            {
                return;
            }
            else if(!data.dialoguePackage[currentIndex].circular && !data.dialoguePackage[currentIndex].repliesAtEnd)
            {
                var val = data.dialoguePackage[currentIndex].ifNoRepliesNewIndex;
                UpdateInteractionCode(val);
            }
        }
        else if (currentPage == pages && currentChoiceSet.Length >= 2)
        {
            SetDialogueSprite();
            StopAllFinishBlink();
            threedotsCG.alpha = 1f;
            StartCoroutine("AwaitResponse");
            allowContinuePress = false;
            StartCoroutine(CursorChange(true, 0.4f));
            RevealReplyChoices();
        }
    }

    public void ChosenReplyContinue(int val)
    {
        currentIndex = val;

        StopAllFinishBlink();
        tmpText.text = "";
        //charImg.sprite = charSprites[0];
        ongoingDialogue = true;
        threedotsCG.alpha = 0f;
        allowContinuePress = false;

        currentTextSet = AcquireDialogueTextSet();
        currentChoiceSet = AcquireAnswerSet();

        pages = currentTextSet.Length;
        currentPage = 1;
        RenderTextField(currentTextSet[0]);
        StartCoroutine(FadeCanvas(choicesCG, 1f, 0f, 0.2f, 0f));
        StartCoroutine(CursorChange(false, 0f));
    }

    void ForcedExitDialogue()
    {
        if (ongoingDialogue)
        {
            StopAllFinishBlink();
            if (choicesCG.alpha != 0f)
                StartCoroutine(FadeCanvas(choicesCG, 1f, 0f, 0.2f, 0f));
            ongoingDialogue = false;
            allowContinuePress = false;
            interactionTriggered = false;
            StartCoroutine(CursorChange(false, 0f));
            FadeOut();

            if(freezeMouseViewDuringDialogue)
                player.GetComponent<SimplerMovement>().InDialogueCurrently(false);
        }
    }

    #endregion



    #region UImanagement

    void ResetRevealChoiceElements()
    {
        button0.gameObject.SetActive(false);
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        choiceBG2.SetActive(false);
        choiceBG3.SetActive(false);
        choiceBG4.SetActive(false);
    }

    void RevealReplyChoices()
    {
        if (data.dialoguePackage[currentIndex].replyChoice.Length <= 1 && data.dialoguePackage[currentIndex].repliesAtEnd == true)
        {
            Debug.Log("reply with only 1 option??");
            return;
        }

        ResetRevealChoiceElements();
        var requiredFields = data.dialoguePackage[currentIndex].replyChoice.Length;
        switch (requiredFields)
        {
            case 2:
                tmp_b0.text = data.dialoguePackage[currentIndex].replyChoice[0];
                tmp_b1.text = data.dialoguePackage[currentIndex].replyChoice[1];
                button0.gameObject.SetActive(true);
                button1.gameObject.SetActive(true);
                choiceBG2.SetActive(true);
                break;
            case 3:
                tmp_b0.text = data.dialoguePackage[currentIndex].replyChoice[0];
                tmp_b1.text = data.dialoguePackage[currentIndex].replyChoice[1];
                tmp_b2.text = data.dialoguePackage[currentIndex].replyChoice[2];
                button0.gameObject.SetActive(true);
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                choiceBG3.SetActive(true);
                break;
            case 4:
                tmp_b0.text = data.dialoguePackage[currentIndex].replyChoice[0];
                tmp_b1.text = data.dialoguePackage[currentIndex].replyChoice[1];
                tmp_b2.text = data.dialoguePackage[currentIndex].replyChoice[2];
                tmp_b3.text = data.dialoguePackage[currentIndex].replyChoice[3];
                button0.gameObject.SetActive(true);
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                button3.gameObject.SetActive(true);
                choiceBG4.SetActive(true);
                break;
        }
        StartCoroutine(FadeCanvas(choicesCG, 0f, 1f, 0.2f, 0.25f));
    }

    void StopAllFinishBlink()
    {
        StopCoroutine("AwaitResponse");
        StopCoroutine("Threedots");
        StopCoroutine("Exitblink");
    }
    IEnumerator Threedots()
    {
        tmpThreeDots.text = "";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = " .";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = " ..";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = " ...";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        StartCoroutine("Threedots");
    }
    IEnumerator Exitblink()
    {
        tmpThreeDots.text = "";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = ">";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = ">>";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        StartCoroutine("Exitblink");
    }
    IEnumerator AwaitResponse()
    {
        tmpThreeDots.text = "";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = "?";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        tmpThreeDots.text = "??";
        yield return new WaitForSecondsRealtime(threeDotInterval);
        StartCoroutine("AwaitResponse");
    }

    #endregion

    #region Fading
    void FadeIn()
    {
        StartCoroutine(FadeCanvas(dialogueCG, 0f, 1f, 0.2f, 0f));
        StartCoroutine(FadeCanvas(charSpriteCG, 0f, 1f, 0.5f, 0.25f));
    }

    void FadeOut()
    {
        StartCoroutine(FadeCanvas(dialogueCG, 1f, 0f, 0.2f, 0f));
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        float elapsedTime = 0f;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }
        cg.alpha = to;
    }
    #endregion

    #region utility    

    IEnumerator CursorChange(bool onOrOff, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (onOrOff == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    bool AngleCheck()
    {
        var dir = transform.position - player.transform.position;
        float angle = Vector3.Angle(dir, player.transform.forward);

        if (angle <= facingAngleTreshold)
            return true;
        else
            return false;
    }
    #endregion
}
