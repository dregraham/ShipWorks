#tool "nuget:?package=xunit.runner.console&version=2.4.0"
#tool "nuget:?package=vswhere&version=2.8.4"
//#r "tools/Cake/BuildSupport.dll"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

// Constants
var innoPath = @"C:\Program Files (x86)\Inno Setup 5\ISCC.EXE";
var revisionFilePath = @"\\intdev1201\NetworkShare\DevShare\BuildConfig\NextRevision.txt";
var devRevisionFilePath = @"\\intfs01\development\NetworkShare\DevShare\BuildConfig\NextRevision.txt";

FilePath msBuildPathX64 = SetBuildPath();

// Get the params argument that was passed in
// We default to Build and Debug in case no param is passed in
var args = ""; 
var target = Argument("Target", "Build");
var configuration = Argument("configuration", "Debug");
var bracketParam = "";
var treatWarningsAsErrors = "true";
var buildDir = "";
var instanceID = "";
var testCategory = "";
var verbosity = Verbosity.Quiet;

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

		// If rake commands are passed in, convert them to their cake
		// equivilents.
		if (targetRight != string.Empty)
		{
            switch (targetRight)
            {
                case "units":
                    target = "TestUnits";
                    break;
                case "specs":
                    target = "TestSpecs";
                    break;
                case "clean":
                    target = "Clean";
                    break;
                case "restore":
                    target = "RestoreNuGetPackages";
                    break;
                case "quick":
                    target = "BuildQuick";
                    break;
                case "debug32":
                    target = "BuildDebug32";
                    break;
                case "release":
                    target = "BuildRelease";
					configuration = "Release";
                    break;
                case "quiet":
                    verbosity = Verbosity.Quiet;
                    break;
                case "verbose":
                    verbosity = Verbosity.Verbose;
                    break;
                case string s when s.StartsWith("debug_installer"):
                    target = "DebugInstaller";
                    break;
                case string s when s.StartsWith("internal_installer"):
                    target = "InternalInstaller";
					configuration = "Release";
                    break;
                case string s when s.StartsWith("public_installer"):
                    target = "PublicInstaller";
					configuration = "Release";
                    break;
                case string s when s.StartsWith("integration"):
                    target = "TestIntegration";
                    testCategory = ParseBracketParam(targetRight);
                    break;
            }
		}

		bracketParam = ParseBracketParam(targetRight);
		// verbosity = Verbosity.Verbose;
	}
}

Information("+++++++++++++++++++++++++++++++++++++++++++++");
Information("build.cake target: " + target);
Information("build.cake configuration: " + configuration);
Information("build.cake test category: " + testCategory);
Information("+++++++++++++++++++++++++++++++++++++++++++++");

ParseParams();

/// <summary>
/// Clean
/// </summary>
Task("Clean")
    .Does(() =>
	{
		LogStartMessage($"cleaning directories");
		CleanDirectories("./Code/**/bin");
		CleanDirectories("./Code/**/obj");
		LogFinishedMessage($"cleaning directories");

		LogStartMessage("cleaning solution");

		var settings = CreateBuildSettings(configuration)
					   .WithTarget("Clean");
		
		MSBuild("./ShipWorks.sln", settings);
	
		LogFinishedMessage("cleaning solution");
	});

/// <summary>
/// RestoreNuGetPackages
/// </summary>
Task("RestoreNuGetPackages")
    .Does(() =>
	{
		LogStartMessage("NuGetRestore");
		NuGetRestore("./ShipWorks.sln", new NuGetRestoreSettings { Verbosity = NuGetVerbosity.Quiet });
		LogFinishedMessage("NuGetRestore");
	});

/// <summary>
/// Build
/// </summary>
Task("Build")
    .IsDependentOn("RestoreNuGetPackages")
    .Does(() =>
	{
		LogStartMessage("build");

		var settings = CreateBuildSettings(configuration);
		MSBuild("./ShipWorks.sln", settings);

		LogFinishedMessage("build");
	});

/// <summary>
/// BuildForCI
/// </summary>
Task("BuildForCI")
    .IsDependentOn("ReplaceInstanceID")
    .IsDependentOn("Clean")
	.Does(() => 
	{ 
		LogStartMessage("BuildForCI");
		RunTarget("Build");
		LogFinishedMessage("BuildForCI");	
	});

/// <summary>
/// BuildQuick
/// </summary>
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

