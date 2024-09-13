#pragma once
#define NOMINMAX
#include <Windows.h>  // HMODULE
#include "tolk.h"


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


namespace A11yMod::SR
{

class SrApi {
public:
    SrApi();
    ~SrApi();

public:
    TolkLoadPtr load;
    TolkIsLoadedPtr is_loaded;
    TolkUnloadPtr unload;

    // Tolk_TrySAPI
    // Tolk_PreferSAPI

    TolkDetectScreenReaderPtr detect_screenreader;
    TolkHasSpeechPtr has_speech;
    // Tolk_HasBraille

    // Tolk_Output
    TolkSpeakPtr speak;
    // Tolk_Braille

    TolkIsSpeakingPtr is_speaking;
    TolkSilencePtr silence;

private:
    HMODULE SrLib;

public:
    auto sr_init_and_check() -> void;

private:
    auto sr_func_init() -> void;
    auto sr_func_check() -> void;
};

}
