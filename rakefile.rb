require 'albacore'


Albacore.configure do |config|
	config.msbuild do |msbuild|
		msbuild.use :net40
		msbuild.parameters = "/m:3"
		msbuild.solution = "ShipWorks.sln"		# Assumes rake will be executed from the directory containing the rakefile and solution file
	end

	config.mstest do |mstest|
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 11.0\\Common7\\IDE\\mstest.exe"
	end
end

@innoPath = "C:/Program Files (x86)/Inno Setup 5/ISCC.EXE"

# The path to the file containing the next revision number to use in the version (i.e. 4567 would result in a version of x.x.x.4567)
@revisionFilePath = "\\\\INTFS01\\Development\\CruiseControl\\Configuration\\Versioning\\ShipWorks\\NextRevision.txt"

desc "Cleans and builds the solution with the debug config"
task :rebuild => ["build:clean", "build:debug"]

########################################################################
## Tasks to build in debug and release modes (using Albacore library)
########################################################################
namespace :build do
	desc "Cleans the ShipWorks solution"
	msbuild :clean do |msb|
		print "Cleaning solution...\r\n\r\n"
		msb.targets :Clean
	end

	desc "Build ShipWorks in the Debug configuration"
	msbuild :debug do |msb|
		print "Building solution with the debug config...\r\n\r\n"

		msb.properties :configuration => :Debug
		msb.targets :Build
	end

	desc "Build ShipWorks in the Release configuration"
	msbuild :release do |msb|
		print "Building solution with the release config...\r\n\r\n"

		msb.properties :configuration => :Release
		msb.targets :Clean, :Build
	end
	
	desc "Build ShipWorks.Native in a given configuration and platform"
	msbuild :native, [:configuration, :platform] do |msb, args|
		print "Building the Native project with the #{args[:configuration]}|#{args[:platform]} config...\r\n\r\n"

		msb.solution = "Code/ShipWorks.Native/Native.vcxproj"
		msb.properties args
		msb.targets :Build
	end
		
	desc "Build an unsigned Debug installer for local testing"
	task :debug_installer => :debug do
		print "Building unsigned debug installer package...\r\n\r\n"

		Rake::Task['build:native'].instance_exec do
			invoke "Debug", "Win32"
			reenable
			invoke "Debug", "x64"
		end

		print "\r\nCopying Native dlls to Artifacts... "
		FileUtils.mkdir_p "Artifacts/Application/Win32"
		FileUtils.cp "Code/ShipWorks.Native/Win32/Debug/ShipWorks.Native.dll", "Artifacts/Application/Win32"
		FileUtils.mkdir_p "Artifacts/Application/x64"
		FileUtils.cp "Code/ShipWorks.Native/x64/Debug/ShipWorks.Native.dll", "Artifacts/Application/x64"
		print "done.\r\n"

		print "Querying required schema version... "
		print schemaID = `Build/echoerrorlevel.cmd "Artifacts\\Application\\ShipWorks.exe" /c=getdbschemaversion /type=required`

		print "Running INNO compiler... "
		`"#{@innoPath}" Installer/ShipWorks.iss /O"Artifacts/Installer" /F"ShipWorksSetup" /DEditionType="Standard" /DVersion="0.0.0.0" /DAppArtifacts="../Artifacts/Application" /DRequiredSchemaID="#{schemaID}"`
		FileUtils.rm_f "InnoSetup.iss"
		print "done.\r\n"
	end

	desc "Build ShipWorks and generate an MSI for internal testing. Usage: internal_installer[3.5.2] to label with a specific major/minor/patch number; otherwise 0.0.0 will be used"
	msbuild :internal_installer, :versionLabel do |msb, args|
		print "Building internal release installer...\r\n\r\n"

		# Default the build label to 0.0.0
		labelForBuild = "0.0.0"
		
		if args != nil and args.versionLabel != nil and args.versionLabel != ""
			# A label was passed in, so use it for the Major.Minor.Patch 
			labelForBuild = args.versionLabel
		end
		
		# Use the MSBuild project when building the installer
		msb.solution = "./Build/shipworks.proj"
		msb.properties :configuration => :Release

		# Grab the revision number to use for this build
		revisionFile = File.open(@revisionFilePath)
		revisionNumber = revisionFile.readline
		revisionFile.close

		# Append the revision number to the label 
		labelForBuild = labelForBuild + "." + revisionNumber
		print "Building with label " + labelForBuild + "\r\n\r\n"
		
		# Use the revisionNumber extracted from the file and pass the revision filename
		# so the build will increment the version in preperation for the next run
		msb.parameters = "/p:CreateInstaller=True /p:Tests=None /p:Obfuscate=False /p:ReleaseType=Internal /p:BuildType=Automated /p:ProjectRevisionFile=" + @revisionFilePath + " /p:CCNetLabel=" + labelForBuild
	end
	
	desc "Build ShipWorks and generate a public installer"
	msbuild :public_installer, :versionLabel do |msb, args|
		print "Building an installer for the public release...\r\n\r\n"

		# Default the build label to 0.0.0
		labelForBuild = "0.0.0"
		
		if args != nil and args.versionLabel != nil and args.versionLabel != ""
			# A label was passed in, so use it for the Major.Minor.Patch 
			labelForBuild = args.versionLabel
		end
		
		# Use the MSBuild project when building the installer
		msb.solution = "./Build/shipworks.proj"
		msb.properties :configuration => :Release

		# Grab the revision number to use for this build
		revisionFile = File.open(@revisionFilePath)
		revisionNumber = revisionFile.readline
		revisionFile.close

		# Append the revision number to the label 
		labelForBuild = labelForBuild + "." + revisionNumber
		print "Building with label " + labelForBuild + "\r\n\r\n"
		
		# Use the revisionNumber extracted from the file and pass the revision filename
		# so the build will increment the version in preparation for the next run
		msb.parameters = "/p:CreateInstaller=True /p:Tests=None /p:Obfuscate=True /p:ReleaseType=Public /p:BuildType=Automated /p:ProjectRevisionFile=" + @revisionFilePath + " /p:CCNetLabel=" + labelForBuild
	end
