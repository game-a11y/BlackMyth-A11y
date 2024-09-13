#include <DynamicOutput/DynamicOutput.hpp>
#include "WkCommon.hpp"
#include "sr.hpp"


namespace A11yMod::SR
{
    using namespace RC;

    SrApi::SrApi()
    {
        auto dll_path = STR("Tolk.dll");

        // https://learn.microsoft.com/zh-cn/windows/win32/debug/calling-the-dbghelp-library
        SrLib = LoadLibraryW(dll_path);
        if (!SrLib) {
            Output::send<LogLevel::Error>(MODSTR("Failed to load dll <{}>, error code: 0x{:x}\n"), dll_path, GetLastError());
            return;
        }

        Output::send<LogLevel::Verbose>(MODSTR("Loaded tolk.dll.\n"));
    }

    SrApi::~SrApi()
    {
        if (!SrLib) {
            return;
        }

        // TODO: Tolk_Unload
        FreeLibrary(SrLib);
        Output::send<LogLevel::Normal>(MODSTR("Free tolk.dll.\n"));
    }

    auto SrApi::func_init() -> void
    {
        if (!SrLib) {
            Output::send<LogLevel::Warning>(MODSTR("tolk.dll not load, skip init tolk.\n"));
            return;
        }
        Output::send<LogLevel::Verbose>(MODSTR("init_and_check.\n"));

        /* --- 导出 Tolk 函数 --- */
        // https://blog.benoitblanchon.fr/getprocaddress-like-a-boss/
        load = reinterpret_cast<TolkLoadPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Load"));
        is_loaded = reinterpret_cast<TolkIsLoadedPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_IsLoaded"));
        unload = reinterpret_cast<TolkUnloadPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Unload"));
        // Tolk_TrySAPI
        // Tolk_PreferSAPI
        detect_screenreader = reinterpret_cast<TolkDetectScreenReaderPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_DetectScreenReader"));
        has_speech = reinterpret_cast<TolkHasSpeechPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_HasSpeech"));
        // Tolk_HasBraille
        // Tolk_Output
        speak = reinterpret_cast<TolkSpeakPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Speak"));
        // Tolk_Braille
        is_speaking = reinterpret_cast<TolkIsSpeakingPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_IsSpeaking"));
        silence = reinterpret_cast<TolkSilencePtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Silence"));
    }

    auto SrApi::sr_func_check() -> void
    {
        if (!load) {
            Output::send<LogLevel::Error>(MODSTR("null function Ptr; error code: 0x{:x}\n"), GetLastError());
            return;
        }
        load();

        bool tolk_has_init = has_speech();
        Output::send<LogLevel::Verbose>(MODSTR("tolk_has_speech: {}\n"), tolk_has_init);
        if (!tolk_has_init) {
            Output::send<LogLevel::Warning>(MODSTR("Tolk has no backend!\n"));
            return;
        }

        std::wstring sr_name{detect_screenreader()};
        Output::send<LogLevel::Verbose>(MODSTR("tolk_DetectScreenReader: {}\n"), sr_name);
        
        auto ret = speak(STR("Tolk.dll is loaded"), false);
        if (!ret) {
            Output::send<LogLevel::Error>(MODSTR("tolk_speak failed\n"));
            return;
        }
        ret = speak(STR("English output test successful."), false);
        ret = speak(STR("中文输出测试成功！"), false);
    }

    auto SrApi::init_and_check() -> void
    {
        func_init();
        sr_func_check();
    }

};
