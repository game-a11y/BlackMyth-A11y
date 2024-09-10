#include <mod.hpp>
#include <tolk.h>


// tolk func types

typedef decltype(&Tolk_Load)        TolkLoadPtr;
typedef decltype(&Tolk_HasSpeech)   TolkHasSpeechPtr;
typedef decltype(&Tolk_DetectScreenReader)  TolkDetectScreenReaderPtr;
typedef decltype(&Tolk_Speak)       TolkSpeakPtr;
typedef decltype(&Tolk_Silence)     TolkSilencePtr;


namespace A11yMod
{
    // 加载屏幕阅读器库
    auto BlackMythA11yCpp::sr_load_lib() -> void 
    {
        auto dll_path = STR("Tolk.dll");

        // https://learn.microsoft.com/zh-cn/windows/win32/debug/calling-the-dbghelp-library
        tolk_lib = LoadLibraryW(dll_path);
        if (!tolk_lib) {
            Output::send<LogLevel::Error>(MODSTR("Failed to load dll <{}>, error code: 0x{:x}\n"), dll_path, GetLastError());
            return;
        }

        Output::send<LogLevel::Verbose>(MODSTR("Loaded tolk.dll.\n"));
    }

    // 卸载 SR 库
    auto BlackMythA11yCpp::unload_sr_lib() -> void
    {
        if (!tolk_lib) {
            return;
        }

        // TODO: Tolk_Unload
        FreeLibrary(tolk_lib);
        Output::send<LogLevel::Normal>(MODSTR("Free tolk.dll.\n"));
    }

    // 加载并初始化 DLL
    auto BlackMythA11yCpp::sr_init_and_check() -> void
    {
        if (!tolk_lib) {
            Output::send<LogLevel::Warning>(MODSTR("tolk.dll not load, skip init tolk.\n"));
            return;
        }
        Output::send<LogLevel::Verbose>(MODSTR("sr_init_and_check.\n"));

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

        bool tolk_has_init = tolk_has_speech();
        Output::send<LogLevel::Verbose>(MODSTR("tolk_has_speech: {}\n"), tolk_has_init);
        if (!tolk_has_init) {
            Output::send<LogLevel::Warning>(MODSTR("Tolk has no backend!\n"));
            return;
        }

        std::wstring sr_name{tolk_DetectScreenReader()};
        Output::send<LogLevel::Verbose>(MODSTR("tolk_DetectScreenReader: {}\n"), sr_name);
        
        auto ret = tolk_speak(STR("test test test 11111"), true);
        if (!ret) {
            Output::send<LogLevel::Error>(MODSTR("tolk_speak failed\n"));
        }
    }

};
