require 'albacore'
require 'win32/registry'
require "securerandom"

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
	
	desc "Runs code analysis"
	msbuild :analyze do |msb|
		print "Running code analysis...\r\n\r\n"
		
		msb.verbosity = "quiet"
		msb.properties :configuration => :Debug, :RunCodeAnalysis => true
		msb.parameters = "/p:warn=0"
		msb.targets :Build
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

		# Grab the revision number to use for this build
		revisionFile = File.open(@revisionFilePath)
		revisionNumber = revisionFile.readline
		revisionFile.close
		
		print "Running INNO compiler... "
		`"#{@innoPath}" Installer/ShipWorks.iss /O"Artifacts/Installer" /F"ShipWorksSetup.Debug" /DEditionType="Standard" /DVersion="0.0.0.0" /DAppArtifacts="../Artifacts/Application" /DRequiredSchemaID="#{schemaID}"`
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
		# so the build will increment the version in preparation for the next run
		
		# NOTE: The ReleaseType=Internal parameter will cause the assemblies/installer to 
		# be signed; this will fail without the correct certificate
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
		mstest.parameters = "/noisolation", "/detail:errormessage", "/testContainer:./Code/ShipWorks.Tests/bin/Debug/ShipWorks.Tests.dll", "/resultsfile:TestResults/units-results.trx"
	end	
	
	desc "Execute integration tests"
	mstest :integration, :categoryFilter do |mstest, args|
		print "Deleting previous result...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		File.delete("TestResults/integration-results.trx") if File.exist?("TestResults/integration-results.trx")
		
		categoryParameter = ""
		if args != nil and args.categoryFilter != nil and args.categoryFilter != ""
			# We need to filter the tests based on the categories provided
			categoryParameter = "/category:" + args.categoryFilter
		end
		
		puts categoryParameter
		
		print "Executing ShipWorks integrations tests...\r\n\r\n"
		mstest.parameters = "/testContainer:./Code/ShipWorks.Tests.Integration.MSTest/bin/Debug/ShipWorks.Tests.Integration.MSTest.dll", "/detail:stdout", "/detail:stderr", categoryParameter, "/resultsfile:TestResults/integration-results.trx"
	end
end


