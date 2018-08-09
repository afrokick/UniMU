using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    private string _key;

    private int _curLangId = -1;
    private Text _label;

    void Awake()
    {
        _label = GetComponent<Text>();
    }

    void Update()
    {
        if (_label != null && _curLangId != (int)LocalizationService.CurrentLang)
        {
            _curLangId = (int)LocalizationService.CurrentLang;

            var val = LocalizationService.Get(_key);

            if (!string.IsNullOrEmpty(val))
            {
                val = val.Replace("\\n", "\n");
            }

            _label.text = val;
        }
    }
}