/// <summary>
/// BuildDebug32
/// </summary>
Task("BuildDebug32")
	.Does(() => 
	{ 
		LogStartMessage("BuildDebug32");

		configuration = "Debug (No Analyzers)";
		SetBuildDir();

		RunTarget("Build");
		Make32Bit();

		LogFinishedMessage("BuildDebug32");	
	});

/// <summary>
/// BuildRelease
/// </summary>
Task("BuildRelease")
    .IsDependentOn("Clean")
	.Does(() => 
	{ 
		LogStartMessage("BuildRelease");

		configuration = "Release";
		SetBuildDir();

		RunTarget("Build");

		LogFinishedMessage("BuildRelease");	
	});

/// <summary>
/// DebugInstaller
/// </summary>
Task("DebugInstaller")
    .IsDependentOn("Clean")
    .IsDependentOn("RestoreNuGetPackages")
	.Does(() => 
	{ 
		LogStartMessage("DebugInstaller");
		
		CreateDebugInstaller();

		LogFinishedMessage("DebugInstaller");	
	});

/// <summary>
/// InternalInstaller
/// </summary>
Task("InternalInstaller")
    .IsDependentOn("Clean")
    .IsDependentOn("RestoreNuGetPackages")
	.Does(() => 
	{ 
		LogStartMessage("InternalInstaller");
		
		CreateInstaller("Internal", false, false);

		LogFinishedMessage("InternalInstaller");	
	});

/// <summary>
/// PublicInstaller
/// </summary>
Task("PublicInstaller")
    .IsDependentOn("Clean")
    .IsDependentOn("RestoreNuGetPackages")
	.Does(() => 
	{ 
		LogStartMessage("PublicInstaller");
		
		CreateInstaller("Public", true, true);

		LogFinishedMessage("PublicInstaller");	
	});

/// <summary>
/// ReplaceInstanceID
/// </summary>
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

/// <summary>
/// DeleteOldUnitTestRuns
/// </summary>
Task("DeleteOldUnitTestRuns")
    .Does(() =>
	{
		LogStartMessage("delete old unit test runs");
		DeleteFiles("./TestResults/units.xml");
		LogFinishedMessage("delete old unit test runs");
	});

/// <summary>
/// DeleteOldSpecTestRuns
/// </summary>
Task("DeleteOldSpecTestRuns")
    .Does(() =>
	{
		LogStartMessage("delete old unit test runs");
		DeleteFiles("./TestResults/specs.xml");
		LogFinishedMessage("delete old unit test runs");
	});

/// <summary>
/// Unit Tests
/// </summary>
Task("TestUnits")
    .IsDependentOn("DeleteOldUnitTestRuns")
    .Does(() =>
	{
		LogStartMessage("unit tests");
		
		var settings = CreateBuildSettings(configuration)
					   .WithTarget("Units");
		MSBuild("./tests.msbuild", settings);
	
		LogFinishedMessage("unit tests");
	});

/// <summary>
/// Spec Tests
/// </summary>
Task("TestSpecs")
    .IsDependentOn("DeleteOldSpecTestRuns")
    .Does(() =>
	{
		LogStartMessage("specs tests");
		
		var settings = CreateBuildSettings(configuration)
					   .WithTarget("Specs");
		settings.Verbosity = Verbosity.Quiet;
		MSBuild("./tests.msbuild", settings);
	
		LogFinishedMessage("specs tests");
	});

/// <summary>
/// DeleteOldIntegrationTestRuns
/// </summary>
Task("DeleteOldIntegrationTestRuns")
    .Does(() =>
	{
		LogStartMessage("delete old integration test runs");
		DeleteFiles("./TestResults/integration.xml");
		LogFinishedMessage("delete old integration test runs");
	});

/// <summary>
/// Integration Tests
/// </summary>
Task("TestIntegration")
    .IsDependentOn("DeleteOldIntegrationTestRuns")
    .Does(() =>
	{
		LogStartMessage("integration tests");

		if (testCategory.ToLower() == string.Empty)
		{
			testCategory = "ContinuousIntegration";
		}

		verbosity = Verbosity.Verbose;
		var settings = CreateBuildSettings(configuration)
					   .WithTarget("Integration");
		MSBuild("./tests.msbuild", settings);
		
		LogFinishedMessage("integration tests");
	});

