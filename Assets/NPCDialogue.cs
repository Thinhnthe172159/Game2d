using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC DiaLogue")]
public class NPCDiaLogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLine;
    public float typingSpeed = 0.1f;
    public AudioClip voiceSound;
    public float voicePith = 1f;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
}
