#tool "nuget:?package=xunit.runner.console&version=2.4.0"
#tool "nuget:?package=vswhere&version=2.8.4"
//#r "tools/Cake/BuildSupport.dll"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

// Constants
var innoPath = @"C:\Program Files (x86)\Inno Setup 5\ISCC.EXE";
//var revisionFilePath = @"\\INTFS01\Development\CruiseControl\Configuration_MovedToAWS\Versioning\ShipWorks\NextRevision.txt";
var revisionFilePath = @"\\intdev1201\NetworkShare\DevShare\BuildConfig\NextRevision.txt";

DirectoryPath vsLatest  = VSWhereLatest(new VSWhereLatestSettings { Version = "[15.0,16.0]" });
FilePath msBuildPathX64 = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");
							
if (msBuildPathX64 == null || !System.IO.File.Exists(msBuildPathX64.FullPath))
{
	vsLatest  = VSWhereLatest(new VSWhereLatestSettings { Version = "[15.0,16.0]" });
	msBuildPathX64 = (vsLatest == null) ? null : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");
}
else if (msBuildPathX64==null || !System.IO.File.Exists(msBuildPathX64.FullPath))
{
	vsLatest  = VSWhereLatest();
	msBuildPathX64 = (vsLatest == null) ? null : vsLatest.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");
}

Information("msBuildPathX64: " + msBuildPathX64);
LogStartMessage("Getting args");

// Get the params argument that was passed in
// We default to Build and Debug in case no param is passed in
var args = ""; // Argument("params", "Build#DebugNoAnalyzers");
var target = Argument("Target", "Build");
var configuration = Argument("configuration", "Debug");
var bracketParam = "";
var treatWarningsAsErrors = "true";
var buildDir = "";
var instanceID = "";
var testCategory = "";
var verbosity = Verbosity.Minimal;

if (target.Contains(":"))
{
	Information("target contains : target=> " + target);

	var splitTarget = target.Split(':');
	target = splitTarget[0];
	Information("    new target " + target);

	if (splitTarget.Length > 1)
	{
		var targetRight = splitTarget[1].ToLower();
		Information("    targetRight " + targetRight);

		if (targetRight != string.Empty)
		{
			if (targetRight == "units")
			{
				target = "TestUnits";
			}
			if (targetRight == "specs")
			{
				target = "TestSpecs";
			}
			else if (targetRight == "clean")
			{
				target = "Clean";
			}
			else if (targetRight == "restore")
			{
				target = "Restore-NuGet-Packages";
			}
			else if (targetRight == "quick")
			{
				target = "BuildQuick";
			}
			else if (targetRight == "debug32")
			{
				target = "BuildDebug32";
			}
			else if (targetRight == "release")
			{
				target = "BuildRelease";
			}
			else if (targetRight.StartsWith("debug_installer"))
			{
				target = "DebugInstaller";
			}
			else if (targetRight.StartsWith("internal_installer"))
			{
				target = "InternalInstaller";
			}
			else if (targetRight.StartsWith("public_installer"))
			{
				target = "PublicInstaller";
			}
			else if (targetRight == "quiet")
			{
				verbosity = Verbosity.Quiet;
			}
			else if (targetRight.StartsWith("integration"))
			{
				target = "TestIntegration";
				testCategory = targetRight.Substring(targetRight.IndexOf("[") + 1, targetRight.Length - targetRight.IndexOf("[") - 2);
			}
		}
	}
}

Information("+++++++++++++++++++++++++++++++++++++++++++++");
Information("build.cake target: " + target);
Information("build.cake configuration: " + configuration);
Information("build.cake test category: " + testCategory);
Information("+++++++++++++++++++++++++++++++++++++++++++++");

ParseParams();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


// ************** Clean **************
Task("Clean")
    .Does(() =>
	{
		LogStartMessage($"cleaning directories");
		CleanDirectories("./Code/**/bin");
		CleanDirectories("./Code/**/obj");
		LogFinishedMessage($"cleaning directories");

		LogStartMessage("cleaning solution");

		var settings = new MSBuildSettings()
		{
			Verbosity = verbosity,
			ToolPath = msBuildPathX64,
			PlatformTarget = PlatformTarget.MSIL,
			Configuration = configuration
		};
		MSBuild("./ShipWorks.sln", 
			settings.WithTarget("Clean"));
	
		LogFinishedMessage("cleaning solution");
	});

