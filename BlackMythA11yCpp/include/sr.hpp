#pragma once
#include <tolk.h>

/**  tolk func types */

typedef decltype(&Tolk_Load)        TolkLoadPtr;
typedef decltype(&Tolk_IsLoaded)    TolkIsLoadedPtr;
typedef decltype(&Tolk_Unload)      TolkUnloadPtr;

typedef decltype(&Tolk_TrySAPI)     Tolk_TrySAPIPtr;
typedef decltype(&Tolk_PreferSAPI)  Tolk_PreferSAPIPtr;

typedef decltype(&Tolk_DetectScreenReader)  TolkDetectScreenReaderPtr;
typedef decltype(&Tolk_HasSpeech)   TolkHasSpeechPtr;
typedef decltype(&Tolk_HasBraille)  TolkHasBraillePtr;

typedef decltype(&Tolk_Output)      TolkOutputPtr;
typedef decltype(&Tolk_Speak)       TolkSpeakPtr;
typedef decltype(&Tolk_Braille)     TolkBraillePtr;

typedef decltype(&Tolk_IsSpeaking)  TolkIsSpeakingPtr;
typedef decltype(&Tolk_Silence)     TolkSilencePtr;
