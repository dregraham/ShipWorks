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
	
	desc "Runs code analysis"
	msbuild :analyze do |msb|
		print "Running code analysis...\r\n\r\n"
		
		msb.verbosity = "normal"
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
	mstest :integration do |mstest|
		print "Deleting previous result...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		File.delete("TestResults/integration-results.trx") if File.exist?("TestResults/integration-results.trx")
		
		print "Executing ShipWorks integrations tests...\r\n\r\n"
		mstest.parameters = "/testContainer:./Code/ShipWorks.Tests.Integration.MSTest/bin/Debug/ShipWorks.Tests.Integration.MSTest.dll", "/resultsfile:TestResults/integration-results.trx"
	end
end


########################################################################
## Tasks for creating and seeding the database 
########################################################################
namespace :db do

	desc "Create and populate a new ShipWorks database with seed data"
	task :rebuild => [:create, :schema, :seed]	

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
	task :schema do
		puts "Creating the database schema..."
				
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
end