/// <summary>
/// Zip Layout Files
/// </summary>
Task("ZipLayout")
    .Does(() =>
	{
		LogStartMessage("zip layout files");

		Zip(@".\Code\ShipWorks\ApplicationCore\Appearance\WindowLayoutDefault",
			@".\Code\ShipWorks\ApplicationCore\Appearance\WindowLayoutDefault.swl");
		
		LogFinishedMessage("zip layout files");
	});

/// <summary>
/// Zip Template Files
/// </summary>
Task("ZipTemplates")
    .Does(() =>
	{
		LogStartMessage("zip template files");

		Zip(@".\Code\ShipWorks.Res\Templates\Distribution\Source",
			@".\Code\ShipWorks.Res\Templates\Distribution\Source.zip");
		
		LogFinishedMessage("zip template files");
	});

/// <summary>
/// Default task
/// </summary>
Task("Default")
    .IsDependentOn("Build");

/// <summary>
/// Execute the target
/// </summary>
RunTarget(target);

/// <summary>
/// Parse the given value to extract the value contained within the brackets;
/// </summary>
string ParseBracketParam(string rightValue)
{
	if (rightValue.Contains("["))
	{
		return rightValue.Substring(rightValue.IndexOf("[") + 1, rightValue.Length - rightValue.IndexOf("[") - 2);
	}

	return string.Empty;
}

/// <summary>
/// Get and set the MS build path
/// </summary>
FilePath SetBuildPath()
{
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
	return msBuildPathX64;
}

/// <summary>
/// Parse and set params
/// </summary>
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

/// <summary>
/// Set the build output dir
/// </summary>
void SetBuildDir()
{
	buildDir = Directory("./Artifacts/Application");
}

/// <summary>
/// Create default build settings
/// </summary>
MSBuildSettings CreateBuildSettings(string configurationOverride)
{
	var settings = new MSBuildSettings()
	{
		Verbosity = verbosity,
		ToolPath = msBuildPathX64,
		Configuration = configurationOverride,
	};

	settings = settings.SetMaxCpuCount(4)
			.WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors);

	if (!testCategory.IsNullOrWhiteSpace())
	{
		settings = settings.WithProperty("IncludeTraits", $"Category={testCategory}".Quote());
	}

	return settings;
}

/// <summary>
/// Make the exe 32 bit
/// </summary>
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

/// <summary>
/// Create a debug installer
/// </summary>
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

/// <summary>
/// Create an installer
/// </summary>
void CreateInstaller(string releaseType, bool obfuscate, bool packageModules)
{
	//Information($"bracketParam: '{bracketParam}'");	
	var labelForBuild = bracketParam.Trim();
	Information($"labelForBuild: {labelForBuild}");

	//System.IO.File.WriteAllText(".build-label", labelForBuild);

	var settings = CreateBuildSettings("Release")
			.WithProperty("TreatWarningsAsErrors", "False")
			.WithProperty("CreateInstaller", "true")
			.WithProperty("Tests", "None")
			// .WithProperty("Obfuscate", obfuscate ? "True" : "False")			
			.WithProperty("Obfuscate",  "False")
			.WithProperty("ReleaseType", releaseType)
			.WithProperty("BuildType", "Automated")
			//.WithProperty("ProjectRevisionFile", revisionFilePath)
			.WithProperty("CCNetLabel", labelForBuild)
			.WithProperty("Platform", "Mixed Platforms");

	if (packageModules)
	{
		settings = settings
			.WithProperty("PackageModules", "True");
	}

	MSBuild("./Build/ShipWorks.proj", settings);

	var setupFilename = System.IO.Directory.GetFiles(@".\Artifacts\Distribute\", "ShipWorksSetup_*.exe").First();
	var sha = ComputeSHA256CheckSum(setupFilename);
	System.IO.File.WriteAllText(@".\Artifacts\Distribute\Sha256.txt", sha);
}

/// <summary>
/// Calculate the SHA256 value of the given file
/// </summary>
string ComputeSHA256CheckSum(string filename)
{
	using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
	{
		using (System.IO.FileStream fileStream = System.IO.File.OpenRead(filename))
		{
			byte[] hash = sha.ComputeHash(fileStream);
			return BitConverter.ToString(hash).Replace("-", string.Empty);
		}
	}
}

/// <summary>
/// Log START message
/// </summary>
void LogStartMessage(string msg)
{
	LogMessage($"Starting {msg}");
}

/// <summary>
/// Log FINISHED message
/// </summary>
void LogFinishedMessage(string msg)
{
	LogMessage($"Finished {msg}");
}

/// <summary>
/// Log a message
/// </summary>
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