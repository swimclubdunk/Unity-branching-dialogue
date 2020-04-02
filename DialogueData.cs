using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    public string characterName;
    public int vocalisationFrequency = 4;
    public Sprite[] sprites;
    public enum CharSprites {unspecified};
    public InteractionSet[] dialoguePackage;

    [System.Serializable]
    public class InteractionSet
    {
        [Space(10)]
        public bool circular = false;
        public bool repliesAtEnd = false;
        public int ifNoRepliesNewIndex = 0;
        [TextArea(5,5)]
        public string[] textSlide;
        public DialogueData.CharSprites[] spriteToShow;
        [TextArea(3, 3)]
        public string[] replyChoice;
        public int[] nextIndex;        
    }
}
