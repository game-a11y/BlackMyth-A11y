#include <mod.hpp>
#include <tolk.h>

#define NOMINMAX
#include <Windows.h>

// tolk func types

typedef decltype(&Tolk_Load)        TolkLoadPtr;
typedef decltype(&Tolk_HasSpeech)   TolkHasSpeechPtr;
typedef decltype(&Tolk_DetectScreenReader)  TolkDetectScreenReaderPtr;
typedef decltype(&Tolk_Speak)       TolkSpeakPtr;
typedef decltype(&Tolk_Silence)     TolkSilencePtr;


namespace A11yMod
{
    // 加载并初始化 DLL
    auto BlackMythA11yCpp::load_and_init_tolk() -> void
    {
        if (tolk_lib) {
            return;
        }

        /* --- 加载 DLL --- */
        auto dll_path = STR("Tolk.dll");

        // https://learn.microsoft.com/zh-cn/windows/win32/debug/calling-the-dbghelp-library
        tolk_lib = LoadLibraryW(dll_path);
        if (!tolk_lib) {
            Output::send<LogLevel::Error>(MODSTR("Failed to load dll <{}>, error code: 0x{:x}\n"), dll_path, GetLastError());
            return;
        }

        Output::send<LogLevel::Verbose>(MODSTR("Loaded tolk.dll.\n"));
        
        /* --- 导出 Tolk 函数 --- */
        // https://blog.benoitblanchon.fr/getprocaddress-like-a-boss/
        auto tolk_init = reinterpret_cast<TolkLoadPtr>(GetProcAddress((HMODULE)tolk_lib, "Tolk_Load"));
        auto tolk_has_speech = reinterpret_cast<TolkHasSpeechPtr>(GetProcAddress((HMODULE)tolk_lib, "Tolk_HasSpeech"));
        auto tolk_DetectScreenReader = reinterpret_cast<TolkDetectScreenReaderPtr>(GetProcAddress((HMODULE)tolk_lib, "Tolk_DetectScreenReader"));
        auto tolk_speak = reinterpret_cast<TolkSpeakPtr>(GetProcAddress((HMODULE)tolk_lib, "Tolk_Speak"));
        auto tolk_silence = reinterpret_cast<TolkSilencePtr>(GetProcAddress((HMODULE)tolk_lib, "Tolk_Silence"));

        if (!tolk_init) {
            Output::send<LogLevel::Error>(MODSTR("null function Ptr; error code: 0x{:x}\n"), GetLastError());
            return;
        }
        tolk_init();

        Output::send<LogLevel::Verbose>(MODSTR("tolk_has_speech: {}\n"), tolk_has_speech());
        std::wstring sr_name{tolk_DetectScreenReader()};
        Output::send<LogLevel::Verbose>(MODSTR("tolk_DetectScreenReader: {}\n"), sr_name);
        
        auto ret = tolk_speak(STR("test test test 11111"), true);
        if (!ret) {
            Output::send<LogLevel::Error>(MODSTR("tolk_speak failed\n"));
        }
    }

};
