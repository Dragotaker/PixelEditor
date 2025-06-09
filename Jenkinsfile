pipeline {
    agent any
    
    environment {
        DOCKER_REGISTRY = 'localhost:5000'
        DOCKER_IMAGE = 'pixel-editor'
    }
    
    stages {
        stage('Check Environment') {
            steps {
                script {
                    // Проверка Docker
                    sh 'docker --version || echo "Docker not installed or not accessible"'
                    
                    // Проверка Minikube
                    sh 'minikube status || echo "Minikube not running or minikube command not found"'
                    
                    // Проверка kubectl
                    sh 'kubectl version --client || echo "kubectl not installed or not found"'
                }
            }
        }
        
        stage('Validate Credentials') {
            steps {
                script {
                    // Kubeconfig credentials are made available as a temporary file
                    withCredentials([file(credentialsId: 'kubeconfig', variable: 'KUBECONFIG_FILE_PATH')]) {
                        sh "echo Kubeconfig credentials found at ${KUBECONFIG_FILE_PATH}"
                    }
                    
                    // Docker registry credentials
                    withCredentials([usernamePassword(credentialsId: 'docker-registry-credentials', 
                                                    usernameVariable: 'DOCKER_USER', 
                                                    passwordVariable: 'DOCKER_PASSWORD')]) {
                        echo "Docker registry credentials found"
                    }
                }
            }
        }
        
        stage('Build') {
            steps {
                script {
                    if (!fileExists('Dockerfile')) {
                        error 'Dockerfile not found!'
                    }
                    
                    // Build the Docker image
                    sh "docker build -t ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER} ."
                }
            }
        }
        
        stage('Test') {
            steps {
                echo 'Running tests...'
                // Add your test commands here
            }
        }
        
        stage('Push') {
            steps {
                script {
                    // Login to Docker registry
                    withCredentials([usernamePassword(credentialsId: 'docker-registry-credentials', 
                                                    usernameVariable: 'DOCKER_USER', 
                                                    passwordVariable: 'DOCKER_PASSWORD')]) {
                        sh "docker login ${DOCKER_REGISTRY} -u ${DOCKER_USER} -p ${DOCKER_PASSWORD}"
                    }
                    
                    // Push the image
                    sh "docker push ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER}"
                    sh "docker tag ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER} ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:latest"
                    sh "docker push ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:latest"
                }
            }
        }
        
        stage('Deploy') {
            steps {
                script {
                    withCredentials([file(credentialsId: 'kubeconfig', variable: 'KUBECONFIG_FILE_PATH')]) {
                        sh """
                            export KUBECONFIG=${KUBECONFIG_FILE_PATH}
                            kubectl config use-context minikube
                            kubectl apply -f k8s/deployment.yaml
                            kubectl set image deployment/pixel-editor pixel-editor=${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER}
                            kubectl rollout status deployment/pixel-editor
                        """
                    }
                }
            }
        }
    }
    
    post {
        always {
            script {
                node { // Ensure cleanup runs within a node context
                    // Cleanup
                    sh "docker rmi ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${BUILD_NUMBER} || true"
                }
            }
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
} 