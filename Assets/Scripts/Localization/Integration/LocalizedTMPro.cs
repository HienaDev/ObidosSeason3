using LocalizationSystem.Core;
using TMPro;
using UnityEngine;

namespace LocalizationSystem.Integration
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMPro : Localization
    {
        private TMP_Text _tmPro;

        private void Awake()
        {
            _tmPro ??= GetComponent<TMP_Text>();
        }

        protected override void Localize(LocalizedInfo info)
        {
            _tmPro.text = info.Text;
        }

        void Reset()
        {
            _tmPro = GetComponent<TMP_Text>();
            if (_tmPro)
            {
                _localizations = new LocalizedInfo[1];
                _localizations[0] = new LocalizedInfo(Language.pt_pt, _tmPro.text);
            }
        }
    }
}
