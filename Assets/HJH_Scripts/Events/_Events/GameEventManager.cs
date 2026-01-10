using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;

    public MiscEvents miscEvents;

    public QuestEvents questEvents;

    public InputEvents inputEvents;

    public PlayerEvents playerEvents;

    public GoldEvents goldEvents;

    public DialogueEvents dialogueEvents;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        miscEvents = new MiscEvents(); 
        questEvents = new QuestEvents();
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        goldEvents = new GoldEvents();
        dialogueEvents = new DialogueEvents();
    }
}
