# This is the first script that should be run to automate the creation of Jenkins jobs
# when a new branch is pushed to origin. 

# Build the name of the registry key based on the workspace directory and the feature name
KEY=$(echo C:\\jenkins-builds\\@@BRANCH_NAME@@\\Artifacts\\Application | sed  -e 's/@@BRANCH_NAME@@/'$BRANCH_NAME'/')

# Use the rake task to add the registry key with a randomly generated GUID value
# and write out the SQL session file to point to the seed database
rake setup:registry[$KEY] "setup:sqlSession[$KEY,ShipWorks_SeedData]"