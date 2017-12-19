# A PowerShell script that uses the Jenkins REST API to delete
# jobs and a view for a particular feature branch. The text of
# the script below is used in "Delete Branch Workflow" job in
# Jenkins to automatically remove jobs when a branch is deleted
# from Git. The intent of having this shell script in a file is
# to be able to put it under source control.

function Post-ToUrl($url)
{
    $crumb = Invoke-RestMethod -URI 'http://intdev1201:8080/crumbIssuer/api/xml?xpath=concat(//crumbRequestField,"=",//crumb)'
    Invoke-RestMethod -URI $url -Body $crumb -Method POST
}

function Delete-JobDirectory($path)
{
    echo "Trying to delete from $path"
    if (Test-Path -Path $path)
    {
        echo "Deleting source directory for $env:FEATURE_NAME..."
        rm -Force -Recurse $path
        echo "Source directory for $env:FEATURE_NAME has been deleted"
    }
}

echo "Deleting jobs for $env:FEATURE_NAME"

[xml] $jobXml = Invoke-RestMethod -URI "http://intdev1201:8080/view/$env:FEATURE_NAME/api/xml?xpath=//job/name&wrapper=jobs"

# Read the job names from the XML and make a request to Jenkins delete each job
$jobNames = Select-Xml "//jobs/name" $jobXml
$jobNames | ForEach-Object {
    echo "Deleting job $_"

    Post-ToUrl "http://intdev1201:8080/view/$env:FEATURE_NAME/job/$_/doDelete"
}

Post-ToUrl "http://intdev1201:8080/view/$env:FEATURE_NAME/doDelete"

# Delete the source code directory from disk for the common prefixes (feature, defect, spike)
Delete-JobDirectory "\\intdev1201\C$\jenkins-builds\feature-$env:FEATURE_NAME"
Delete-JobDirectory "\\intdev1201\C$\jenkins-builds\defect-$env:FEATURE_NAME"
Delete-JobDirectory "\\intdev1201\C$\jenkins-builds\spike-$env:FEATURE_NAME"
Delete-JobDirectory "\\intdev1201\C$\jenkins-builds\$env:FEATURE_NAME"