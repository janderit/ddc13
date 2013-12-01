#r @"tools/FAKE/tools/FakeLib.dll"
#r @"tools/nugettool.dll"

open Fake

// Config

let project = "Bibliothek"
let project_description = "DDC Demo Bibliothek"
let project_summary = "DDC Demo Bibliothek Nuget Paket"
let project_tags = "Demo"
let copyright = "Copyright Jander IT 2013"
let authors = ["Jander IT"]

let product_version = "0.0"
let build_number = match buildServer with
                   | Jenkins -> jenkinsBuildNumber.PadLeft(5, '0')
                   | _ -> "00000"

let version = sprintf "%s.%s-dev" product_version build_number

let buildDir="./build"
let packagingDir="./temp_nuget"
let deployDir="./deploy"

let nunitSearchPaths = [ProgramFilesX86 @@ "nunit 2.6.3" @@ "bin"; ProgramFilesX86 @@ "nunit 2.6.1" @@ "bin"; ProgramFilesX86 @@ "nunit 2.6.2" @@ "bin"]

RestorePackages()


// -----------------------------------------

trace ("Building "+version+"...")

let private nunit_console_in_path path = System.IO.File.Exists <| path @@ "nunit-console.exe"
let nunitPath = nunitSearchPaths |> Seq.tryFind nunit_console_in_path

// Helpers

let empty () = ()  // c#: ()=>{}
let getdependencies projectfile = NugetTool.Dependencies.Find projectfile |> Seq.map (fun dep -> dep.Id, ("["+dep.Version+"]")) |> Seq.toList

// Targets

Target "Clean" ( fun _ ->
  CleanDirs [buildDir;packagingDir;deployDir]
)

Target "BuildDebug" ( fun _ ->
  !! "**/*.csproj"
  |> MSBuildDebug buildDir "Build"
  |> Log "Build: "
)

Target "Test" ( fun _ ->
   
  let toolpath = match nunitPath with
                 | None -> failwith "Nunit-console.exe not found on nunitSearchPaths"
                 | Some(path) -> path

  let configuration = fun p -> { p with
                                   OutputFile = buildDir @@ "TestResults.xml"
                                   ToolPath = toolpath
                                   DisableShadowCopy = true
                               }
  !! (buildDir @@ "*.dll") |> NUnit configuration
)

Target "Nuget" ( fun _ ->
    // Copy all the package files into a package folder
    CreateDir packagingDir
    CreateDir deployDir

    let net45 = packagingDir @@ "lib" @@ "net45"
    CreateDir net45
    CopyFiles net45 [buildDir @@ project+".dll"]

    let deps = getdependencies project
    trace <| sprintf "%A" deps

    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = project
            Description = project_description
            OutputPath = deployDir
            Summary = project_summary
            WorkingDir = packagingDir
            Copyright = copyright
            Version = version
            Publish = false
            Tags = project_tags
            Dependencies = deps
             }) 
            "template.nuspec"    
) 

Target "Default" empty

// Dependencies

"Clean" ==> "BuildDebug" ==> "Test" ==> "Nuget" ==> "Default"

RunTargetOrDefault "Default"