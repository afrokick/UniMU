using strange.extensions.signal.impl;
using System;

public class AppStartSignal : Signal { }
public class AppQuitSignal : Signal { }

public class HardwareBackPressSignal : Signal { }

public class OpenAlertSignal : Signal<string, Action<AlertResult>> { }

public class OpenSelectServerScreenSignal : Signal { }
public class OpenLoginScreenSignal : Signal { }

public class ServerListUpdatedSignal : Signal<ServerListInfoModel> { }
public class ServerListItemUpdatedSignal : Signal<ServerListItemInfoModel> { }