// ************** Restore-NuGet-Packages **************
Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
	{
		LogStartMessage("NuGetRestore");
		NuGetRestore("./ShipWorks.sln", new NuGetRestoreSettings { Verbosity = NuGetVerbosity.Quiet });
		LogFinishedMessage("NuGetRestore");
	});

// ************** Build **************
Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
	{
		LogStartMessage("build");

		var settings = new MSBuildSettings()
		{
			Verbosity = verbosity, // Verbosity.Diagnostic
			// ToolVersion = MSBuildToolVersion.VS2017,
			ToolPath = msBuildPathX64,
			PlatformTarget = PlatformTarget.MSIL,
			Configuration = configuration
		};
		MSBuild("./ShipWorks.sln", settings
				.WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors));

		LogFinishedMessage("build");
	});

// ************** BuildForCI **************
Task("BuildForCI")
    .IsDependentOn("ReplaceInstanceID")
	.Does(() => 
	{ 
		LogStartMessage("BuildForCI");
		RunTarget("Build");
		LogFinishedMessage("BuildForCI");	
	});

// ************** BuildQuick **************
Task("BuildQuick")
	.Does(() => 
	{ 
		LogStartMessage("BuildQuick");

		treatWarningsAsErrors = "false"; 
		configuration = "Debug (No Analyzers)";
		SetBuildDir();

		RunTarget("Build");

		LogFinishedMessage("BuildQuick");	
	});

// ************** BuildDebug32 **************
Task("BuildDebug32")
	.Does(() => 
	{ 
		LogStartMessage("BuildDebug32");

		configuration = "DebugNoAnalyzers";
		SetBuildDir();

		RunTarget("Build");
		Make32Bit();

		LogFinishedMessage("BuildDebug32");	
	});

// ************** BuildRelease **************
Task("BuildRelease")
	.Does(() => 
	{ 
		LogStartMessage("BuildRelease");

		configuration = "Release";
		SetBuildDir();

		RunTarget("Build");

		LogFinishedMessage("BuildRelease");	
	});

// ************** DebugInstaller **************
Task("DebugInstaller")
	.Does(() => 
	{ 
		LogStartMessage("DebugInstaller");
		
		CreateDebugInstaller();

		LogFinishedMessage("DebugInstaller");	
	});

// ************** DebugInstaller **************
Task("InternalInstaller")
    .IsDependentOn("Restore-NuGet-Packages")
	.Does(() => 
	{ 
		LogStartMessage("InternalInstaller");
		
		CreateInternalInstaller();

		LogFinishedMessage("InternalInstaller");	
	});

// ************** DebugInstaller **************
Task("PublicInstaller")
    .IsDependentOn("Restore-NuGet-Packages")
	.Does(() => 
	{ 
		LogStartMessage("PublicInstaller");
		
		CreatePublicInstaller();

		LogFinishedMessage("PublicInstaller");	
	});

// ************** ReplaceInstanceID **************
Task("ReplaceInstanceID")
    .Does(() =>
	{
		LogStartMessage("ReplaceInstanceID");
		
		string appConfigPath = "./Code/ShipWorks.Tests.Integration/App.config";
		System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
		xmlDoc.Load(appConfigPath);
		System.Xml.XmlNode node = xmlDoc.SelectSingleNode("//configuration/appSettings/add[@key='ShipWorksInstanceGuid']");
		node.Attributes["value"].Value = instanceID;
		xmlDoc.Save(appConfigPath);

		LogFinishedMessage("ReplaceInstanceID");
	});

// ************** DeleteOldUnitTestRuns **************
Task("DeleteOldUnitTestRuns")
    .Does(() =>
	{
		LogStartMessage("delete old unit test runs");
		DeleteFiles("./TestResults/units.xml");
		LogFinishedMessage("delete old unit test runs");
	});

// ************** Unit Tests **************
Task("TestUnits")
    .IsDependentOn("DeleteOldUnitTestRuns")
    .Does(() =>
	{
		LogStartMessage("unit tests");

		var settings = new MSBuildSettings()
		{
			Verbosity = Verbosity.Minimal, 
			ToolPath = msBuildPathX64,
			PlatformTarget = PlatformTarget.x86,
			Configuration = configuration,
		};
		MSBuild("./tests.msbuild", 
			settings
				.WithTarget("Units")
				.WithProperty("m", "4")
		);
	
		LogFinishedMessage("unit tests");
	});

