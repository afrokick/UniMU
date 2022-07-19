using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using UniMU.Models;

public class AppStartSignal : Signal { }
public class AppQuitSignal : Signal { }

public class HardwareBackPressSignal : Signal { }

public class OpenAlertSignal : Signal<string, Action<AlertResult>> { }

public class OpenSelectServerScreenSignal : Signal { }
public class OpenLoginScreenSignal : Signal { }

public class ServerListUpdatedSignal : Signal<ServerListInfoModel> { }
public class ServerListItemUpdatedSignal : Signal<ServerListItemInfoModel> { }
public class LoggedInSignal : Signal { }
public class OpenCharactersScreenSignal : Signal { }
public class CharactersListUpdatedSignal : Signal<List<Character>> { }
public class CharacterFocusedSignal : Signal<string> { }
public class CharacterSelectedSignal : Signal { }
public class OpenCreateCharacterPopupSignal : Signal { }
public class OpenLoadingCharacterScreenSignal : Signal { }
public class CharacterCreatedSignal : Signal<bool, Character> { }
public class CharacterDeletedSignal : Signal<bool> { }

public class OpenWorldSignal : Signal { }

public class NewPlayersInScopeSignal : Signal<List<Character>> { }