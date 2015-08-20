require 'albacore'
require 'win32/registry'
require "securerandom"
require 'date'
require 'fileutils'
require 'nokogiri'

def program_files
	ENV["PROGRAMFILES(x86)"] || ENV["PROGRAMFILES"]
end

Albacore.configure do |config|
	config.msbuild do |msbuild|
		msbuild.parameters = "/m:3"
		msbuild.solution = "ShipWorks.sln"		# Assumes rake will be executed from the directory containing the rakefile and solution file
		msbuild.command = "#{program_files}/MSBuild/14.0/Bin/msbuild.exe"
		#msbuild.properties = { TreatWarningsAsErrors: true }
	end
end

DATABASE_NAME = "ShipWorks_SeedData"
HOST_AND_INSTANCE_NAME = "#{ENV["COMPUTERNAME"]}\\Development"

@innoPath = "C:/Program Files (x86)/Inno Setup 5/ISCC.EXE"

# The path to the file containing the next revision number to use in the version (i.e. 4567 would result in a version of x.x.x.4567)
@revisionFilePath = "\\\\INTFS01\\Development\\CruiseControl\\Configuration\\Versioning\\ShipWorks\\NextRevision.txt"

desc "Cleans and builds the solution with the debug config"
task :rebuild, [:forCI] => ["build:clean", "build:debug"]

########################################################################
## Tasks to build in debug and release modes (using Albacore library)
########################################################################
namespace :build do
	desc "Restore nuget packages"
	task :restore do
		`build/nuget.exe restore ShipWorks.sln`
	end

	desc "Cleans the ShipWorks solution"
	msbuild :clean do |msb|
		print "Cleaning solution...\r\n\r\n"
		msb.targets :Clean
	end

	desc "Build ShipWorks in the Debug configuration"
	msbuild :debug, [:forCI] => "build:restore" do |msb, args|
		if args != nil and args.forCI != nil and args.forCI == 'true'			
			puts 'Updating config file for integration tests to run on CI server...'

			# Read in the app settings for the integration test project
			appConfigFilePath = Dir.pwd + "/Code/ShipWorks.Tests.Integration/App.config"
			
			replace_instance_guid appConfigFilePath, shipworks_instance_guid

			puts 'config file has been updated'
		end

		print "Building solution with the debug config...\r\n\r\n"

		msb.properties :configuration => :Debug, TreatWarningsAsErrors: true
		msb.targets :Build
	end

	desc "Build ShipWorks in the Release configuration"
	msbuild :release => ["build:clean", "build:restore"] do |msb|
		print "Building solution with the release config...\r\n\r\n"

		msb.properties :configuration => :Release, TreatWarningsAsErrors: true
		msb.targets :Build
	end
		
	desc "Build an unsigned Debug installer for local testing"
	task :debug_installer => :debug do
		print "Building unsigned debug installer package...\r\n\r\n"

		print "\r\nCopying Native dlls to Artifacts... "
		FileUtils.mkdir_p "Artifacts/Application/Win32"
		FileUtils.cp "Components/Win32/ShipWorks.Native.dll", "Artifacts/Application/Win32"
		FileUtils.mkdir_p "Artifacts/Application/x64"
		FileUtils.cp "Components/x64/ShipWorks.Native.dll", "Artifacts/Application/x64"
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
		labelForBuild = nil_if_empty(args[:versionLabel]) || "0.0.0"
		
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
		labelForBuild = nil_if_empty(args[:versionLabel]) || "0.0.0"
		
		# Use the MSBuild project when building the installer
		msb.solution = "./Build/shipworks.proj"
		msb.properties :configuration => :Release

		# Grab the revision number to use for this build
		revisionFile = File.open(@revisionFilePath)
		revisionNumber = revisionFile.readline
		revisionFile.close

		# Append the revision number to the label 
		labelForBuild = labelForBuild + "." + revisionNumber
		print "Building with label #{labelForBuild}\r\n\r\n"
		
		# Use the revisionNumber extracted from the file and pass the revision filename
		# so the build will increment the version in preparation for the next run
		msb.parameters = "/p:CreateInstaller=True /p:Tests=None /p:Obfuscate=True /p:ReleaseType=Public /p:BuildType=Automated /p:ProjectRevisionFile=#{@revisionFilePath} /p:CCNetLabel=#{labelForBuild}"
	end
end

