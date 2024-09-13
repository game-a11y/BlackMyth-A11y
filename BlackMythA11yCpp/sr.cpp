#include <mod.hpp>
#include <sr.hpp>


namespace A11yMod
{
    // 加载屏幕阅读器库
    auto BlackMythA11yCpp::sr_load_lib() -> void 
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

    // 卸载 SR 库
    auto BlackMythA11yCpp::sr_unload_lib() -> void
    {
        if (!SrLib) {
            return;
        }

        // TODO: Tolk_Unload
        FreeLibrary(SrLib);
        Output::send<LogLevel::Normal>(MODSTR("Free tolk.dll.\n"));
    }

    // 加载并初始化 DLL
    auto BlackMythA11yCpp::sr_init_and_check() -> void
    {
        if (!SrLib) {
            Output::send<LogLevel::Warning>(MODSTR("tolk.dll not load, skip init tolk.\n"));
            return;
        }
        Output::send<LogLevel::Verbose>(MODSTR("sr_init_and_check.\n"));

        /* --- 导出 Tolk 函数 --- */
        // https://blog.benoitblanchon.fr/getprocaddress-like-a-boss/
        auto tolk_init = reinterpret_cast<TolkLoadPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Load"));
        auto tolk_IsLoaded = reinterpret_cast<TolkIsLoadedPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_IsLoaded"));
        auto tolk_Unload = reinterpret_cast<TolkUnloadPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Unload"));
        // Tolk_TrySAPI
        // Tolk_PreferSAPI
        auto tolk_DetectScreenReader = reinterpret_cast<TolkDetectScreenReaderPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_DetectScreenReader"));
        auto tolk_has_speech = reinterpret_cast<TolkHasSpeechPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_HasSpeech"));
        // Tolk_HasBraille
        // Tolk_Output
        auto tolk_speak = reinterpret_cast<TolkSpeakPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Speak"));
        // Tolk_Braille
        auto tolk_IsSpeaking = reinterpret_cast<TolkIsSpeakingPtr>(GetProcAddress((HMODULE)SrLib, "Tolk_IsSpeaking"));
        auto tolk_silence = reinterpret_cast<TolkSilencePtr>(GetProcAddress((HMODULE)SrLib, "Tolk_Silence"));

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
        
        auto ret = tolk_speak(STR("Tolk.dll is loaded"), false);
        if (!ret) {
            Output::send<LogLevel::Error>(MODSTR("tolk_speak failed\n"));
            return;
        }
        ret = tolk_speak(STR("English output test successful."), false);
        ret = tolk_speak(STR("中文输出测试成功！"), false);
    }

};
