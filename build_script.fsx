#r @"tools/FAKE/tools/FakeLib.dll"

open Fake

// Config

let product_version = "0.0"
let build_number = "00000"

let version = sprintf "%s.%s-dev" product_version build_number

let buildDir="./build"


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

Target "Default" empty

// Dependencies

"Clean" ==> "BuildDebug" ==> "Default"

RunTargetOrDefault "Default"