#r @"tools/FAKE/tools/FakeLib.dll"

open Fake

// Config

let product_version = "0.0"
let build_number = "00000"

let version = sprintf "%s.%s-dev" product_version build_number

let buildDir="./build"

let nunitPath = ProgramFilesX86 @@ "nunit 2.6.3" @@ "bin" 

RestorePackages()


// -----------------------------------------

trace ("Building "+version+"...")

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
  let configuration = fun p -> { p with
                                   OutputFile = buildDir @@ "TestResults.xml"
                                   ToolPath = nunitPath
                                   DisableShadowCopy = true
                               }
  !! (buildDir @@ "*.dll") |> NUnit configuration
)

Target "Default" empty

// Dependencies

"Clean" ==> "BuildDebug" ==> "Test" ==> "Default"

RunTargetOrDefault "Default"