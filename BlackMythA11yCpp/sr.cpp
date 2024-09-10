#include <a11y.hpp>
#include <tolk.h>

#define NOMINMAX
#include <Windows.h>


namespace A11yMod
{
    auto BlackMythA11yCpp::load_tolk_lib() -> void
    {
        auto dll_path = STR("Tolk.dll");

        tolk_lib = LoadLibraryW(dll_path);
        if (!tolk_lib) {
            Output::send<LogLevel::Error>(MODSTR("Failed to load dll <{}>, error code: 0x{:x}\n"), dll_path, GetLastError());
            return;
        }

        Output::send<LogLevel::Verbose>(MODSTR("Loaded tolk.dll.\n"));
        
        
    }
};
