public abstract class BasePopup : CachedMonoBehaviour
{
    public bool IsShowed { get { return gameObject.activeSelf; } }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}