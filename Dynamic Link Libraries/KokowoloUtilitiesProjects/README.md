# Building a Dynamic Link Library in Visual Studio Code
This walk-through has been largely copied from https://learn.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio-code?pivots=dotnet-7-0, but has been adapted to better address the steps for Unity Projects. Note, if a solution file and a class library project has already been skipped, open the `.sln` file (as seen in the first couple of steps) then proceed to the steps under **Building the class library project**.

## Create a Solution
1. Start Visual Studio Code.
2. Select **File** > **Open Folder...** (**Open...** on macOS) from the main menu
3. In the **Open Folder** dialog, create a new folder (this will be the name of the `.sln` file) and click **Select Folder** (**Open** on macOS).
4. Open the **Terminal** in Visual Studio Code by selecting **View** > **Terminal** from the main menu. The **Terminal** opens with the command prompt in the solution folder.
5. In the **Terminal**, enter the following command:
```
dotnet new sln
```
The terminal output looks like the following:
```
The template "Solution File" was created successfully.
```

## Create a class library project
1. In the terminal, run the following command to create the library project:
```
dotnet new classlib -o NAME_OF_LIBRARY
```
The -o or --output command specifies the location to place the generated output.
2. Run the following command to add the library project to the solution:
```
dotnet sln add NAME_OF_LIBRARY/NAME_OF_LIBRARY.csproj
```
3. Edit the library targets by opening `NAME_OF_LIBRARY/NAME_OF_LIBRARY.csproj` and make sure Unity is added (additionally add `UnityEditor` if needed). In the future you may need to open Unity's `Assembly-CSharp.csproj` and copy any fields that may have changed.
```
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <Reference Include="UnityEngine">
       <HintPath>C:\Program Files\Unity\Editor\Data\Managed\UnityEngine.dll</HintPath>
     </Reference>
  </ItemGroup>

  <PropertyGroup>
    <LangVersion>9.0</LangVersion>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

</Project>

```

## Building the class library project
1. Add and save any files to the project or create any additional class libraries now.
2. Run the following command to build the solution and verify that the project compiles without error.
```
dotnet build
```
The terminal output looks like the following example:
```
Microsoft (R) Build Engine version 16.7.4+b89cb5fde for .NET
Copyright (C) Microsoft Corporation. All rights reserved.
  Determining projects to restore...
  All projects are up-to-date for restore.
  NAME_OF_LIBRARY -> C:\...\NAME_OF_PROJECT\NAME_OF_LIBRARY\bin\Debug\netstandard2.0\NAME_OF_LIBRARY.dll
Build succeeded.
    0 Warning(s)
    0 Error(s)
Time Elapsed 00:00:02.78
```

## Add the `.dll` to Unity
1. All that Unity needs to run the library files is the newly built `.dll` as seen from the path above. Thus, drag the `.dll` from current folder to the Unity project or package folder that needs it. 
2. After Unity has compiled the library should be available to be used from within Unity.
3. At this point it is safe to delete all the contents of the library folder(s) that are not specifically the C# files and the `.csproj` file (i.e. the `bin` and `obj` directories).