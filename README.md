# Plugin

    A lightweight plug-in framework for asp.net mvc.
    
## how to test?

   Visit /Demo/Demo/Index,/Home/Index.

## how to publish plugin?
   please edit post-build event command line like this:
   
    set area=Demo
    rd /q /s $(SolutionDir)WebSite\Plugins\%area%\bin
    rd /q /s $(SolutionDir)WebSite\Plugins\%area%\Views
    mkdir $(SolutionDir)WebSite\Plugins\%area%\bin
    mkdir $(SolutionDir)WebSite\Plugins\%area%\Views
    xcopy /r /y /s  $(ProjectDir)bin $(SolutionDir)WebSite\Plugins\%area%\bin
    xcopy /r /y /s  $(ProjectDir)Views $(SolutionDir)WebSite\Plugins\%area%\Views
   

## web.config
    <appSettings>
       <add key="PluginPath" value="~\Plugins" />
       <add key="PluginBinPath" value="~\Dependency" />
    </appSettings>
    <runtime>
       <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
         <probing privatePath="Dependency" />
        </assemblyBinding>
    </runtime>
