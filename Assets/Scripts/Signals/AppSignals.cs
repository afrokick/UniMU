using strange.extensions.signal.impl;
using System;

public class AppStartSignal : Signal { }
public class AppQuitSignal : Signal { }

public class HardwareBackPressSignal : Signal { }

public class OpenSelectServerScreenSignal : Signal { }
//public class OpenLevelsMenuSignal : Signal { }

//public class InternetStateChangedSignal : Signal<bool> { }
//public class OpenCheckInternetSignal : Signal { }

public class OpenAlertSignal : Signal<string, Action<AlertResult>> { }

public class ServerListUpdatedSignal : Signal<ServerListInfoModel> { }