// ************** Spec Tests **************
Task("TestSpecs")
    .IsDependentOn("DeleteOldUnitTestRuns")
    .Does(() =>
	{
		LogStartMessage("specs tests");

		var settings = new MSBuildSettings()
		{
			Verbosity = Verbosity.Quiet, 
			ToolPath = msBuildPathX64,
			PlatformTarget = PlatformTarget.x86,
			Configuration = configuration,
		};
		MSBuild("./tests.msbuild", 
		settings
			.WithTarget("Specs")
			.WithProperty("m", "4")
		);
	
		LogFinishedMessage("specs tests");
	});

// ************** DeleteOldUnitTestRuns **************
Task("DeleteOldIntegrationTestRuns")
    .Does(() =>
	{
		LogStartMessage("delete old integration test runs");
		DeleteFiles("./TestResults/integration.xml");
		LogFinishedMessage("delete old integration test runs");
	});

// ************** Integration Tests **************
Task("TestIntegration")
    .IsDependentOn("DeleteOldIntegrationTestRuns")
    .Does(() =>
	{
		LogStartMessage("integration tests");

		var settings = new MSBuildSettings()
		{
			Verbosity = verbosity,
			ToolPath = msBuildPathX64,
			PlatformTarget = PlatformTarget.x86,
			Configuration = configuration,
		};

		MSBuild("./tests.msbuild", 
			settings
				.WithTarget("Integration")
				.WithProperty("m", "5")
				.WithProperty("IncludeTraits", $"Category={testCategory}".Quote())
			);
		
		LogFinishedMessage("integration tests");
	});

// ************** Zip Layout Files **************
Task("ZipLayout")
    .Does(() =>
	{
		LogStartMessage("zip layout files");

		Zip(@".\Code\ShipWorks\ApplicationCore\Appearance\WindowLayoutDefault",
			@".\Code\ShipWorks\ApplicationCore\Appearance\WindowLayoutDefault.swl");
		
		LogFinishedMessage("zip layout files");
	});

