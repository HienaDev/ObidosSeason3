using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkingData", menuName = "Scriptable Objects/TalkingData")]
public class TalkingData : ScriptableObject
{


    public enum TypeOfConversation
    {
        Football,
        RandomTopics

    }

    [Serializable]
    public struct TalkingTopics
    {
        public Sprite symbol;
        public TypeOfConversation type;
    }

    public TalkingTopics[] topics;
    public TalkingTopics[] alwaysBadTopics;
}
