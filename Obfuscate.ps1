cls

remove-item Artifacts\Application\Obfuscated\*.* 
remove-item Artifacts\Application\Original\*.* 
    
rake build:debug

copy Artifacts\Application\ShipWorks.* Artifacts\Application\Original\
copy Artifacts\Application\Interapptive.* Artifacts\Application\Original\

& "C:\Program Files (x86)\PreEmptive Solutions\Dotfuscator Professional Edition Evaluation 4.18.1\dotfuscator.exe" "ShipWorksDotfuscator.xml"

copy Artifacts\Application\Obfuscated\*.* Artifacts\Application\


copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\ShipWorks.Tests\bin\Debug
copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\Interapptive.Shared.Tests\bin\Debug
copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\ShipWorks.Data.Modal.Tests\bin\Debug
copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\ShipWorks.Shipping.Tests\bin\Debug
copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\ShipWorks.Shipping.UI.Tests\bin\Debug
copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\ShipWorks.Stores.Tests\bin\Debug
copy Artifacts\Application\*.* D:\Projects\ShipWorks\Code\ShipWorks.UI.Tests\bin\Debug