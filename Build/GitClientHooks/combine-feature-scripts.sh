#!/bin/sh
#
# The goal is to merge/combine any scripts in the "feature" directory
# into a single script located in the latest version directory for script
# with a file name of [current version] + 1. For example, if the current 
# version is 3.7.0.1, then this will concatenate all the SQL files in the 
# feature directory into a 3.7.0.2 script in the 3.7 directory.
#
# This could be set up to run as a post-merge hook, or as a one-off command
# after merging a feature branch into the integration branch if a post-merge
# hook is not set up on the developer's machine. 

# It may be worth looking into enhancing the script at some point, so that 
# running it in a one-off manner, would allow you to specify the target version. 
# For example, rather than 3.7.0.2, you could specify 3.8.0.0 and the shell script 
# would take care of the rest, so that the SQL scripts get named 3.8.0.0.


featureDirectoryName="99.0"
currentBranch=$(git symbolic-ref --short HEAD)

if [ "$currentBranch" == "integration" ]; then
	# We're on the integration branch, so see if there are any 
	# feature/story scripts that need to be combined into a version
	# script
	
	# Make sure we're on the root directory of the branch and make
	# note of the absolute path since we'll need this later on
	relativeRoot=$(git rev-parse --show-cdup)
	cd "$relativeRoot"	
	absoluteRoot=$(pwd)
	
	# Go to the SQL update scripts directory to see if there 
	# is a feature directory
	cd ./Code/ShipWorks/Data/Administration/Scripts/Update
	if [ -d $featureDirectoryName ]; then
		echo "Trying to combine feature scripts into a version script..."	
				
		# Change to what is currently the latest version directory that has SQL 
		# scripts by grabbing the second directory in the list: contents are 
		# sorted in descending so our "feature" directory is the first item in 
		# the results; we want the second item (i.e. the latest version directory).
		latestVersionDirectory=$(ls -r | sed -n 2p)
		cd "$latestVersionDirectory"
		
		# Find what is currently the latest script and "increment" the file 
		# name to serve as the target file for the SQL scripts in the $featureDirectoryName
		# directory...
		
		# Parse each position looking for what is currently the latest version
		majorVersion=$(ls | awk -F'.' '{print $1}' | sort -n | tail -1)
		minorVersion=$(ls $majorVersion.* | awk -F'.' '{print $2}' | sort -n | tail -1)
		patchNumber=$(ls $majorVersion.$minorVersion.* | awk -F'.' '{print $3}' | sort -n | tail -1)
		revisionNumber=$(ls $majorVersion.$minorVersion.$patchNumber.* | awk -F'.' '{print $4}' | sort -n | tail -1)
		
		currentScriptVersion=$(ls $majorVersion.$minorVersion.$patchNumber.$revisionNumber.sql)
		echo The latest script is currently $currentScriptVersion
		
		# This will grab the latest revision and increment the last 
		# digit (e.g. 3.7.0.5.sql would yield "6" as in 3.7.0.6.sql)
		nextRevision=$(($revisionNumber + 1))
		
		# Now append the increment to the prefix to arrive at the file name
		# for the latest script name
		incrementedVersionFileName=$majorVersion.$minorVersion.$patchNumber.$nextRevision.sql		
				
		# We're already in the latest version directory, so move up a level and
		# concatenate the .sql files from the feature directory to a single
		# version script
		echo Combining all feature scripts into $latestVersionDirectory/$incrementedVersionFileName
		ls ../$featureDirectoryName/*.sql | xargs cat >> $incrementedVersionFileName
				
		# Need to update the .csproj file to include the new script file 
		# as an embedded resource
		echo Including the combined script into project file...		
				
		# Change to the Code/ShipWorks directory in preparation to edit the project file.		
		cd $absoluteRoot/Code/ShipWorks		
		
		# There are permissions errors when trying to edit the project file in place, 
		# so we need to write to a temp file and replace the previous file. (The new
		# embedded resource line is spaced to the left of the "sed..." so it is indented
		# correctly in the project file)
		sed -e '/.*Data\\Administration\\Scripts\\Update\\'$latestVersionDirectory'\\'$currentScriptVersion'.*/a\
	<EmbeddedResource Include="Data\\Administration\\Scripts\\Update\\'$latestVersionDirectory'\\'$incrementedVersionFileName'" />' ShipWorks.csproj > added-version.csproj
		
		# Remove any references to the "feature" directory from the project file
		echo Removing references to the feature scripts from project file...
		sed -e '/.*Data\\Administration\\Scripts\\Update\\'$featureDirectoryName'*/d' added-version.csproj > added-version-removed-feature.csproj
		
		# Delete the added-version project file and rename the second temp 
		# file back to ShipWorks.csproj
		rm -f added-version.csproj
		mv -f added-version-removed-feature.csproj ShipWorks.csproj
		
		# Delete the feature directory on disk		
		echo Project file updated successfully. Deleting feature directory from disk...
		cd $absoluteRoot
		rm -f ./Code/ShipWorks/Data/Administration/Scripts/Update/$featureDirectoryName/*
		rmdir ./Code/ShipWorks/Data/Administration/Scripts/Update/$featureDirectoryName
		
		# Add the new version script and the updated project file to the index
		# and remove the feature directory/scripts from the repository				
		echo Committing scripts to git...
		git add .
		git rm -r ./Code/ShipWorks/Data/Administration/Scripts/Update/$featureDirectoryName/
		git commit --quiet -m"Auto-merge feature scripts into a named version script		
		
- Add version script and include it as an embedded resource in the project file

- Remove the feature scripts that were combined into the version script"
		
		echo All feature scripts have been combined into a version script. Double check the log for accuracy.
	fi
fi
