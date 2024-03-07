pipeline {
    agent any
    stages{
        stage ('Git Checkout') {
            steps {
                cleanWs()
                git branch: 'Développement', url: 'https://github.com/Bastien55/SocketBackend.git'
            }
        }

        stage('Restore packages') {
            steps {
                bat "dotnet restore ${workspace}\\SocketBackend.csproj"
            }
        }
        
        stage ('Build') {
            steps {
                bat "dotnet build ${workspace}\\SocketBackend.csproj"
            }
        }
    }
}