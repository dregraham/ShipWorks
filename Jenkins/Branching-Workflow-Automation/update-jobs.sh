# The script below is used to build out the Jenkins artifacts when a new branch gets pushed to
# origin. A new view is created and a number of jobs get created to compile, test, analyze, etc.
# This should be run after the environment-setup.sh script is run.

# Create View
echo Feature name: $FEATURE_NAME

for CFG in $(ls ./Jenkins/Config/*_config.xml); do
	JOB=$(echo $(basename $CFG) | sed 's/_config.xml$//g')
	JOB=${FEATURE_NAME}-$JOB
	sed -i 's/@@BRANCH_NAME@@/'$BRANCH_NAME'/g' $CFG
	sed -i 's/@@FEATURE_NAME@@/'$FEATURE_NAME'/g' $CFG

	echo Uploading job configuration for $JOB"..."
	CRUMB=$(curl -q 'http://intdev1201:8080/crumbIssuer/api/xml?xpath=concat(//crumbRequestField,":",//crumb)')
	curl -X POST -H "Content-Type: text/xml" -H $CRUMB --upload-file $CFG http://intdev1201:8080/job/$JOB/config.xml > /dev/null
	echo $JOB configuration uploaded
done

echo Jenkins is ready to build $FEATURE_NAME