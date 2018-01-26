def BUILD_FOLDER = env.BRANCH_NAME.replaceAll('/', '-').replaceAll(' ', '-')

pipeline {
	agent {
		node {
			label 'windows'
			customWorkspace "c:/jenkins-builds/SB_${BUILD_FOLDER}"
		}
	}
	stages {
		stage('Compile the solution') {
			steps {
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
						publishHTML([allowMissing: true,
							alwaysLinkToLastBuild: false,
							keepAll: false,
							reportDir: 'NDepend\\NDependOut',
							reportFiles: 'NDependReport.html',
							reportName: 'NDepend Results',
							reportTitles: ''])

					}
				}
			}
		}
		stage('Integration tests') {
			steps {
				bat 'bundle exec rake test:integration[ContinuousIntegration]'
			}
		}
	}
	post {
		always {
			step([$class: 'XUnitBuilder',
				    thresholds: [[$class: 'FailedThreshold', unstableThreshold: '1']],
				    tools: [[$class: 'XUnitDotNetTestType', pattern: 'TestResults/*.xml', failIfNotNew: true, deleteOutputFiles: true, stopProcessingIfError: true]]])
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