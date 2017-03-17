pipeline {
    agent any
    stages {
        stage('build') {
            steps {
                bat 'powershell -file deploy.ps1'
            }
        }
    }
}