// ************** Zip Template Files **************
Task("ZipTemplates")
    .Does(() =>
	{
		LogStartMessage("zip template files");

		Zip(@".\Code\ShipWorks.Res\Templates\Distribution\Source",
			@".\Code\ShipWorks.Res\Templates\Distribution\Source.zip");
		
		LogFinishedMessage("zip template files");
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

void ParseParams()
{
	Information("*************************************************************");
	Information(target);
	Information(configuration);
	Information("*************************************************************");
	
	// See if there was a configuration passed in.
	if (args.Contains("#"))
	{
		var argsWithConfigSplit = args.Split('#');

		if (argsWithConfigSplit[1]?.Trim().Length != 0)
		{
			configuration = argsWithConfigSplit[1].Trim();
		}
		args = argsWithConfigSplit[0];
	}

	// Now get the rest of the args
	if (args.Contains(":"))
	{
		var argsSplit = args.Split(':');
		if (argsSplit[1]?.Trim().Length != 0)
		{
			target = $"{argsSplit[0].Trim()}-{argsSplit[1].Trim()}";
		}
		else
		{
			target = argsSplit[0].Trim();
		}
	}

	// See if there was a test category filter
	if (target.Contains("["))
	{
		bracketParam = target.Split('[')[1];
		bracketParam = bracketParam.Substring(0, bracketParam.Length - 1);
		target = target.Split('[')[0].Trim();
	}	
	
	SetBuildDir();

	// Write out our running values	
	Information("        target: " + target);
	Information(" configuration: " + configuration);
	Information("      buildDir: " + buildDir);
	Information("    instanceID: " + instanceID);
	Information("bracketParam: " + bracketParam);
}

void SetBuildDir()
{
	buildDir = Directory("./Artifacts/Application"); // + Directory(configuration);
}

void Make32Bit()
{
	string corFlagsPath = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\CorFlags.exe";
	if (System.IO.File.Exists(corFlagsPath))
	{
		StartProcess(corFlagsPath, new ProcessSettings {
			Arguments = new ProcessArgumentBuilder()
				.Append(@".\Artifacts\Application\ShipWorks.exe")
				.Append("/32bit+")
				.Append("/Force")
			}
		);	
	}
	else
	{
		Information($"Error: could not find CorFlags.exe at '{corFlagsPath}'");	
	}	
}

void CreateDebugInstaller()
{
	System.IO.Directory.CreateDirectory("./Artifacts/Application/Win32");
	System.IO.Directory.CreateDirectory("./Artifacts/Application/x64");
		
	System.IO.File.Copy("./Components/Win32/ShipWorks.Native.dll", "./Artifacts/Application/Win32/ShipWorks.Native.dll", true);
	System.IO.File.Copy("./Components/x64/ShipWorks.Native.dll", "./Artifacts/Application/x64/ShipWorks.Native.dll", true);
		
	var schemaID = StartProcess("./Artifacts/Application/ShipWorks.exe", new ProcessSettings {
		Arguments = new ProcessArgumentBuilder()
			.Append("/c=getdbschemaversion")
			.Append("/type=required")
		}
	);	

	Information($"SchemaID: {schemaID}");
		
	StartProcess(innoPath, new ProcessSettings {
		Arguments = new ProcessArgumentBuilder()
			.Append($"Installer/ShipWorks.iss /O\"Artifacts/Installer\" /F\"ShipWorksSetup.Debug\" /DEditionType=\"Standard\" /DVersion=\"0.0.0.0\" /DAppArtifacts=\"../Artifacts/Application\" /DRequiredSchemaID=\"{schemaID}\"")
		}
	);	
		
	StartProcess(innoPath, new ProcessSettings {
		Arguments = new ProcessArgumentBuilder()
			.Append($@"certutil -hashfile .\Artifiacts\Install\ShipWorksSetup.Debug.exe sha256")
		}
	);	
}

void CreateInternalInstaller()
{
	Information($"bracketParam: '{bracketParam}'");	
	var labelForBuild = "0.0.0";
	if (!bracketParam.IsNullOrWhiteSpace())
	{
		labelForBuild = bracketParam.Trim();
	}
	Information($"labelForBuild: {labelForBuild}");

	string revisionNumber = GetRevisionNumber();
	labelForBuild = $"{labelForBuild}.{revisionNumber}";
	Information($"labelForBuild: {labelForBuild}");

	System.IO.File.WriteAllText(".build-label", labelForBuild);

	var settings = new MSBuildSettings()
	{
		Verbosity = verbosity, // Verbosity.Diagnostic Quiet
		ToolPath = msBuildPathX64,
		Configuration = "Release"
	};
	MSBuild("./Build/ShipWorks.proj", settings
			.WithProperty("TreatWarningsAsErrors", "False")
			.WithProperty("CreateInstaller", "true")
			.WithProperty("Tests", "None")
			.WithProperty("Obfuscate", "False")
			.WithProperty("ReleaseType", "Internal")
			.WithProperty("BuildType", "Automated")
			.WithProperty("ProjectRevisionFile", revisionFilePath)
			.WithProperty("CCNetLabel", labelForBuild)
			.WithProperty("Platform", "Mixed Platforms")
			);
}

void CreatePublicInstaller()
{
	Information($"bracketParam: '{bracketParam}'");	
	var labelForBuild = "0.0.0";
	if (!bracketParam.IsNullOrWhiteSpace())
	{
		labelForBuild = bracketParam.Trim();
	}
	Information($"labelForBuild: {labelForBuild}");

	string revisionNumber = GetRevisionNumber();
	labelForBuild = $"{labelForBuild}.{revisionNumber}";
	Information($"labelForBuild: {labelForBuild}");

	System.IO.File.WriteAllText(".build-label", labelForBuild);

	var settings = new MSBuildSettings()
	{
		Verbosity = verbosity, // Verbosity.Diagnostic Quiet
		ToolPath = msBuildPathX64,
		Configuration = "Release"
	};
	MSBuild("./Build/ShipWorks.proj", settings
			.WithProperty("TreatWarningsAsErrors", "False")
			.WithProperty("CreateInstaller", "true")
			.WithProperty("Tests", "None")
			.WithProperty("Obfuscate", "True")
			.WithProperty("ReleaseType", "Public")
			.WithProperty("BuildType", "Automated")
			.WithProperty("ProjectRevisionFile", revisionFilePath)
			.WithProperty("CCNetLabel", labelForBuild)
			.WithProperty("Platform", "Mixed Platforms")
			.WithProperty("PackageModules", "True")
			);
}

string GetRevisionNumber()
{
	return System.IO.File.ReadAllText(revisionFilePath);
}

// Log START message
void LogStartMessage(string msg)
{
	LogMessage($"Starting {msg}");
}

// Log FINISHED message
void LogFinishedMessage(string msg)
{
	LogMessage($"Finished {msg}");
}

// Log a message
void LogMessage(string msg)
{
	Information("{0} at ({1})", msg, DateTime.Now.TimeOfDay.ToString());
}

/// <summary>
/// Indicates whether a specified string is null, empty, or consists only of white-space characters.
/// </summary>
public static bool IsNullOrWhiteSpace(this string s)
{
    return s?.Trim().Length == 0;
}