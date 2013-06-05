require 'albacore'


# Location of MSBuild and MSTest on this computer
@msBuildPath = "#{ENV['SystemRoot']}\\Microsoft.NET\\Framework\\v4.0.30319\\msbuild.exe"
@msTestPath = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"


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
		puts "Executing eBay tests"
		Rake::Task['test:ebay'].execute

		puts "Executing stamps tests"
		Rake::Task['test:stamps'].execute
				
		puts "Executing SCAN form tests"
		Rake::Task['test:scanForms'].execute

		puts "Executing FedEx tests"
		Rake::Task['test:fedEx'].execute

		puts "Executing iParcel tests"
		Rake::Task['test:iParcel'].execute

		puts "Executing OnTrac tests"
		Rake::Task['test:onTrac'].execute

		puts "Executing UPS tests"
		Rake::Task['test:ups'].execute
	
		puts "Executing Scheduled Action tests"
		Rake::Task['test:scheduledAction'].execute		

		puts "Executing Newegg tests"
		Rake::Task['test:newegg'].execute
	end

	desc "Execute the eBay tests"
	mstest :ebay do |mstest|
		print "Deleting previous eBay test results...\r\n\r\n"
		File.delete("TestResults/ebay-results.trx") if File.exist?("TestResults/ebay-results.trx")
		print "Executing eBay tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"eBay Tests\"", "/resultsfile:TestResults/ebay-results.trx"
	end

	desc "Execute the stamps.com registration tests"
	mstest :stamps do |mstest|
		print "Deleting previous Stamps.com test results...\r\n\r\n"		
		File.delete("TestResults/stamps-results.trx") if File.exist?("TestResults/stamps-results.trx")
		print "Executing Stamps.com registration tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"Stamps Registration Tests\"", "/resultsfile:TestResults/stamps-results.trx"
	end
	
	desc "Execute the SCAN form tests"
	mstest :scanForms do |mstest|
		print "Deleting previous ScanForm test results...\r\n\r\n"		
		File.delete("TestResults/scanForm-results.trx") if File.exist?("TestResults/scanForm-results.trx")
		print "Executing SCAN form tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"ScanForm Tests\"", "/resultsfile:TestResults/scanForm-results.trx"
	end

	desc "Execute the FedEx tests"
	mstest :fedEx do |mstest|
		print "Deleting previous FedEx test results...\r\n\r\n"
		File.delete("TestResults/fedEx-results.trx") if File.exist?("TestResults/fedEx-results.trx")
		print "Executing FedEx tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"FedEx Tests\"", "/resultsfile:TestResults/fedEx-results.trx"
	end

	desc "Execute the iParcel tests"
	mstest :iParcel do |mstest|
		print "Deleting previous i-parcel test results...\r\n\r\n"
		File.delete("TestResults/iParcel-results.trx") if File.exist?("TestResults/iParcel-results.trx")
		print "Executing iParcel tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"iParcel Tests\"", "/resultsfile:TestResults/iParcel-results.trx"
	end

	desc "Execute the OnTrac tests"
	mstest :onTrac do |mstest|
		print "Deleting previous OnTrac test results...\r\n\r\n"
		File.delete("TestResults/onTrac-results.trx") if File.exist?("TestResults/onTrac-results.trx")
		print "Executing OnTrac tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"OnTrac Tests\"", "/resultsfile:TestResults/onTrac-results.trx"
	end

	desc "Execute the UPS tests"
	mstest :ups do |mstest|
		print "Deleting previous UPS open account test results...\r\n\r\n"
		File.delete("TestResults/ups-results.trx") if File.exist?("TestResults/ups-results.trx")
		print "Executing UPS tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"UPS Tests\"", "/resultsfile:TestResults/ups-results.trx"
	end
	
	desc "Execute the scheduled action tests"
	mstest :scheduledAction do |mstest|
		print "Deleting previous Scheduled Action test results...\r\n\r\n"
		File.delete("TestResults/scheduled-action-results.trx") if File.exist?("TestResults/scheduled-action-results.trx")
		print "Executing Scheduled Action tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"Scheduled Action Tests\"", "/resultsfile:TestResults/scheduled-action-results.trx"
	end

	desc "Execute the Newegg tests"
	mstest :newegg do |mstest|
		print "Deleting previous Newegg test results...\r\n\r\n"
		File.delete("TestResults/newegg-results.trx") if File.exist?("TestResults/newegg-results.trx")
		print "Executing Newegg tests...\r\n\r\n"
		Dir.mkdir("TestResults") if !Dir.exist?("TestResults")
		mstest.command = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE\\mstest.exe"		
		mstest.parameters "/testmetadata:ShipWorks.vsmdi", "/testlist:\"Newegg Tests\"", "/resultsfile:TestResults/newegg-results.trx"
	end
end
