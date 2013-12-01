#r @"tools/FAKE/tools/FakeLib.dll"

open Fake

// Config

let product_version = "0.0"
let build_number = "00000"

let version = sprintf "%s.%s-dev" product_version build_number


// -----------------------------------------

trace ("Building "+version+"...")

// Helpers

let empty () = ()  // c#: ()=>{}

// Targets

Target "Default" empty

RunTargetOrDefault "Default"