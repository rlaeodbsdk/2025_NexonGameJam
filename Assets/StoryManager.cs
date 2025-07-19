using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public List<StoryDialog> storyDialogs;

    public StoryDialog getNextStory()
    {
        return storyDialogs[Managers.Game.stageNumber];
    }
}
