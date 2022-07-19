using strange.extensions.context.impl;
using strange.extensions.signal.impl;
using UnityEngine;
using System.Diagnostics;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using log4net;

public class MainContextView : ContextView
{
    private readonly ILog log = LogManager.GetLogger(typeof(MainContextView));

    [SerializeField]
    private Transform _uiRoot;

    [SerializeField]
    private bool _resetPlayerPrefs;

    private MainContext _context;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
#if UNITY_EDITOR
        if (_resetPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Flush();
        }
#endif
        var sw = Stopwatch.StartNew();
        _context = new MainContext(_uiRoot, this);
        sw.Stop();
        log.Debug("Create context:" + sw.ElapsedMilliseconds + "ms");

        sw = Stopwatch.StartNew();
        _context.Start();
        sw.Stop();
        log.Debug("Start context:" + sw.ElapsedMilliseconds + "ms");

        sw = Stopwatch.StartNew();
        _context.Launch();
        sw.Stop();
        log.Debug("Launch context:" + sw.ElapsedMilliseconds + "ms");
    }

    //Dispose all objects then app die
    void OnApplicationQuit()
    {
        PlayerPrefs.Flush();

        DispatchSignal<AppQuitSignal>();
    }

    void OnApplicationPause(bool paused)
    {
        PlayerPrefs.Flush();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DispatchSignal<HardwareBackPressSignal>();
        }
    }

    private void DispatchSignal<T>() where T : Signal
    {
        if (_context == null)
            return;

        var signal = _context.GetComponent<T>() as T;

        if (signal == null)
            return;

        signal.Dispatch();
    }
}