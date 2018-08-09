using UnityEngine;

public class UIManager : MonoBehaviour
{
	//public PreloaderScreen PreloaderScreen;
	//public ThemesMenuScreen ThemesMenuScreen;
	//public LevelsMenuScreen LevelsMenuScreen;
	//public GameScreen GameScreen;

	//public LosePopup LosePopup;
	//public WordFinishedPopup WordFinishedPopup;

	//public BuyTipPopup BuyTipPopup;
	//public BuyCoinsPopup BuyCoinsPopup;

	//public LoaderPopup LoaderPopup;
	//public CoinsBarView CoinsBarView;

    public string GameServerSharedSecret = "";

	public static UIManager Instance;

	void Awake()
	{
		Instance = this;
	}
}