########################################################################
## Tasks to run unit tests (using Albacore library)
########################################################################
namespace :test do

	desc "Execute all unit tests and integration tests"
	task :all => ["test:units", "test:integration"]

	desc "Execute unit tests"
	msbuild :units do |msbuild|
		# Delete results from any previous test runs
		DeleteOldTestRuns("units")
		
		print "Executing ShipWorks unit tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")

		msbuild.solution = "tests.msbuild"		# Assumes rake will be executed from the directory containing the rakefile and solution file
		msbuild.properties :configuration => :Debug
		msbuild.targets :Units
	end

	desc "Execute integration tests"
	msbuild :integration, [:categoryFilter] do |msbuild, args|
		# Delete results from any previous test runs
		DeleteOldTestRuns("integration")
		
		unless args.categoryFilter.nil? or args.categoryFilter.empty?
			# We need to filter the tests based on the categories provided
			#categoryParameter = "/category:" + args.categoryFilter
			msbuild.parameters = "/p:IncludeTraits=\"Category=#{args.categoryFilter}\""
			print "Category Parameter #{args.categoryFilter}"
		
		end
		
		print "Executing ShipWorks integrations tests...\r\n\r\n"

		#msbuild.parameters = "/m:1"
		msbuild.solution = "tests.msbuild"		# Assumes rake will be executed from the directory containing the rakefile and solution file
		msbuild.properties :configuration => :Debug
		msbuild.targets :Integration
	end
end

