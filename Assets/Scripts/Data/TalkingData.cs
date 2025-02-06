using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkingData", menuName = "Scriptable Objects/TalkingData")]
public class TalkingData : ScriptableObject
{


    public enum TypeOfConversation
    {
        Football,
        RandomTopics,
        Hat,
        Book

    }

    public enum BookShape
    {
        Triangle,
        Ball,
        Square

    }




    [Serializable]
    public struct TalkingTopics
    {
        public Sprite symbol;
        public TypeOfConversation type;
    }


    public Color[] bookColors;
    public Sprite[] bookShapes;

    public Sprite[] footballTeamWinning;
    public Sprite[] footballTeamLogo;
    public TalkingTopics[] topics;
    public TalkingTopics[] alwaysBadTopics;
    public TalkingTopics[] hats;

    public Sprite specialItem;
    public Sprite specialHat;
    public Sprite specialTopic;
}
