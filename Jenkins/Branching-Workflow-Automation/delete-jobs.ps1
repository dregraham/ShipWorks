# A PowerShell script that uses the Jenkins REST API to delete
# jobs and a view for a particular feature branch. The text of
# the script below is used in "Delete Branch Workflow" job in 
# Jenkins to automatically remove jobs when a branch is deleted 
# from Git. The intent of having this shell script in a file is
# to be able to put it under source control.

echo "Deleting jobs for $env:FEATURE_NAME"

$listJobsUrl = "http://intdev1201:8080/view/" + $env:FEATURE_NAME + "/api/xml?xpath=//job/name&wrapper=jobs"
echo $listJobsUrl 

# Make a request to obtain all of the jobs for the feature's view
[net.httpWebRequest] $request = [net.webRequest]::create($listJobsUrl)
[net.httpWebResponse] $response = $request.getResponse()

# Read the response into an XML variable
$responseStream = $response.GetResponseStream()
$streamReader = new-object IO.StreamReader($responseStream)
[xml] $jobXml = $streamReader.ReadToEnd()

# Read the job names from the XML ane make a request to Jenkins delete each job
$jobNames = Select-Xml "//jobs/name" $jobXml
$jobNames | ForEach-Object { 
    echo "Deleting job $_" 

    $deleteJobUrl = "http://intdev1201:8080/view/" + $env:FEATURE_NAME + "/job/$_/doDelete"
    echo $deleteJobUrl

    # $deleteJobUrl = "http://www.google.com/"
    [net.httpWebRequest] $jobRequest = [net.webRequest]::create($deleteJobUrl)
    $jobRequest.Method = "POST"

    $jobRequest.getResponse()
}

# URL to delete the feature's view in Jenkins
$deleteViewUrl = "http://intdev1201:8080/view/" + $env:FEATURE_NAME + "/doDelete"

# Make the request to delete the feature view
$request = [net.webRequest]::create($deleteViewUrl)
$request.Method = "POST"
$request.getResponse()


# Delete the source code directory from disk
$jobSourceDirectory = "\\intdev1201\C$\jenkins-builds\" + $env:FEATURE_NAME
if (Test-Path -Path $jobSourceDirectory)
{
    echo "Deleting source directory for $env:FEATURE_NAME..."
    rm -Force -Recurse $jobSourceDirectory
    echo "Source directory for $env:FEATURE_NAME has been deleted"
}