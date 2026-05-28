
task("installmod")
    on_run("install_mod")
    set_menu {
        usage = "xmake installmod [options] [modname]",
        description = "Installs a mod into the specified game directory.",
        options = 
        {
            {'d', 'exedir', "v", nil, "Path of the game's executable folder"},
            {nil, 'name', "v", nil, "Name of the mod to install."},
        }
    }

task("bi")
    on_run("build_and_install")
    set_menu {
        usage = "xmake bi [options] [modname]",
        description = "Build and install a mod. Equivalent to 'xmake build' followed by 'xmake installmod'",
        options = 
        {
            {'d', 'exedir', "v", nil, "Path of the game's executable folder"},
            {nil, 'name', "v", nil, "Name of the mod to build and install."},
        }
    }