########################################################################
## Tasks for creating and seeding the database 
########################################################################
namespace :db do

	desc "Create, populate, and switch to a new ShipWorks database that is populated with seed data; useful for running locally"
	task :rebuild, [:schemaVersion, :instance, :targetDatabase] => [:create, :schema, :seed, :switch, :deploy]

	desc "Create and populate a new ShipWorks database with seed data. Intended to be executed in a build"
	task :populate, [:schemaVersion, :instance, :targetDatabase, :filePath] => [:create, :schema, :seed]
	
	desc "Drop and create the ShipWorks_SeedData database"
	task :create, [:instance, :targetDatabase] do |t, args|
		full_instance = get_instance_from_arguments args
		database_name = get_database_name_from_arguments args
		file_path = get_data_path_from_arguments args, full_instance[:instance]

		dropSqlText = "
			DECLARE @SQL varchar(max)

			-- Build the SQL to kill the all connections to @DatbaseName (Kill 54;Kill 56;...)
			SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
			FROM MASTER..SysProcesses
			WHERE DBId = DB_ID('#{database_name}') AND SPId <> @@SPId
			
			EXEC (@SQL)
			GO

			-- Now it's safe to drop the database without any open connections
			IF EXISTS (SELECT NAME FROM master.dbo.sysdatabases WHERE name = '#{database_name}')
				DROP DATABASE [#{database_name}] "

		execute_sql full_instance, dropSqlText, "Drop seed database"

		# Use the create database in the ShipWorks project, to guarantee it is the same as the one used 
		# by the ShipWorks application
		sqlText = File.read("./Code/ShipWorks/Data/Administration/Scripts/Installation/CreateDatabase.sql")
			.gsub(/{DBNAME}/, database_name)
			.gsub(/{FILEPATH}/, file_path)
			.gsub(/{FILENAME}/, database_name)
		
		execute_sql full_instance, sqlText, "Create seed database"
	end

	desc "Build the ShipWorks_SeedData database schema from scratch"
	task :schema, [:schemaVersion, :instance, :targetDatabase] do |t, args|
		full_instance = get_instance_from_arguments args
		database_name = get_database_name_from_arguments args
		schema_version = get_schema_version_from_arguments args

		# We're going to use the schema script in the ShipWorks project, but we're going to write it
		# to a temporary file, so we can tell prefix the script to use our seed database
		sqlText = "
		USE #{database_name}
		GO
		"
		
		sqlText.concat(File.read("./Code/ShipWorks/Data/Administration/Scripts/Installation/CreateSchema.sql"))

		sqlText.concat("
		        CREATE PROCEDURE [dbo].[GetSchemaVersion] 
                
                AS 
                SELECT '#{schema_version}' AS 'SchemaVersion'
				GO
		")

		execute_sql full_instance, sqlText, "Create seed schema"
	end

	desc "Populate the ShipWorks_SeedData database with order, shipment, and carrier account data"
	task :seed, [:instance, :targetDatabase] do |t, args|
		full_instance = get_instance_from_arguments args
		database_name = get_database_name_from_arguments args
		
		# We're going to write the static seed data to a temporary file, so we can tell prefix the script 
		# to use our seed database
		sqlText = "
		USE {DBNAME}
		GO
		"
		sqlText = sqlText.sub(/{DBNAME}/, database_name)
		
		# Concatenate the script containing our seed data to the string
		sqlText.concat(File.read("./SeedData.sql"))

		execute_sql full_instance, sqlText, "Seed data"
	end
	
	desc "Switch the ShipWorks settings to point to a given database"
	task :switch, [:instance, :targetDatabase] do |t, args|
		full_instance = get_instance_from_arguments args
		database_name = get_database_name_from_arguments args
		
		guid = shipworks_instance_guid

		if guid != ""
			puts "Found an instance GUID: #{guid}"
			fileName = "C:\\ProgramData\\Interapptive\\ShipWorks\\Instances\\#{guid}\\Settings\\sqlsession.xml"
			
			puts "Updating SQL session file..."	
			modify_xml fileName do |xml|
				# Get the connection string we'll be using from the test config file
				xml.xpath("//SqlSession/Server/Instance")[0].content = full_instance[:server]
				xml.xpath("//SqlSession/Server/Database")[0].content = database_name
			end
		end
	end
	
	desc "Deploy assemblies to the given database"
	task :deploy do |t, args|
		command = ".\\Artifacts\\Application\\ShipWorks.exe \/cmd:redeployassemblies"
		sh command
	end

	desc "Regenerate filters to the given database"
	task :deploy do |t, args|
		command = ".\\Artifacts\\Application\\ShipWorks.exe \/cmd:regenerateallfilters"
		sh command
	end
end

namespace :setup do

	desc "Creates ShipWorks entry in the registry based on the path to the ShipWorks.exe provided"
	task :registry, :instancePath do |t, args|
	
		instanceGuid = shipworks_instance_guid
		
		if instanceGuid
			# No instance GUID was found in the registry, so we need to create an entry for this path provided
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
	end
	
	desc "Creates/writes the SQL session file for the given instance to point at the target database provided"
	task :sqlSession, :instancePath, :targetDatabase do |t, args|
		instanceGuid = shipworks_instance_guid
		
		instanceName = "(local)\\Development"
		if args[:instanceName] != nil and args[:instanceName] != ""
			# Default the instance name to run on the local machine's Development instance if 
			# none is provided
			instanceName = args[:instanceName]
			puts "Changed instance name to " + instanceName
		end

		# Write out some boiler plate XML that will contain the database name provided
		boilerPlateXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>
<SqlSession>
  <Server>
    <Instance>@@INSTANCE_NAME@@</Instance>
    <Database>@@DATABASE_NAME@@</Database>
  </Server>
  <Credentials>
    <Username />
    <Password>dgPE4WpwFQg=</Password>
    <WindowsAuth>True</WindowsAuth>
  </Credentials>
</SqlSession>"
		
		boilerPlateXml = boilerPlateXml.gsub(/<Instance>@@INSTANCE_NAME@@<\/Instance>/, '<Instance>' + instanceName + '</Instance>')
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

namespace :launch do
	desc "Launches Shipworks"

	task :shipworks do 
		puts "Building Shipworks in debug mode...\r\n\r\n"
		Rake::Task['build:debug'].execute

		puts "Launching Shipworks...\r\n\r\n"
		command = "start .\\Artifacts\\Application\\ShipWorks.exe"
		sh command
	end
end

private 

def modify_xml(file)
	destination_xml = File.open file do |f|
		Nokogiri::XML(f)
	end

	yield destination_xml
	
	File.open file, "w" do |f|
		f << destination_xml.to_s
	end
end

def replace_instance_guid(app_config_path, instanceGuid)
	modify_xml app_config_path do |xml|
		# Get the connection string we'll be using from the test config file
		xml.xpath("//configuration/appSettings/add[@key='ShipWorksInstanceGuid']").first["value"] = instanceGuid
	end
end

def shipworks_instance_guid
	# Assume we're in the directory containing the ShipWorks solution - we need to get
	# the registry key name based on the directory to the ShipWorks.exe to figure out
	# which GUID to use in our path to the the SQL session file.
	app_directory = (Dir.pwd + "/Artifacts/Application").gsub('/', '\\')
	
	# Read the GUID from the registry, so we know which directory to look in; pass in 
	# 0x100 to read from 64-bit registry otherwise the key will not be found			
	keyName = "SOFTWARE\\Interapptive\\ShipWorks\\Instances"			
	Win32::Registry::HKEY_LOCAL_MACHINE.open(keyName, Win32::Registry::KEY_READ | 0x100) do |reg|
		begin
			reg[app_directory]
		rescue
			nil
		end
	end
end

def trace_output(message)
	Rake.application.trace message if Rake.application.options.trace
end

def nil_if_empty(value)
	value.nil? || value.empty? ? nil : value
end

def get_instance_from_arguments(args)
	host_and_instance_name = nil_if_empty(args[:instance]) ||  HOST_AND_INSTANCE_NAME
	
	pieces = host_and_instance_name.split "\\"
	server = host = pieces[0]
	instance = pieces[1]
	server += "\\" + instance if instance

	trace_output "*** Instance: #{server}"

	{ host: host, instance: (instance || "MSSqlServer"), server: server }
end

def get_database_name_from_arguments(args)
	database_name = nil_if_empty(args[:targetDatabase]) || DATABASE_NAME
	trace_output "*** Database: #{database_name}"
	
	database_name
end

def get_data_path_from_arguments(args, instance_name)
	file_path = nil_if_empty(args[:filePath]) || get_sql_data_path(instance_name)
	trace_output "*** Sql data path: #{file_path}"

	file_path
end

def get_schema_version_from_arguments(args)
	schema_version = nil_if_empty(args[:schemaVersion]) || latest_schema_version
	trace_output "*** Schema version: #{schema_version}"

	schema_version
end

def latest_schema_version
	latest_path = newest_version "", [0, 0], /(\d+)\.(\d+)/
	newest_version latest_path, [0,0,0,0], /(\d*)\.(\d*)\.(\d*)\.(\d*)\.sql/
end

def newest_version(sub_directory, initial_version, version_pattern)
	Dir.entries("code/ShipWorks/Data/Administration/Scripts/Update/#{sub_directory}")
		.map { |item| version_array item, version_pattern }
		.inject(initial_version) { |latest, version| (version <=> latest) == 1 ? version : latest }
		.join "."
end

def version_array(item, pattern)
	match = item.match pattern
	match.to_a.slice(1..-1).map { |item| item.to_i } unless match.nil?
end

def get_sql_data_path(instance)
	keyPath = nil
	keyName = "SOFTWARE\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL"
	Win32::Registry::HKEY_LOCAL_MACHINE.open(keyName, Win32::Registry::KEY_READ | 0x100) do |reg|
		keyPath = reg[instance]
	end

	keyName = "SOFTWARE\\Microsoft\\Microsoft SQL Server\\#{keyPath}\\Setup"
	Win32::Registry::HKEY_LOCAL_MACHINE.open(keyName, Win32::Registry::KEY_READ | 0x100) do |reg|
		reg["SQLDataRoot"] + "\\Data\\"
	end
end

def delete_if_exists(path)
	File.delete(path) if File.exist?(path)
end

def execute_sql(full_instance, sql, info)
	puts info
	name = info.gsub(/ /, "_")

	path = "./#{name}.sql"
	delete_if_exists path
	
	File.open(path, "w") {|file| file.puts sql}
	
	# Run the sqlcomd.exe with the -b option, so any errors will be denoted in the 
	# exit code and $?.success which is checked later
	command = "sqlcmd -S #{full_instance[:server]} -i #{path} -b"
	system(command)

	delete_if_exists path

	abort "Failed executing #{info}." if !$?.success?
end

def DeleteOldTestRuns(testType)
	# Delete the actual file containing the test results from a previous run
	print "Deleting previous #{testType} results...\r\n\r\n"
	Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
	delete_if_exists "TestResults/#{testType}-results.trx"
	delete_if_exists "TestResults/#{testType}.xml"
		
	# Delete previous test result directories to keep disk space under control otherwise
	# there could be GBs of test result files hanging around since each test run contains 
	# the ShipWorks binaries (this results in 100+ MB of space being # reclaimed for each 
	# test run that gets deleted)
	print "Deleting test results older than 4 days...\r\n"
	deletedCount = 0
	Dir["TestResults/*/"].map do |d|
		if (File.stat(d).mtime < (DateTime.now - 4).to_time)
			puts "Deleting " + d + "\r\n" 
			FileUtils.rm_r d
			deletedCount = deletedCount + 1
		end
	end
	print "Deleted the results for #{deletedCount} previous test run(s).\r\n\r\n"
end