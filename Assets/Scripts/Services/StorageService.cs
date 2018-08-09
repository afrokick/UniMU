using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Globalization;

/// <summary>
/// Storage Service used for store data on disk at Application.persistentDataPath;
/// </summary>
public interface IStorageService
{
	void Set(string id, string val);
	void SetBool(string id, bool val);
	void SetInt(string id, int val);
	void SetFloat(string id, float value);

	string Get(string id);
	int GetInt(string id);
	float GetFloat(string id);
	bool GetBool(string id);

	/// <summary>
	/// Remove key from dictionary if it exists
	/// </summary>
	/// <param name="id">Key in dictionary</param>
	void Remove(string id);

	/// <summary>
	/// Check that dicitonary contains key 'id'
	/// </summary>
	/// <param name="id">Key in dictionary</param>
	bool Exists(string id);

	/// <summary>
	/// Flush data to disk
	/// </summary>
	void Save();
}

public class StorageService: IStorageService
{
	private const string YES_BOOL = "1";
	private const string NO_BOOL = "0";

    public void SetInt(string id, int val)
    {
        PlayerPrefs.SetInt(id, val);
    }

    public int GetInt(string id)
    {
        return PlayerPrefs.GetInt(id, 0);
    }

    public void Set(string id, string val)
    {
        PlayerPrefs.SetString(id, val);
    }

    public string Get(string id)
    {
        return PlayerPrefs.GetString(id, string.Empty);
    }

    public float GetFloat(string id)
    {
		var str = Get(id);

		if (string.IsNullOrEmpty(str))
			return 0f;
		
		return float.Parse(str, CultureInfo.InvariantCulture);
    }

    public void SetFloat(string id, float value)
    {
        Set(id, value.ToString(CultureInfo.InvariantCulture));
    }

    public void SetBool(string id, bool val)
    {
		Set(id, val ? YES_BOOL : NO_BOOL);
    }

    public bool GetBool(string id)
    {
		return Get(id).Equals(YES_BOOL);
    }

    public void Remove(string id)
    {
        PlayerPrefs.DeleteKey(id);
    }

    public bool Exists(string id)
    {
        return PlayerPrefs.HasKey(id);
    }

    public void Save()
    {
        PlayerPrefs.Flush();
    }
}