########################################################################
## Tasks for creating and seeding the database 
########################################################################
namespace :db do

	desc "Create and populate a new ShipWorks database with seed data"
	task :rebuild, [:schemaVersion, :targetDatabase] => [:create, :schema, :seed, :switch, :deploy] do |t, args|
	end

	desc "Drop and create the ShipWorks_SeedData database"
	task :create do

		# Drop the seed database if it exists
		File.delete("./DropSeedDatabase.sql") if File.exist?("./DropSeedDatabase.sql")
		
		print "Killing all connections and dropping database...\r\n"
		dropSqlText = "
			DECLARE @DatabaseName nvarchar(50)
			SET @DatabaseName = N'ShipWorks_SeedData'

			DECLARE @SQL varchar(max)

			SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
			FROM MASTER..SysProcesses
			WHERE DBId = DB_ID(@DatabaseName) AND SPId <> @@SPId
			
			EXEC (@SQL)
			GO


			IF EXISTS (SELECT NAME FROM master.dbo.sysdatabases WHERE name = N'ShipWorks_SeedData')
				DROP DATABASE [ShipWorks_SeedData] "

		File.open("./DropSeedDatabase.sql", "w") {|file| file.puts dropSqlText}
		sh "sqlcmd -S (local) -i DropSeedDatabase.sql"

		File.delete("./DropSeedDatabase.sql") if File.exist?("./DropSeedDatabase.sql")
		

		# We're good to create a new seed database
		puts "Creating the database...\r\n"
		File.delete("./CreateSeedDatabase.sql") if File.exist?("./CreateSeedDatabase.sql")
		
		# Use the create database in the ShipWorks project, to guarantee it is the same as the one used 
		# by the ShipWorks application
		sqlText = File.read("./Code/ShipWorks/Data/Administration/Scripts/Installation/CreateDatabase.sql")
		sqlText = sqlText.gsub(/{DBNAME}/, "ShipWorks_SeedData")
		sqlText = sqlText.gsub(/{FILEPATH}/, "C:\\Program Files\\Microsoft SQL Server\\MSSQL10_50.DEVELOPMENT\\MSSQL\\DATA\\")
		sqlText = sqlText.gsub(/{FILENAME}/, "ShipWorks_SeedData")

		# Write the script to disk, so we can execute it via shell
		File.open("./CreateSeedDatabase.sql", "w") {|file| file.puts sqlText}
		sh "sqlcmd -S (local) -i CreateSeedDatabase.sql"

		# Clean up the temporary script
		File.delete("./CreateSeedDatabase.sql") if File.exist?("./CreateSeedDatabase.sql")
	end

	desc "Build the ShipWorks_SeedData database schema from scratch"
	task :schema, :schemaVersion do |t, args|
		puts "Creating the database schema..."
				
		versionForProcedure = "3.0.0.0"
		
		if args != nil and args[:schemaVersion] != nil and args[:schemaVersion] != ""
			# A schema version was passed in, so use it for the schema version procedure
			versionForProcedure = args[:schemaVersion]
		end
				
		# Clean up any remnants of the temporary script that may exist from a previous run
		File.delete("./CreateSeedSchema.sql") if File.exist?("./CreateSeedSchema.sql")

		# We're going to use the schema script in the ShipWorks project, but we're going to write it
		# to a temporary file, so we can tell prefix the script to use our seed database
		sqlText = "
		USE ShipWorks_SeedData
		GO
		"
		
		# Concatenate the schema script to our string
		sqlText.concat(File.read("./Code/ShipWorks/Data/Administration/Scripts/Installation/CreateSchema.sql"))

		sqlText.concat("
		        CREATE PROCEDURE [dbo].[GetSchemaVersion] 
                
                AS 
                SELECT '{SCHEMA_VERSION_VALUE}' AS 'SchemaVersion'
				GO
		")
		sqlText = sqlText.sub(/{SCHEMA_VERSION_VALUE}/, versionForProcedure)
		
		# Write our script to a temporary file and execute the SQL
		File.open("./CreateSeedSchema.sql", "w") {|file| file.puts sqlText}
		sh "sqlcmd -S (local) -i CreateSeedSchema.sql"

		# Clean up the temporary script
		File.delete("./CreateSeedSchema.sql") if File.exist?("./CreateSeedSchema.sql")
	end

	desc "Populate the ShipWorks_SeedData database with order, shipment, and carrier account data"
	task :seed do
		puts "Populating data..."

		# Clean up any remnants of the temporary script that may exist from a previous run
		File.delete("./TempSeedData.sql") if File.exist?("./TempSeedData.sql")

		# We're going to write the static seed data to a temporary file, so we can tell prefix the script 
		# to use our seed database
		sqlText = "
		USE ShipWorks_SeedData
		GO
		"
		
		# Concatenate the script containing our seed data to the string
		sqlText.concat(File.read("./SeedData.sql"))

		# Write our script to a temporary file and execute the SQL
		File.open("./TempSeedData.sql", "w") {|file| file.puts sqlText}
		sh "sqlcmd -S (local) -i TempSeedData.sql"

		# Clean up the temporary script
		File.delete("./TempSeedData.sql") if File.exist?("./TempSeedData.sql")
	end
	
	desc "Switch the ShipWorks settings to point to a given database"
	task :switch, :targetDatabase, :instanceName do |t, args|
		if args != nil and args[:targetDatabase] != nil and args[:targetDatabase] != ""			
			# A target database was passed in, so update the sql session file
			
			# Assume we're in the directory containing the ShipWorks solution - we need to get
			# the registry key name based on the directory to the ShipWorks.exe to figure out
			# which GUID to use in our path to the the SQL session file.
			appDirectory = Dir.pwd + "/Artifacts/Application"
			appDirectory = appDirectory.gsub('/', '\\')
			
			instanceName = "(local)\\Development"
			
			if args[:instanceName] != nil and args[:instanceName] != ""
				# Default the instance name to run on the local machine's Development instance if 
				# none is provided
				instanceName = args[:instanceName]
			end
			
			instanceGuid = ""		
			
			# Read the GUID from the registry, so we know which directory to look in; pass in 
			# 0x100 to read from 64-bit registry otherwise the key will not be found			
			keyName = "SOFTWARE\\Interapptive\\ShipWorks\\Instances"			
			Win32::Registry::HKEY_LOCAL_MACHINE.open(keyName, Win32::Registry::KEY_READ | 0x100) do |reg|
				instanceGuid = reg[appDirectory]				
			end
			
			if instanceGuid != ""
				puts "Found an instance GUID: " + instanceGuid
				fileName = "C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\" + instanceGuid + "\\Settings\\sqlsession.xml"
				
				puts "Updating SQL session file..."			
				
				# Replace the current instance and database name with that of the info provided
				contents = File.read(fileName)				
				contents = contents.gsub(/<Instance>.*<\/Instance>/, '<Instance>' + instanceName + '</Instance>')
				contents = contents.gsub(/<Database>[\w]*<\/Database>/, '<Database>' + args.targetDatabase + '</Database>')
				
				# Write the updated SQL session XML back to the file
				File.open(fileName, 'w') { |file| file.write(contents) }
				
				puts "Updated SQL session file is now..."			
				puts contents
			end
		else
			puts "Did not switch database used by ShipWorks: a target database was not specified."
		end
	end
	
	desc "Deploy assemblies to the given database"
	task :deploy do |t, args|
		command = ".\\Artifacts\\Application\\ShipWorks.exe \/cmd:redeployassemblies"
		sh command
	end
end

namespace :setup do

	desc "Creates ShipWorks entry in the registry based on the path to the ShipWorks.exe provided"
	task :registry, :instancePath do |t, args|
	
		instanceGuid = SecureRandom.uuid
		puts instanceGuid
		
		if args != nil and args[:instancePath] != nil and args[:instancePath] != ""
			
			# Create the ShipWorks instance value based on the registryKey name provided
			keyName = "SOFTWARE\\Interapptive\\ShipWorks\\Instances"		
			Win32::Registry::HKEY_LOCAL_MACHINE.open(keyName, Win32::Registry::KEY_WRITE | 0x100) do |reg|
				reg[args.instancePath] = '{' + instanceGuid + '}'
			end
		end		
	end
	
	desc "Creates/writes the SQL session file for the given instance to point at the target database provided"
	task :sqlSession, :instancePath, :targetDatabase do |t, args|
	
		instanceGuid = ""
		
		# Read the GUID from the registry; pass in 0x100 to read from 64-bit 
		# registry otherwise the key will not be found
		keyName = "SOFTWARE\\Interapptive\\ShipWorks\\Instances"			
		Win32::Registry::HKEY_LOCAL_MACHINE.open(keyName, Win32::Registry::KEY_READ | 0x100) do |reg|
			# Read the instance GUID from the registry
			instanceGuid = reg[args.instancePath]				
		end
			
		
		# Write out some boiler plate XML that will contain the database name provided
		boilerPlateXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>
<SqlSession>
  <Server>
    <Instance>(local)\\DEVELOPMENT</Instance>
    <Database>@@DATABASE_NAME@@</Database>
  </Server>
  <Credentials>
    <Username />
    <Password>dgPE4WpwFQg=</Password>
    <WindowsAuth>True</WindowsAuth>
  </Credentials>
</SqlSession>"
		boilerPlateXml = boilerPlateXml.gsub(/<Database>@@DATABASE_NAME@@<\/Database>/, '<Database>' + args.targetDatabase + '</Database>')
		
		# Make sure the directories are created before writing to the sqlsession file
		Dir.mkdir("C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\" + instanceGuid) if !Dir.exist?("C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\" + instanceGuid)
		Dir.mkdir("C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\" + instanceGuid + "\\Settings") if !Dir.exist?("C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\" + instanceGuid + "\\Settings")
		
		# Create/write the SQL Session file
		fileName = "C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\" + instanceGuid + "\\Settings\\sqlsession.xml"	
		sessionFile = File.new(fileName, 'w')
		sessionFile.puts(boilerPlateXml)
		sessionFile.close
	end
end