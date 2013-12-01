#r @"tools/FAKE/tools/FakeLib.dll"

open Fake

// Config

let product_version = "0.0"
let build_number = "00000"

let version = sprintf "%s.%s-dev" product_version build_number

let buildDir="./build"

let nunitSearchPaths = [ProgramFilesX86 @@ "nunit 2.6.3" @@ "bin"; ProgramFilesX86 @@ "nunit 2.6.1" @@ "bin"; ProgramFilesX86 @@ "nunit 2.6.2" @@ "bin"]

RestorePackages()


// -----------------------------------------

trace ("Building "+version+"...")

let private nunit_console_in_path path = System.IO.File.Exists <| path @@ "nunit-console.exe"
let nunitPath = nunitSearchPaths |> Seq.tryFind nunit_console_in_path

// Helpers

let empty () = ()  // c#: ()=>{}

// Targets

Target "Clean" ( fun _ ->
  CleanDirs [buildDir]
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

Target "Default" empty

// Dependencies

"Clean" ==> "BuildDebug" ==> "Test" ==> "Default"

RunTargetOrDefault "Default"