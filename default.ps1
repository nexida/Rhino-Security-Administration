properties { 
  $base_dir  = resolve-path .
  $build_dir = "$base_dir\build" 
  $sln_file = "$base_dir\src\Rhino.Security.Mgmt.sln" 
  $version = "1.0.0.0"
  $humanReadableversion = "1.0"
  $product = "Rhino Security Administration $version"
  $company = "NEXiDA srl"
  $copyright = "NEXiDA srl 2008 - 2010"
  $title = "Rhino Security Administration $version"
  $description = "Administration UI for the Rhino Security security library"
} 

include .\psake_ext.ps1
	
task default -depends Compile

task Clean { 
  remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright

	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt.Controllers\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright

	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt.Data\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright

	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt.Dtos\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright
        
	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt.Infrastructure\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright        
		
	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt.Model\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright

	Generate-Assembly-Info `
		-file "$base_dir\src\Rhino.Security.Mgmt.Tests\Properties\AssemblyInfo.cs" `
		-title $title `
		-description $description `
		-company $company `
		-product $product `
		-version $version `
		-copyright $copyright                
	new-item $build_dir -itemType directory 
} 

task Compile -depends Init { 
  & msbuild "$sln_file" "/p:OutDir=$build_dir\" /p:Configuration=Release
  if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute msbuild"
  }

} 