reg add HKEY_LOCAL_MACHINE\Software\Classes\CLSID\{E6D6B700-124D-11D4-86F3-DB80AFD98778} /v "(Default)" /t REG_SZ /d "AviSynth+" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\CLSID\{E6D6B700-124D-11D4-86F3-DB80AFD98778}\InProcServer32 /v "(Default)" /t REG_SZ /d "AviSynth.dll" /f
reg add HKEY_LOCAL_MACHINE\Software\Classes\CLSID\{E6D6B700-124D-11D4-86F3-DB80AFD98778}\InProcServer32 /v "ThreadingModel" /t REG_SZ /d "Apartment" /f

reg add "HKEY_LOCAL_MACHINE\Software\Classes\Media Type\Extensions\.avs" /v "Source Filter" /t REG_SZ /d "{D3588AB0-0781-11CE-B03A-0020AF0BA770}" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\.avs /v "(Default)" /t REG_SZ /d "avsfile" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\.avsi /v "(Default)" /t REG_SZ /d "avs_auto_file" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\avsfile /v "(Default)" /t REG_SZ /d "AviSynth+ Script" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\avsfile\DefaultIcon /v "(Default)" /t REG_SZ /d "C:\Windows\SysWow64\AviSynth.dll,0" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\avs_auto_file /v "(Default)" /t REG_SZ /d "AviSynth+ Autoload Script" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\avs_auto_file\DefaultIcon /v "(Default)" /t REG_SZ /d "C:\Windows\SysWow64\AviSynth.dll,1" /f

reg add HKEY_LOCAL_MACHINE\Software\Classes\AVIFile\Extensions\AVS /v "(Default)" /t REG_SZ /d "{E6D6B700-124D-11D4-86F3-DB80AFD98778}" /f

reg add HKEY_LOCAL_MACHINE\Software\AviSynth /v "(Default)" /t REG_SZ /d "C:\Program Files (x86)\AviSynth+" /f
reg add HKEY_LOCAL_MACHINE\Software\AviSynth /v "plugindir+" /t REG_SZ /d "C:\Program Files (x86)\AviSynth+\plugins64+" /f
reg add HKEY_LOCAL_MACHINE\Software\AviSynth /v "plugindir2_5" /t REG_SZ /d "C:\Program Files (x86)\AviSynth+\plugins64" /f