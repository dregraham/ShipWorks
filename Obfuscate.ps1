cls

remove-item Artifacts\Application\Obfuscated\*.* 
remove-item Artifacts\Application\Original\*.* 
    
rake build:debug

copy Artifacts\Application\ShipWorks.* Artifacts\Application\Original\
copy Artifacts\Application\Interapptive.* Artifacts\Application\Original\

& "C:\Program Files (x86)\PreEmptive Solutions\Dotfuscator Professional Edition Evaluation 4.18.1\dotfuscator.exe" "ShipWorksDotfuscator.xml"

copy Artifacts\Application\Obfuscated\*.* Artifacts\Application\

