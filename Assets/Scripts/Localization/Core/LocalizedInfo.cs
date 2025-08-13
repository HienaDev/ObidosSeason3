using UnityEngine;

namespace LocalizationSystem.Core
{
    [System.Serializable]
    public class LocalizedInfo
    {
        [SerializeField] private Language _language;
        [SerializeField] private string _text;
        [SerializeField] private Sprite _sprite;

        public string Text => _text;
        public Sprite Sprite => _sprite;
        public Language Language => _language;

        public LocalizedInfo(Language language, string text)
        {
            _language = language;
            _text = text;
            _sprite = default;
        }
        public LocalizedInfo(Language language, Sprite texture)
        {
            _language = language;
            _text = default;
            _sprite = texture;
        }

        public static LocalizedInfo GetLocalizationOfLanguage(Language language, LocalizedInfo[] infos, UnityEngine.Object o = default)
        {
            if (infos == default || infos.Length == 0)
            {
                Debug.LogError("Trying to get localized info of nonexisting collection.");
                return new LocalizedInfo(default, "🇵🇹");
            }

            for (int i = 0; i < infos.Length; i++)
                if (infos[i].Language == language)
                    return infos[i];

            Debug.LogWarning("Trying to get localized info of nonexisting language: " + language.ToString() + "\n Defaulting to array [0].", o);
            return infos[0];
        }
    }

}