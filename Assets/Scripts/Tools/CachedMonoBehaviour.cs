/*
 * Created by Alexander Sosnovskiy. May 3, 2016
 */
using UnityEngine;

/// <summary>
/// MonoBehaviour with cached properties
/// </summary>
public class CachedMonoBehaviour : MonoBehaviour
{
    private Transform _cachedTransform;
    private GameObject _cachedGameObject;
    private Rigidbody2D _cachedRigidbody2D;
    private Collider _cachedCollider;
    private Rigidbody _cachedRigidbody;
    private RectTransform _cachedRectTransform;
	private AudioSource _cachedAudioSource;

    public new Transform transform
    {
        get
        {
            if (_cachedTransform == null)
                _cachedTransform = base.transform;
            return _cachedTransform;
        }
    }

    public RectTransform rectTransform
    {
        get
        {
            if (_cachedRectTransform == null)
                _cachedRectTransform = transform as RectTransform;
            return _cachedRectTransform;
        }
    }

    public new GameObject gameObject
    {
        get
        {
            if (_cachedGameObject == null)
                _cachedGameObject = base.gameObject;
            return _cachedGameObject;
        }
    }

    public new Rigidbody2D rigidbody2D
    {
        get
        {
            if (_cachedRigidbody2D == null)
                _cachedRigidbody2D = base.GetComponent<Rigidbody2D>();
            return _cachedRigidbody2D;
        }
    }

    public new Collider collider
    {
        get
        {
            if (_cachedCollider == null)
                _cachedCollider = base.GetComponent<Collider>();

            return _cachedCollider;
        }
    }

    public new Rigidbody rigidbody
    {
        get
        {
            if (_cachedRigidbody == null)
                _cachedRigidbody = base.GetComponent<Rigidbody>();

            return _cachedRigidbody;
        }
    }

	public new AudioSource audio
	{
		get
		{
			if(_cachedAudioSource == null)
				_cachedAudioSource = base.GetComponent<AudioSource>();

			return _cachedAudioSource;
		}
	}
}