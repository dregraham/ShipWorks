require 'albacore'


# Location of MSBuild and MSTest on this computer
@msBuildPath = "#{ENV['SystemRoot']}\\Microsoft.NET\\Framework\\v4.0.30319\\msbuild.exe"
@msTestPath = "C:\\Program Files (x86)\\Microsoft Visual Studio 11.0\\Common7\\IDE\\mstest.exe"


# Assumes rake will be executed from the directory containing the rakefile and solution file
@workingDirectory = pwd
@solutionFile = "#{@workingDirectory}\\ShipWorks.sln"

@testMetadataFile = "#{@workingDirectory}\\ShipWorks.vsmdi"

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
		msb.solution = "#{@solutionFile}"
	end

	desc "Build ShipWorks in the Debug configuration"
	msbuild :debug do |msb|
		print "Building solution with the debug config...\r\n\r\n"

		msb.properties :configuration => :Debug
		msb.targets :Build
		msb.solution = "#{@solutionFile}"
	end


	desc "Build ShipWorks in the Release configuration"
	msbuild :release do |msb|
		print "Building solution with the release config...\r\n\r\n"

		msb.properties :configuration => :Release
		msb.targets :Clean, :Build
		msb.solution = "#{@solutionFile}"
	end
end

########################################################################
## Tasks to run unit tests with MsTest (using Albacore library)
########################################################################
namespace :test do

	desc "Execute all test lists"
	task :all do 
		puts "Executing ShipWorks unit tests"
		Rake::Task['test:units'].execute

		# If we ever wanted to include integration tests in the build we would uncomment
		# the following two lines and add a section for "integration" below. Until then
		# running rake test:all and rake test:units are equivalent.
		# puts "Executing ShipWorks integration tests"
		# Rake::Task['test:integration'].execute		
	end

	desc "Execute unit tests"
	mstest :units do |mstest|
		print "Deleting previous units results...\r\n\r\n"
		File.delete("TestResults/units-results.trx") if File.exist?("TestResults/units-results.trx")
		print "Executing ShipWorks unit tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testContainer:./Code/ShipWorks.Tests/bin/Debug/ShipWorks.Tests.dll", "/resultsfile:TestResults/units-results.trx"
	end	
end
