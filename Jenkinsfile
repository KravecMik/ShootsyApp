#!/usr/bin/env groovy
// Jenkinsfile (Declarative Pipeline)
pipeline {
  agent any
  stages {
    stage('Build') {
      steps {

        echo "Create build archive for project ${env.JOB_BASE_NAME}"
        sh "./build.sh"

      }
    }
  }
}
