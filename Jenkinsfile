def BUILD_FOLDER = env.BRANCH_NAME.replaceAll('/', '-').replaceAll(' ', '-')

pipeline {
	options {
    	disableConcurrentBuilds()
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
				bat 'bundle exec rake rebuild[true]'
			}
		}
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
	}
	post {
		always {
			sshagent(credentials: ["shipworks_github"]) {
					//def repository = "git@" + env.GIT_URL.replaceFirst(".+://", "").replaceFirst("/", ":")
					//sh("git remote set-url origin $repository")
					//sh("git tag --force build-${env.BRANCH_NAME}")
					//sh("git push --force origin build-${env.BRANCH_NAME}")
					sh("versionNumber=`cat .build-label`")
					sh("tagName=`ShipWorks_TEST_$versionNumber`")
					sh("echo `Tagging build as $tagName`")
					sh("git tag -a $tagName -m `TEST - Jenkins Build $versionNumber`")
					sh("echo `Pushing tag to origin`")
					sh("git push https://github.com/shipworks/ShipWorks.git $tagName")
				}
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