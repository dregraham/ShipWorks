# The script below is used to build out the Jenkins artifacts when a new branch gets pushed to
# origin. A new view is created and a number of jobs get created to compile, test, analyze, etc.
# This should be run after the environment-setup.sh script is run.

# Create View
FEATURE_NAME="fedex-recert"
echo Feature name: $FEATURE_NAME

echo Submitting request to create view $FEATURE_NAME"..."
curl -d 'name='$FEATURE_NAME'&mode=hudson.model.ListView&json={"name": "'$FEATURE_NAME'", "mode": "hudson.model.ListView"}' http://intdev1201:8080/createView > /dev/null
echo $FEATURE_NAME view created

for CFG in $(ls ./Jenkins/Config/*_config.xml); do
	JOB=$(echo $(basename $CFG) | sed 's/_config.xml$//g')
	JOB=${FEATURE_NAME}-$JOB
	sed -i 's/@@BRANCH_NAME@@/'$BRANCH_NAME'/g' $CFG
	sed -i 's/@@FEATURE_NAME@@/'$FEATURE_NAME'/g' $CFG
	
	echo Uploading job configuration for $JOB"..."
	curl -X POST -H "Content-Type: text/xml" --upload-file $CFG http://intdev1201:8080/createItem?name=$JOB > /dev/null
	echo $JOB configuration uploaded
	
    # Add Job to View
	echo Adding $JOB to $FEATURE_NAME view...
    curl -d '' http://intdev1201:8080/view/$FEATURE_NAME/addJobToView?name=$JOB
	echo $JOB added to $FEATURE_NAME view
done

echo Jenkins is ready to build $FEATURE_NAME