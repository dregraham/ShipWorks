def BUILD_FOLDER = env.BRANCH_NAME.replaceAll('/', '-').replaceAll(' ', '-')

pipeline {
	options {
    	disableConcurrentBuilds()
  	}
	environment {
		versionNumber = readFile ".build-label"
		tagName="ShipWorks_TEST_${env.versionNumber}"
		gitMsg="\"TEST - Jenkins Build ${env.tagName}\""
		gitTag="\"C:/Program Files/Git/bin/git.exe\" tag -a ${env.tagName} -m \"${env.gitMsg}\""
		gitPush="\"C:/Program Files/Git/bin/git.exe\" push https://github.com/shipworks/ShipWorks.git ${env.tagName}"
	}
	agent {
		node {
			label 'windows'
			customWorkspace "d:/jenkins-builds/SB_${BUILD_FOLDER}"
		}
	}
	stages {
		stage('Compile the solution') {
			steps {
				echo "Build on ${NODE_NAME}"
					echo "Start Tagging"
					//bat 'versionNumber="cat .build-label"'
					//bat 'env.versionNumber = readFile "output.txt"'
					echo "${env.versionNumber}"
					//bat 'env.tagName="ShipWorks_TEST_${env.versionNumber}"'
					echo "Tagging build as ${env.tagName}"
					echo "${env.gitMsg}"
					echo "${env.gitTag}"
					echo "${env.gitPush}"
					//bat '"C:/Program Files/Git/bin/git.exe" tag -a ${env.tagName} -m "TEST - Jenkins Build ${env.tagName}"'
					bat "${env.gitTag}"
					echo "Pushing tag to origin"
					//bat '"C:/Program Files/Git/bin/git.exe" push https://github.com/shipworks/ShipWorks.git ${env.tagName}'
					bat "${env.gitPush}"
				//bat 'bundle exec rake build:quick'
			}
		}
/*
		stage('Post-Compilation') {
			parallel {
				stage('Unit tests') {
					steps {
						bat 'bundle exec rake test:units'
					}
				}
				stage('NDepend') {
					steps {
						bat "c:\\tools\\NDepend\\NDepend.Console.exe ${WORKSPACE}\\NDepend\\ShipWorks.ndproj /Silent /Concurrent"
					}
				}
			}
		}
		stage('Integration tests') {
			parallel {
				stage('Integration tests') {
					steps {
						bat 'bundle exec rake test:integration[ContinuousIntegration]'
					}
				}
				stage('Specs') {
					steps {
						bat 'bundle exec rake test:specs'
					}
				}
			}
		}
*/
	}
	post {
		always {
					sh("echo `Start Tagging`")
					sh("versionNumber=`cat .build-label`")
					sh("tagName=`ShipWorks_TEST_$versionNumber`")
					sh("echo `Tagging build as $tagName`")
					sh("git tag -a $tagName -m `TEST - Jenkins Build $versionNumber`")
					sh("echo `Pushing tag to origin`")
					sh("git push https://github.com/shipworks/ShipWorks.git $tagName")
			step([$class: 'XUnitBuilder',
				    thresholds: [[$class: 'FailedThreshold', unstableThreshold: '1']],
				    tools: [[$class: 'XUnitDotNetTestType', pattern: 'TestResults/*.xml', failIfNotNew: true, deleteOutputFiles: true, stopProcessingIfError: true]]])
			publishHTML([allowMissing: true,
				alwaysLinkToLastBuild: false,
				keepAll: false,
				reportDir: 'NDepend\\NDependOut',
				reportFiles: 'NDependReport.html',
				reportName: 'NDepend Results',
				reportTitles: ''])
		}
	}
			/*
	post {
		always {
			emailext(body: '${DEFAULT_CONTENT}', mimeType: 'text/html',
		         replyTo: '$DEFAULT_REPLYTO', subject: '${DEFAULT_SUBJECT}',
		         to: 't.hughes@shipworks.com k.croke@shipworks.com a.benz@shipworks.com m.mulaosmanovic@shipworks.com c.hicks@shipworks.com j.winchester@shipworks.com')
		}
	}
		    */
}
