local projectName = "BlackMythA11yCpp"

target(projectName)
    add_rules("ue4ss.mod")
    add_includedirs(".")
    add_includedirs("include")
    add_headerfiles("*.hpp")
    add_files("*.cpp") 
