# AMD3DConfigurator
Small GUI to configure registry entries for Ryzen 3D cpu core-selection.


## Usage
1. Add the software you want to pin to a specific type of core by clicking `Add` and selecting the respective exe.   
You can change the name of the key used in the registry (but be reasonable - it's not validated!).
2. Select the core-type you want with the switch in front of the entry.
3. Repeat 1 and 2 for every program you want to configure.
4. Save and apply your changes by pressing 'Apply Changes'.   
This will write your changes to the registry (`HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Services\amd3dvcache\Preferences\App`) and restart the service `amd3dvcacheSvc`.

If you ever want to remove an entry just right-click -> delete it. Same thing with changing the cores - just click the switch.   
Don't forget to apply the changes!

> **Info**   
> Since this software writes to the registry and restarts a system-service it requires to run with elevated permissions and will prompt for them on startup!