end

########################################################################
## Tasks to run unit tests with MsTest (using Albacore library)
########################################################################
namespace :test do

	desc "Execute all unit tests and integration tests"
	task :all do 
		puts "Starting ShipWorks unit tests...\r\n\r\n"
		Rake::Task['test:units'].execute
		
		puts "Starting ShipWorks integration tests...\r\n\r\n"
		Rake::Task['test:integration'].execute
		
		# If we ever wanted to include UI/acceptance tests in the build we would add
		# another section below and uncomment the following two lines
		# puts "Starting ShipWorks acceptance tests...\r\n\r\n"
		# Rake::Task['test:acceptance'].execute
	end

	desc "Execute unit tests"
	mstest :units do |mstest|
		print "Deleting previous units results...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		File.delete("TestResults/units-results.trx") if File.exist?("TestResults/units-results.trx")

		print "Executing ShipWorks unit tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.parameters = "/testContainer:./Code/ShipWorks.Tests/bin/Debug/ShipWorks.Tests.dll", "/resultsfile:TestResults/units-results.trx"
	end	
	
	desc "Execute integration tests"
	mstest :integration do |mstest|
		print "Deleting previous result...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		File.delete("TestResults/integration-results.trx") if File.exist?("TestResults/integration-results.trx")
		
		print "Executing ShipWorks integrations tests...\r\n\r\n"
		mstest.parameters = "/testContainer:./Code/ShipWorks.Tests.Integration.MSTest/bin/Debug/ShipWorks.Tests.Integration.MSTest.dll", "/resultsfile:TestResults/integration-results.trx"
	end
end
